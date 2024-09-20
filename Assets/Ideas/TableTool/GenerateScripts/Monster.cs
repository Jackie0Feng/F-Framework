// **********************************************************************
// This file was auto generated
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;

namespace FFramework.Ideas.Tool.Database
{
	public class MonsterData//表的元数据类
	{
		public  int Key;
		public  string Name;
		public  int Hp;

		public override string ToString()
		{
			return $"Monster\n{{\n\tKey: {Key}\n\tName: {Name}\n\tHp: {Hp}\n}}";
		}		
	}

	public class MonsterDatabase: IDatabase
	{
		public const uint TYPE_ID = 2;
		public const string DATA_PATH = "CsvResources/Monster";

		private MonsterData _tempData = new MonsterData();
		private string[][] _datas;

		public uint GetTypeID()
		{
			return TYPE_ID;
		}

		public string GetDataPath()
		{
			return DATA_PATH;
		}

		public void LoadResources()
		{
			TextAsset textAsset = Resources.Load<TextAsset>(GetDataPath());
			_datas = CSVConverter.SerializeCSVDataString(textAsset);
		}

		public MonsterData GetDataByKey(int key)//通过键值获取数据
		{
			for (int i = 0; i < _datas.Length; i++)
			{
				if (_datas[i][0] == key.ToString())
				{
					//序列化数据到数据类里
					
			if(!int.TryParse(_datas[i][0], out _tempData.Key))
			{
				_tempData.Key = 0;
			}

		_tempData.Name = _datas[i][1];
		
			if(!int.TryParse(_datas[i][2], out _tempData.Hp))
			{
				_tempData.Hp = 0;
			}


					return _tempData;
				}
			}
			return null;
		}

		public int GetCount()
		{
			return _datas.Length;
		}
	}
}
