// **********************************************************************
// This file was auto generated
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;

namespace FFramework.Ideas.Tool.Database
{
    public class DatabaseManager
    {
        private Dictionary<uint,IDatabase> _tables;

        public DatabaseManager()
        {
            _tables = new Dictionary<uint, IDatabase>();

            RegisterDataType(new CodeGenerationTestTableDatabase());
			RegisterDataType(new MonsterDatabase());
			RegisterDataType(new PlayerDatabase());
			RegisterDataType(new WeaponDatabase());


            Load();
        }

        public void Load()
        {
            foreach(KeyValuePair<uint , IDatabase> table in _tables)
            {
                table.Value.LoadResources();
            }
        }

        public T GetTable<T>() where T : IDatabase, new()
        {
            T result = new T();
            if(_tables.ContainsKey(result.GetTypeID()))
            {
                return (T)_tables[result.GetTypeID()];
            }

            return default(T);
        }

        private void RegisterDataType(IDatabase database)
        {
            _tables.TryAdd(database.GetTypeID(), database);
        }
    }
}
