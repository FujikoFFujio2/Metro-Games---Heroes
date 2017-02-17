using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float inputDelay = 0.1f;
	public float forwardVel = 1;
	public float rotateVel = 100;

	Quaternion targetRotation;
	Rigidbody rBody;
	// TODO: what are the variables below are for?
	float forwardInput, turnInput;

	/*public Quaternion TargetRotation {
		get { return targetRotation;}
	}*/

	void Start() {
		targetRotation = transform.rotation;
		// GetComponent gets the RigidBody attached to this game object
		if (GetComponent<Rigidbody>())
			rBody = GetComponent<Rigidbody>();
		else
			Debug.LogError ("The character needs a rigidbody.");

		forwardInput = turnInput = 0;
	}

	void onCollisionEnter() {
		
	}

	void GetInput() {
		forwardInput = Input.GetAxis ("Vertical");
		turnInput = Input.GetAxis ("Horizontal");
	}

	void Update() {
		GetInput ();
		Turn ();
	}

	void FixedUpdate() {
		Run ();
	}

	void Run() {
		if (Mathf.Abs(forwardInput) > inputDelay) {
			// move
			GetComponent<Animation>().Play("walk");
			rBody.velocity = transform.forward * forwardInput * forwardVel / 10;
		} else {
			// zero velocity
			// velocity here is a Vector3 object because the rigidBody moving in 3 dimensional space
			rBody.velocity = Vector3.zero;
			GetComponent<Animation>().Play("idle");
		}
	}

	void Turn() {
		// TODO: why we need to use Vector3.up (y axis) here?
		targetRotation *= Quaternion.AngleAxis (rotateVel * turnInput * Time.deltaTime, Vector3.up);
		transform.rotation = targetRotation;
	}
}
