using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// public reference to our target
	public Transform target;
	public float smoothTime = 1f;
	// How far the camera is from target
	public Vector3 offsetFromChar = new Vector3 (0, 0.5f, 0);


	// How far we want our camera to rotate from the xAxis
	// public float xTilt = 10;
	// Where our camera will move to
	Vector3 destination = Vector3.zero;
	// We need a Character Controller here so that we could access the rotation of the character
	CharacterController charController;
	float rotateVel = 0;

	void Awake() {
		SetCameraTarget (target);
	}

	// This method would set a new target for the camera to look at
	/// <summary>
	/// Sets the camera target (target is inputted from Unity interface)
	/// </summary>
	/// <param name="t">T.</param>
	public void SetCameraTarget(Transform t) {
		target = t;
		// Check if target is supplied to this script
		if (target != null) {
			// Check if the target is of type CharacterController
			if (target.GetComponent<CharacterController> ())
				charController = target.GetComponent<CharacterController> ();
			else
				Debug.LogError ("The camera's target needs a character controller.");
		} else {
			Debug.LogError ("Your camera needs a target");
		}
	}

	void LateUpdate() {
		// moving
		MoveToTarget ();
		// rotating
		GetViewFromTargetFront ();
	}
	/// <summary>
	/// Moves to target. Moves the camera to the character's position
	/// </summary>
	void MoveToTarget() {
		transform.position = target.position;
		transform.position += offsetFromChar;
	}

	void GetViewFromTargetFront() {
		/// Mathf.SmoothDampAngle gradually changes an angle given in degrees towards a desired goal angle over time.
		/// transform.eulerAngles.y: current camera transform y eulerAngles
		/// target.eulerAngles.y: target transform y eulerAngles
		/// rotateVel: The current velocity, this value is modified by Mathf.SmoothDampAngle every time you call it.
		/// That is why we need to pass in rotateVel as a ref, not as a value
		/// Approximately the time it will take to reach the target. A smaller value will reach the target faster.
		float eulerYAngle = 
			Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, smoothTime);
		// Assumption: character will not rotate around x and z axis
		transform.rotation = Quaternion.Euler (0, eulerYAngle, 0);
	}
}
