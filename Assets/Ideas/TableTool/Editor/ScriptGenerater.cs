using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

		private static string REGISTER_LIST;

		private static string CONVERT_LIST;

		[MenuItem("TableTool/Generate Script")]
		public static void GenerateScript()
		{
			Initialize();
			CreateIDatabaseScript();
			CreateMetaScript();
		}

		/// <summary>
		/// 解析CSV文件表头生成类文本
		/// </summary>
		private static void CreateMetaScript()
		{
			string[] csvPaths = Directory.GetFiles(CSV_PATH, ".csv", SearchOption.AllDirectories);
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
			GenerateScript("IDatabase", template);
		}

		/// <summary>
		/// 根据字符串生成脚本文件
		/// </summary>
		/// <param name="scriptName"></param>
		/// <param name="date"></param>
		/// <exception cref="NotImplementedException"></exception>
		private static void GenerateScript(string scriptName, string date)
		{
			string path = GENERATE_SCRIPT_PATH + scriptName + ".cs";
			if (File.Exists(path)) { File.Delete(path); }

			StreamWriter sr = File.CreateText(path);
			sr.WriteLine(date);
			sr.Close();
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
