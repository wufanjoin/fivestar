using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ETModel
{
	[ObjectSystem]
	public class DbComponentSystem : AwakeSystem<DBComponent>
	{
		public override void Awake(DBComponent self)
		{
			self.Awake();
		}
	}

	/// <summary>
	/// 连接mongodb
	/// </summary>
	public class DBComponent : Component
	{
		public MongoClient mongoClient;
		public IMongoDatabase database;

		public void Awake()
		{
            DBConfig config = Game.Scene.GetComponent<StartConfigComponent>().StartConfig.GetComponent<DBConfig>();
            string connectionString = config.ConnectionString;
            mongoClient = new MongoClient(connectionString);
            this.database = this.mongoClient.GetDatabase(config.DBName);
        }

		public IMongoCollection<ComponentWithId> GetCollection(string name)
		{
			return this.database.GetCollection<ComponentWithId>(name);
		}

	    public async void GetCollection123456(string name)
	    {
	        IMongoCollection<ComponentWithId> ins = this.database.GetCollection<ComponentWithId>(name);
	        FilterDefinition<ComponentWithId> filterDefinition = new JsonFilterDefinition<ComponentWithId>("{}");
            SortDefinition<ComponentWithId> sortDefinition = new JsonSortDefinition<ComponentWithId>("{\"UserId\":1}");
            IFindFluent<ComponentWithId, ComponentWithId> s =ins.Find(filterDefinition).Sort(sortDefinition).Limit(1);
            List<ComponentWithId> components33 = await s.ToCursor().ToListAsync();

	    }
    }
}