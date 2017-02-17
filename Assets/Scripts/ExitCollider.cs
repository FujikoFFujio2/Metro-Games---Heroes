using UnityEngine;
using System.Collections;

public class ExitCollider : MonoBehaviour {

	private LevelManager levelManager;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
	}

	void OnTriggerEnter (Collider other) {
		print ("Going to the next level");
		levelManager.LoadLevel ("Level2");
	}
}

