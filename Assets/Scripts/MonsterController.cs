using UnityEngine;
using System.Collections;
// TODO: rewrite this class

public class MonsterController : MonoBehaviour {

	public float inputDelay = 0.1f;
	private float forwardVel = 1;
	public float rotateVel = 100;
	private Vector3 direction;

	Quaternion targetRotation;
	Rigidbody rBody;

	void Start() {
		targetRotation = transform.rotation;
		direction = (new Vector3(Random.Range(-1.0f,1.0f), 0.0f,Random.Range(-1.0f,1.0f))).normalized;
		transform.Rotate(direction);
		// GetComponent gets the RigidBody attached to this game object
		if (GetComponent<Rigidbody>())
			rBody = GetComponent<Rigidbody>();
		else
			Debug.LogError ("The character needs a rigidbody.");
	}


	void FixedUpdate() {
		//Run ();
		GetComponent<Animation>().Play("walk");
		Vector3 newPos = transform.position + direction * forwardVel * Time.deltaTime;
		rBody.MovePosition (newPos);
	}

	void OnCollisionEnter (Collision col)
	{
		Debug.Log ("Collision");
		direction = col.contacts[0].normal;
		direction = Quaternion.AngleAxis(Random.Range(-70.0f, 70.0f), Vector3.up) * direction;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}
