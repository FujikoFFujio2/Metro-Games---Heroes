using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DatabaseAccess : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print("DatabaseAccess");
		var ds = new DataService ("gameDatabase.db");
		var mazes = ds.GetMazes ();
		print (mazes);
		foreach (var maze in mazes) {
			print(maze.ToString());
		}
	}

}