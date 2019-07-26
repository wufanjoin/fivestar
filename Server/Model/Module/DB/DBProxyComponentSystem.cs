using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace ETModel
{
	[ObjectSystem]
	public class DbProxyComponentSystem : AwakeSystem<DBProxyComponent>
	{
		public override void Awake(DBProxyComponent self)
		{
			self.Awake();
		}
	}
	
	/// <summary>
	/// 用来与数据库操作代理
	/// </summary>
	public static class DBProxyComponentEx
	{
		public static void Awake(this DBProxyComponent self)
		{
			StartConfig dbStartConfig = StartConfigComponent.Instance.DBConfig;
			self.dbAddress = dbStartConfig.GetComponent<InnerConfig>().IPEndPoint;
		}

		public static async Task Save(this DBProxyComponent self, ComponentWithId component, bool needCache = true)
		{
			Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
			await session.Call(new DBSaveRequest { Component = component, NeedCache = needCache});
		}

        private static List<ComponentWithId> SaveComponentss=new List<ComponentWithId>();
	    public static async Task Save<T>(this DBProxyComponent self, List<T> componentsT, bool needCache = true) where T:ComponentWithId

        {
	        Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
            SaveComponentss.Clear();
            for (int i = 0; i < componentsT.Count; i++)
            {
                SaveComponentss.Add(componentsT[i]);
            }
            await session.Call(new DBSaveBatchRequest { Components = SaveComponentss, NeedCache = needCache });
	    }

        public static async Task SaveBatch(this DBProxyComponent self, List<ComponentWithId> components, bool needCache = true)
		{
			Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
			await session.Call(new DBSaveBatchRequest { Components = components, NeedCache = needCache});
		}

		public static async Task Save(this DBProxyComponent self, ComponentWithId component, bool needCache, CancellationToken cancellationToken)
		{
			Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
			await session.Call(new DBSaveRequest { Component = component, NeedCache = needCache}, cancellationToken);
		}

		public static async void SaveLog(this DBProxyComponent self, ComponentWithId component)
		{
			Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
			await session.Call(new DBSaveRequest { Component = component,  NeedCache = false, CollectionName = "Log" });
		}

		public static async Task<T> Query<T>(this DBProxyComponent self, long id, bool needCache = true) where T: ComponentWithId
		{
			Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
			DBQueryResponse dbQueryResponse = (DBQueryResponse)await session.Call(new DBQueryRequest { CollectionName = typeof(T).Name, Id = id, NeedCache = needCache });
			return (T)dbQueryResponse.Component;
		}
		
		/// <summary>
		/// 根据查询表达式查询
		/// </summary>
		/// <param name="self"></param>
		/// <param name="exp"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static async Task<List<T>> Query<T>(this DBProxyComponent self, Expression<Func<T ,bool>> exp) where T: ComponentWithId
		{
			ExpressionFilterDefinition<T> filter = new ExpressionFilterDefinition<T>(exp);
			IBsonSerializerRegistry serializerRegistry = BsonSerializer.SerializerRegistry;
			IBsonSerializer<T> documentSerializer = serializerRegistry.GetSerializer<T>();
			string json = filter.Render(documentSerializer, serializerRegistry).ToJson();
			return await self.Query<T>(json);
		}

        public static async Task<List<ComponentWithId>> Query<T>(this DBProxyComponent self, List<long> ids, bool needCache = true) where T : ComponentWithId
		{
			Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
			DBQueryBatchResponse dbQueryBatchResponse = (DBQueryBatchResponse)await session.Call(new DBQueryBatchRequest { CollectionName = typeof(T).Name, IdList = ids, NeedCache = needCache});
			return dbQueryBatchResponse.Components;
		}

		/// <summary>
		/// 根据json查询条件查询
		/// </summary>
		/// <param name="self"></param>
		/// <param name="json"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static async Task<List<T>> Query<T>(this DBProxyComponent self, string json) where T : ComponentWithId
		{
			Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
			DBQueryJsonResponse dbQueryJsonResponse = (DBQueryJsonResponse)await session.Call(new DBQueryJsonRequest { CollectionName = typeof(T).Name, Json = json });
            return ConversionType<T>(dbQueryJsonResponse.Components);
		}

	    public static List<T> ConversionType<T>(List<ComponentWithId> componentWithIds) where T : ComponentWithId
        {
            if (componentWithIds == null)
            {
                return null;
            }
            List<T> listt=new List<T>(){};
            foreach (var componentWithId in componentWithIds)
            {
                listt.Add(componentWithId as T);
            }
            return listt;
        }
        //Expression表达式转换时monongb语句
        private static string ExpressionConversionJson<T>(Expression<Func<T, bool>> exp)
        {
            ExpressionFilterDefinition<T> filter = new ExpressionFilterDefinition<T>(exp);
            IBsonSerializerRegistry serializerRegistry = BsonSerializer.SerializerRegistry;
            IBsonSerializer<T> documentSerializer = serializerRegistry.GetSerializer<T>();
            return filter.Render(documentSerializer, serializerRegistry).ToJson();
        }

        /// <summary>
        /// 根据json查询条件查询
        /// </summary>
        /// <param name="self"></param>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<List<T>> SortQuery<T>(this DBProxyComponent self, Expression<Func<T, bool>> queryExp, Expression<Func<T, bool>> sortExp, int count) where T : ComponentWithId
        {
            string queryJson = ExpressionConversionJson(queryExp);
            string sortJson = ExpressionConversionJson(sortExp);
            return await self.SortQuery<T>(typeof(T).Name, queryJson, sortJson, count);
        }

        /// <summary>
        /// 根据json查询条件查询
        /// </summary>
        /// <param name="self"></param>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<List<T>> SortQuery<T>(this DBProxyComponent self,string typeName,string queryJson, string sortJson, int count) where T : ComponentWithId
        {
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
            DBQueryJsonResponse dbQueryJsonResponse = (DBQueryJsonResponse)await session.Call(new DBSortQueryJsonRequest { CollectionName = typeName, QueryJson = queryJson, SortJson = sortJson, Count = count });
            return ConversionType<T>(dbQueryJsonResponse.Components);
        }
    }
}