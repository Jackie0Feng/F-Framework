using UnityEngine;
using System.Collections.Generic;

namespace FFramework.Ideas.Tool.Database
{
	public class WeaponData//表的元数据类
	{
		public string Key;
		public string Name;
		public int[] Atk;
		public string[] Rarity;
	}

	public class WeaponDatabase : IDatabase
	{
		public const uint TYPE_ID = 4;
		public const string DATA_PATH = "CsvResources/Weapon";

		private WeaponData _tempData = new WeaponData();
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

		public WeaponData GetDataByKey(string key)//通过键值获取数据
		{
			for (int i = 0; i < _datas.Length; i++)
			{
				if (_datas[i][0] == key)
				{
					_tempData.Key = _datas[i][0];
					_tempData.Name = _datas[i][1];
					_tempData.Atk = CSVConverter.ConvertToArray<int>(_datas[i][2]);
					_tempData.Rarity = CSVConverter.ConvertToArray<string>(_datas[i][3]);//序列化数据到数据类里

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
