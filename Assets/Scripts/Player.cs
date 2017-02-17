using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

	/*private float moveSpeed = 40.0f;

	public Transform target;
	Camera camera;

	// Update is called once per frame
	// FixedUpdate should be used instead of Update when dealing with Rigidbody.
	void FixedUpdate () {
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveX, 0f, moveZ);
		GetComponent<Rigidbody> ().velocity = movement * moveSpeed;
	} */
}
