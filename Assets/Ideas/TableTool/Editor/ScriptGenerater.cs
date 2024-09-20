using Codice.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace FFramework.Ideas
{
	public class ScriptGenerater
	{
		private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/Ideas/TableTool/GenerateScripts/";

		private static string EDITOR_PATH = Application.dataPath + "/Ideas/TableTool/Editor/";

		private static string TEMPLATE_IDATABASE_PATH = "Assets/Ideas/TableTool/Editor/Template_IDatabase.txt";

		private static string TEMPLATE_DATABASE_PATH = "Assets/Ideas/TableTool/Editor/Template_Database.txt";

		private static string TEMPLATE_DATABASEMANAGER_PATH = "Assets/Ideas/TableTool/Editor/Template_DatabaseManager.txt";

		private static string CSV_PATH = Application.dataPath + "/Ideas/TableTool/Resources/CsvResources/";

		private static int DATA_ID;

		private static string REGISTER_LIST;//用来替换DatabaseManager里生成类的代码

		private static string CONVERT_LIST;//用来替换DatabaseManager里生成类的代码

		private static char[] _separaters = new char[] { '.', '/' };

		[MenuItem("TableTool/GenerateScript")]
		public static void GenerateScript()
		{
			Initialize();
			CreateIDatabaseScript();
			CreateMetaScript();
			CreateDatabaseManagerScript();
		}

		private static void CreateDatabaseManagerScript()
		{
			string template = GetTemplate(TEMPLATE_DATABASEMANAGER_PATH);
			template = template.Replace("$RegisterList", REGISTER_LIST);
			GenerateScriptFile("DatabaseManager", template);
		}

		/// <summary>
		/// 解析CSV文件表头生成类文本
		/// </summary>
		private static void CreateMetaScript()
		{
			string[] csvPaths = Directory.GetFiles(CSV_PATH, "*.csv", SearchOption.AllDirectories);
			string assetPath = "";
			TextAsset textAsset = null;

			for (int i = 0; i < csvPaths.Length; i++)
			{
				assetPath = "Assets" + csvPaths[i].Replace(Application.dataPath, "").Replace('\\', '/');
				textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

				REGISTER_LIST += string.Format("RegisterDataType(new {0}Database());\n", textAsset.name);
				if (i != csvPaths.Length - 1)
					REGISTER_LIST += "\t\t\t";
				CONVERT_LIST += string.Format("CsvToJsonConverter.Convert<{0}Data>(\"{0}\"); \n", textAsset.name);
				if (i != csvPaths.Length - 1)
					CONVERT_LIST += "\t\t\t";

				GenerateMetaScript(textAsset);
			}
		}

		/// <summary>
		/// CSV文件表头替换标识符生成类文本
		/// </summary>
		/// <param name="textAsset"></param>
		private static void GenerateMetaScript(TextAsset textAsset)
		{
			DATA_ID++;

			string template = GetTemplate(TEMPLATE_DATABASE_PATH);//获取数据类替换模版
			template = template.Replace("$DataClassName", textAsset.name + "Data");
			template = template.Replace("$DataAttributes", GetClassParameters(textAsset));
			template = template.Replace("$DataToString", GetParametersString(textAsset));
			template = template.Replace("$CsvSerialize", GetCsvSerialize(textAsset));
			template = template.Replace("$DataTypeName", textAsset.name + "Database");
			template = template.Replace("$DataID", DATA_ID.ToString());
			template = template.Replace("$DataPath", "\"CsvResources/" + textAsset.name + "\"");

			GenerateScriptFile(textAsset.name, template);
		}

		/// <summary>
		/// 生成ToStirng代码，方便打印数据类
		/// 良好格式化
		/// </summary>
		/// <param name="textAsset"></param>
		/// <returns></returns>
		private static string GetParametersString(TextAsset textAsset)
		{
			StringBuilder result = new StringBuilder();

			result.Append("$\"");
			result.Append(textAsset.name + "\\n");
			result.Append("{{\\n");

			string[] csvParameter = CSVConverter.SerializeCSVMetaString(textAsset);
			for (int i = 0; i < csvParameter.Length; i++)
			{
				string csv = csvParameter[i];
				string[] attributes = csv.Split(_separaters, options: StringSplitOptions.RemoveEmptyEntries);
				if (attributes[0].EndsWith("[]"))//数组
				{
					result.Append($"\\t{attributes[1]}: {{CSVConverter.GetArrayString({attributes[1]})}}\\n");
				}
				else
					result.Append($"\\t{attributes[1]}: {{{attributes[1]}}}\\n");
			}

			result.Append("}}\"");
			return result.ToString();
		}



		private static string GetCsvSerialize(TextAsset textAsset)
		{
			string[] csvParameter = CSVConverter.SerializeCSVMetaString(textAsset);

			int keyCount = csvParameter.Length;

			string csvSerialize = string.Empty;

			for (int i = 0; i < keyCount; i++)
			{
				string[] attributes = csvParameter[i].Split(new char[] { '/', '.' }, System.StringSplitOptions.RemoveEmptyEntries);

				if (attributes[0] == "string")
				{
					csvSerialize += string.Format("_tempData.{0} = _datas[i][{1}];", attributes[1], i);
				}
				else if (attributes[0] == "bool")
				{
					csvSerialize += GetCsvSerialize(attributes, i, "0");
				}
				else if (attributes[0] == "int")
				{
					csvSerialize += GetCsvSerialize(attributes, i, "0");
				}
				else if (attributes[0] == "float")
				{
					csvSerialize += GetCsvSerialize(attributes, i, "0.0f");
				}
				else if (attributes[0] == "string[]")
				{
					csvSerialize += string.Format("_tempData.{0} = CSVConverter.ConvertToArray<string>(_datas[i][{1}]);",
						attributes[1], i);
				}
				else if (attributes[0] == "bool[]")
				{
					csvSerialize += string.Format("_tempData.{0} = CSVConverter.ConvertToArray<bool>(_datas[i][{1}]);",
						attributes[1], i);
				}
				else if (attributes[0] == "int[]")
				{
					csvSerialize += string.Format("_tempData.{0} = CSVConverter.ConvertToArray<int>(_datas[i][{1}]);",
						attributes[1], i);
				}
				else if (attributes[0] == "float[]")
				{
					csvSerialize += string.Format("_tempData.{0} = CSVConverter.ConvertToArray<float>(_datas[i][{1}]);",
						attributes[1], i);
				}

				if (i != keyCount - 1)
				{
					csvSerialize += "\n";
					csvSerialize += "\t\t";
				}
			}

			return csvSerialize;
		}

		private static string GetCsvSerialize(string[] attributes, int arrayCount, string defaultValue)
		{
			string csvSerialize = "";
			csvSerialize += string.Format("\n\t\t\tif(!{0}.TryParse(_datas[i][{1}], out _tempData.{2}))\n", attributes[0], arrayCount, attributes[1]);

			csvSerialize += "\t\t\t{\n";
			csvSerialize += string.Format("\t\t\t\t_tempData.{0} = {1};\n", attributes[1], defaultValue);
			csvSerialize += "\t\t\t}\n";

			return csvSerialize;

		}

		/// <summary>
		/// 解析表头数据类型和标识符
		/// </summary>
		/// <param name="textAsset"></param>
		/// <returns></returns>
		private static string GetClassParameters(TextAsset textAsset)
		{
			string[] csvParameter = CSVConverter.SerializeCSVMetaString(textAsset);
			int keyCount = csvParameter.Length;

			string classParameters = string.Empty;//实际类属性字符串

			for (int i = 0; i < keyCount; i++)
			{
				string[] attributes = csvParameter[i].Split(new char[] { '/', '.' }, System.StringSplitOptions.RemoveEmptyEntries);

				classParameters += $"public  {attributes[0]} {attributes[1]};";

				if (i != keyCount - 1)
				{
					classParameters += "\n";
					classParameters += "\t\t";
				}
			}
			return classParameters;
		}

		/// <summary>
		/// 初始化一些数据，创建自动化脚本文件夹
		/// </summary>

		private static void Initialize()
		{
			DATA_ID = 0;
			REGISTER_LIST = string.Empty;
			CONVERT_LIST = string.Empty;

			if (Directory.Exists(GENERATE_SCRIPT_PATH))
			{
				Directory.Delete(GENERATE_SCRIPT_PATH, true);
			}
			Directory.CreateDirectory(GENERATE_SCRIPT_PATH);
		}

		private static void CreateIDatabaseScript()
		{
			string template = GetTemplate(TEMPLATE_IDATABASE_PATH);
			GenerateScriptFile("IDatabase", template);
		}

		/// <summary>
		/// 根据字符串生成脚本文件
		/// </summary>
		/// <param name="scriptName"></param>
		/// <param name="data"></param>
		/// <exception cref="NotImplementedException"></exception>
		private static void GenerateScriptFile(string scriptName, string data)
		{
			string path = GENERATE_SCRIPT_PATH + scriptName + ".cs";
			if (File.Exists(path)) { File.Delete(path); }

			StreamWriter sr = File.CreateText(path);
			sr.WriteLine(data);
			sr.Close();

			Debug.Log(path + "has been generated");
		}

		/// <summary>
		/// 加载模版txt文件返回字符串
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private static string GetTemplate(string path)
		{
			TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
			return textAsset.text;
		}
	}
}
