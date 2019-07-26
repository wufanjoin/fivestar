using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ETModel
{
    [ObjectSystem]
    public class DBSortQueryJsonTaskAwakeSystem : AwakeSystem<DBSortQueryJsonTask, string[],int,TaskCompletionSource<List<ComponentWithId>>>
    {
        public override void Awake(DBSortQueryJsonTask self, string[] strs,int count, TaskCompletionSource<List<ComponentWithId>> tcs)
        {
            self.CollectionName = strs[0];
            self.QueryJson = strs[1];
            self.SortJson = strs[2];
            self.Count = count;
            self.Tcs = tcs; 
        }
    }

    public sealed class DBSortQueryJsonTask : DBTask
    {
        public string CollectionName { get; set; }

        public string QueryJson { get; set; }

        public string SortJson { get; set; }

        public int Count { get; set; }

        public TaskCompletionSource<List<ComponentWithId>> Tcs { get; set; }

        public override async Task Run()
        {
            DBComponent dbComponent = Game.Scene.GetComponent<DBComponent>();
            try
            {
                // 执行查询数据库任务
              //  FilterDefinition<ComponentWithId> filterDefinition = new JsonFilterDefinition<ComponentWithId>(this.QueryJson);
              //  IAsyncCursor<ComponentWithId> cursor = await dbComponent.GetCollection(this.CollectionName).FindAsync(filterDefinition);
               // List<ComponentWithId> components = await cursor.ToListAsync();
              //  this.Tcs.SetResult(components);

                FilterDefinition<ComponentWithId> filterDefinition = new JsonFilterDefinition<ComponentWithId>(this.QueryJson);
                SortDefinition<ComponentWithId> sortDefinition = new JsonSortDefinition<ComponentWithId>(this.SortJson);
                IFindFluent<ComponentWithId, ComponentWithId> ifindiluent = dbComponent.GetCollection(this.CollectionName).Find(filterDefinition).Sort(sortDefinition).Limit(this.Count);
                List<ComponentWithId> components = await ifindiluent.ToCursor().ToListAsync();
                this.Tcs.SetResult(components);
            }
            catch (Exception e)
            {
                this.Tcs.SetException(new Exception($"查询数据库异常! {CollectionName} {this.QueryJson}", e));
            }
        }
    }
}