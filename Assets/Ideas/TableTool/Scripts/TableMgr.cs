using FFramework.Ideas.Tool.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
	public class TableMgr : MonoBehaviour
	{
		// Start is called before the first frame update
		void Start()
		{
			DatabaseManager databaseManager = new DatabaseManager();
			databaseManager.Load();

			Debug.Log(databaseManager.GetTable<CodeGenerationTestTableDatabase>().GetDataByKey(0).ID);
			Debug.Log(databaseManager.GetTable<CodeGenerationTestTableDatabase>().GetDataByKey(0).Name[0]);
			Debug.Log(databaseManager.GetTable<CodeGenerationTestTableDatabase>().GetDataByKey(0).Name[1]);
			Debug.Log(databaseManager.GetTable<CodeGenerationTestTableDatabase>().GetDataByKey(0).Des);

			Debug.Log(databaseManager.GetTable<CodeGenerationTestTableDatabase>().GetDataByKey(0));
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
