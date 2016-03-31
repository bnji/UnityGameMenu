using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour
{
	public bool canRotate = true;
	public float rotationSpeed = 1f;
	public Transform rotateAround = null;

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (canRotate && rotateAround != null && rotationSpeed > 0f)
			transform.RotateAround (rotateAround.position, Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
