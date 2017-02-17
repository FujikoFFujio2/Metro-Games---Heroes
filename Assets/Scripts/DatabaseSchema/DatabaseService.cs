using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService  {

	private SQLiteConnection _connection;

	public DataService(string DatabaseName){
		var dbPath = string.Format(@"Assets/Database/{0}", DatabaseName);

		_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		Debug.Log("Final PATH: " + dbPath);
	}

	public IEnumerable<MazeEntity> GetMazes(){
		return _connection.Table<MazeEntity>();
	}
}