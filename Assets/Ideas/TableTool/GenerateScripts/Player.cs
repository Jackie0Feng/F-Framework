// **********************************************************************
// This file was auto generated
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;

namespace FFramework.Ideas.Tool.Database
{
	public class PlayerData//表的元数据类
	{
		public  int Key;
		public  int Level;
		public  int Hp;
		public  int Exp;

		public override string ToString()
		{
			return $"Player\n{{\n\tKey: {Key}\n\tLevel: {Level}\n\tHp: {Hp}\n\tExp: {Exp}\n}}";
		}		
	}

	public class PlayerDatabase: IDatabase
	{
		public const uint TYPE_ID = 3;
		public const string DATA_PATH = "CsvResources/Player";

		private PlayerData _tempData = new PlayerData();
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

		public PlayerData GetDataByKey(int key)//通过键值获取数据
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

		
			if(!int.TryParse(_datas[i][1], out _tempData.Level))
			{
				_tempData.Level = 0;
			}

		
			if(!int.TryParse(_datas[i][2], out _tempData.Hp))
			{
				_tempData.Hp = 0;
			}

		
			if(!int.TryParse(_datas[i][3], out _tempData.Exp))
			{
				_tempData.Exp = 0;
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
