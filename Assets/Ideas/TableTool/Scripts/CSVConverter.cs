using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FFramework.Ideas
{
	/// <summary>
	/// CSV解析器
	/// 格式定义
	/// 每一行用换行符分隔区分
	/// 基本元素用逗号‘,’分隔区分
	/// 数组内容用分号‘;’分隔区分
	/// 表头元数据用变量类型与变量名用‘.’分隔区分
	/// </summary>
	public class CSVConverter
	{
		/// <summary>
		/// 序列化CSV文件中的Meta信息
		/// </summary>
		/// <param name="csvAsset"></param>
		/// <returns></returns>
		public static string[] SerializeCSVMetaString(TextAsset csvAsset)
		{
			string[] csvLines = csvAsset.text.Replace("\n", string.Empty).Split("\r");
			return csvLines[0].Split(',');
		}

		/// <summary>
		/// 序列化CSV文件中的Data信息
		/// </summary>
		/// <param name="csvData"></param>
		/// <returns></returns>
		public static string[][] SerializeCSVDataString(TextAsset csvAsset)
		{
			string[] csvLines = csvAsset.text.Replace("\n", string.Empty).Split("\r");
			string[][] result = new string[csvLines.Length - 1][];

			for (int i = 1; i < csvLines.Length; i++)//跳过表头
			{
				result[i - 1] = csvLines[i].Split(",");
			}

			return result;
		}

		/// <summary>
		/// 将使用“;”分隔的字符串转换为响应的数组
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T[] ConvertToArray<T>(string value)
		{
			string[] strings = value.Split(';');

			T[] result = new T[strings.Length];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = (T)Convert.ChangeType(strings[i], typeof(T));
			}
			return result;
		}

		public static string GetArrayString<T>(T[] array)
		{
			if (array == null)
			{
				return "Array is null.";
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("[");
			for (int i = 0; i < array.Length; i++)
			{
				sb.Append(array[i]);
				if (i < array.Length - 1)
				{
					sb.Append(", ");
				}
			}
			sb.Append("]");
			return sb.ToString();
		}
	}
}
