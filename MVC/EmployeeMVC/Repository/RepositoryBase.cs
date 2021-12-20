﻿using EmployeeMVC.Models;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EmployeeMVC.Util;
using EmployeeMVC.Data;

namespace EmployeeMVC.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApiDbContext RepositoryContext { get; set; }
        public string? OldObjString { get; set; }

        public RepositoryBase(ApiDbContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }
        public EntityEntry EntryObject(T objT)
        {

            return this.RepositoryContext.Entry(objT);
        }
        public T? FindByID(long ID)
        {
            return this.RepositoryContext.Set<T>().Find(ID);
        }
        public T? FindByID(int ID)
        {
            return this.RepositoryContext.Set<T>().Find(ID);
        }
        public async Task<T?> FindByIDAsync(long ID)
        {
            return await this.RepositoryContext.Set<T>().FindAsync(ID);
        }

        public async Task<T?> FindByIDAsync(int ID)
        {
            return await this.RepositoryContext.Set<T>().FindAsync(ID);
        }

        public async Task<T?> FindByIDAsync(string ID)
        {
            return await this.RepositoryContext.Set<T>().FindAsync(ID);
        }

        public async Task<T?> FindByDateAsync(DateTime date)
        {
            return await this.RepositoryContext.Set<T>().FindAsync(date);
        }
        
        public T? FindByCompositeID(long ID1, long ID2)
        {
            return this.RepositoryContext.Set<T>().Find(ID1, ID2);
        }
        public T? FindByCompositeID(int ID1, int ID2)
        {
            return this.RepositoryContext.Set<T>().Find(ID1, ID2);
        }

        public async Task<T?> FindByCompositeIDAsync(long ID1, long ID2)
        {
            return await this.RepositoryContext.Set<T>().FindAsync(ID1, ID2);
        }

        public bool AnyByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Any(expression);
        }
        public async Task<bool> AnyByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await this.RepositoryContext.Set<T>().AnyAsync(expression);
        }
        public IEnumerable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().ToList();
        }
        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await this.RepositoryContext.Set<T>().ToListAsync();
        }
        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression);
        }
        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await  this.RepositoryContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<bool> IsExist(string ID)
        {
            
            var obj = await this.RepositoryContext.Set<T>().FindAsync(ID);
            if(obj != null)
                return true;
            else
                return false;
        }

        public async Task<bool> IsExist(int ID)
        {
            
            var obj = await this.RepositoryContext.Set<T>().FindAsync(ID);
            if(obj != null)
                return true;
            else
                return false;
        }

        public async Task<bool> IsExist(DateTime date)
        {
            
            var obj = await this.RepositoryContext.Set<T>().FindAsync(date);
            if(obj != null)
                return true;
            else
                return false;
        }

        public void Create(dynamic entity, bool flush = true)
        {
            //entity.SetEventLogMessage(this.SetOldObjectToString(entity)); //it is not necessary for create record, there is no old record in create. 
            this.RepositoryContext.Set<T>().Add(entity);
            if (flush) this.Save();
        }

        public void Update(dynamic entity, bool flush = true)
        {
            // entity.SetEventLogMessage(this.GetUpdateEventLogString(entity));
            this.RepositoryContext.Set<T>().Update(entity);
            if (flush) this.Save();
        }

        public void Delete(dynamic entity, bool flush = true)
        {
            // entity.SetEventLogMessage(this.SetOldObjectToString(entity));
            this.RepositoryContext.Set<T>().Remove(entity);
            if (flush) this.Save();
        }

        public void Save()
        {
            this.RepositoryContext.SaveChanges();
        }

        
        public async Task CreateAsync(dynamic entity, bool flush = true)
        {
            this.RepositoryContext.Set<T>().Add(entity);
            if (flush) await this.SaveAsync();
        }

        public async Task UpdateAsync(dynamic entity, bool flush = true)
        {
            this.RepositoryContext.Set<T>().Update(entity);
            if (flush) await this.SaveAsync();
        }

        public async Task DeleteAsync(dynamic entity, bool flush = true)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
            if (flush) await this.SaveAsync();
        }

        public async Task SaveAsync()
        {
            await this.RepositoryContext.SaveChangesAsync();
        }

        public string SetOldObjectToString(dynamic OldObj)
        {
            OldObjString = "";
            JObject _duplicateObj = JObject.FromObject(OldObj);
            var _List = _duplicateObj.ToObject<Dictionary<string, object>>();
            foreach (var msg in from item in _List
                                let name = item.Key
                                let val = item.Value
                                let msg = name + " : " + val + "\r\n"
                                select msg)
            {
                OldObjString += msg;
            }

            return OldObjString;
        }

        public string? GetOldObjectString()
        {
            return this.OldObjString;
        }
        
        public String GetUpdateEventLogString(dynamic entity)
        {
            PropertyValues? oldObj;
            string _OldObjString = "";
            try
            {
                oldObj = this.RepositoryContext.Entry(entity).OriginalValues;
                if (oldObj == null) return "";
                JObject _newObj = JObject.FromObject(entity);
                var _newList = _newObj.ToObject<Dictionary<string, object>>();

                foreach (var item in oldObj.Properties)
                {
                    var name = item.Name;
                    var val = oldObj[name] == null ? "" : oldObj[name]!.ToString()!.Trim();
                    var newval = _newList!.GetValueOrDefault(name) != null ? _newList!.GetValueOrDefault(name)!.ToString()!.Trim() : "";
                    string msg = "";
                    if(val != newval || item.IsKey()) msg = name + " : " + val + " >>> " + newval + "\r\n";   //include primary key and changes fields only
                    _OldObjString += msg;
                }
            }
            catch (Exception ex)
            {
                Globalfunction.WriteSystemLog("Exception :" + ex.Message);
            }
            return _OldObjString;
        }
       
    }
}
