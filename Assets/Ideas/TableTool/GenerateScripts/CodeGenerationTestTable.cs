using UnityEngine;
using System.Collections.Generic;

namespace FFramework.Ideas.Tool.Database
{
	public class CodeGenerationTestTableData//表的元数据类
	{
		public  string ID;
		public  string[] Name;
		public  string Des;
		public  int HP;
		public  int Exp;
		public  string Equment;
		public  int Num;
	}

	public class CodeGenerationTestTableDatabase : IDatabase
	{
		public const uint TYPE_ID = 1;
		public const string DATA_PATH = "CsvResources/CodeGenerationTestTable";

		private CodeGenerationTestTableData _tempData = new CodeGenerationTestTableData();
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

		public CodeGenerationTestTableData GetDataByKey(string key)//通过键值获取数据
		{
			for(int i = 0; i < _datas.Length; i ++)
			{
				if(_datas[i][0] == key)
				{
					_tempData.ID = _datas[i][0];
		_tempData.Name = CSVConverter.ConvertToArray<string>(_datas[i][1]);
		_tempData.Des = _datas[i][2];
		
			if(!int.TryParse(_datas[i][3], out _tempData.HP))
			{
				_tempData.HP = 0;
			}

		
			if(!int.TryParse(_datas[i][4], out _tempData.Exp))
			{
				_tempData.Exp = 0;
			}

		_tempData.Equment = _datas[i][5];
		
			if(!int.TryParse(_datas[i][6], out _tempData.Num))
			{
				_tempData.Num = 0;
			}
//序列化数据到数据类里

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
