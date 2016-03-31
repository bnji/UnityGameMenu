using UnityEngine;
using System.Collections;

public class CameraFollowFinger : MonoBehaviour
{
//	public Vector3 positionOnStart;
	public bool isDisabled = false;
	public float screenBoundarySize = 0.1f;
	public Transform leftBoundary;
	public Transform rightBoundary;
	public Transform topBoundary;
	public Transform bottomBoundary;
	public bool useSmoothDamp = true;
	public float zOffset = 7f;
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
	public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.
	
	public Transform objectToFollow;		// Reference to the player's transform.
	float targetZ;
	
	public float smoothTime = 0.3F;
	private Vector3 velocity = Vector3.zero;
	
	public void Enable ()
	{
		this.enabled = true;
	}
	
	void Awake ()
	{
		//		lastSearchedForPlayer = Time.time;
		targetZ = objectToFollow.transform.position.z - zOffset;
		transform.position = new Vector3 (objectToFollow.position.x, objectToFollow.position.y, targetZ);
	}
	
	
	bool CheckXMargin ()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs (transform.position.x - objectToFollow.position.x) > xMargin;
	}
	
	
	bool CheckYMargin ()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs (transform.position.y - objectToFollow.position.y) > yMargin;
	}
	
	void FixedUpdate ()
	{
		if (isDisabled) {
			return;
		}
		
		if (objectToFollow != null) {
			if (Input.touchCount == 1) {// || Input.GetMouseButton (0)) {
//				var pos = Vector2.zero;
//				if (Input.touchCount == 1) {
//					pos = Camera.main.ScreenToViewportPoint (Input.GetTouch (0).position);
//				}
//				if (pos.y >= 0.9f) {
//					return;
//				}
//				TrackPlayer ();
			}
			TrackPlayer ();
		}
	}
	
	
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;
		
		// If the player has moved beyond the x margin...
		if (CheckXMargin ()) {
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp (transform.position.x, objectToFollow.position.x, xSmooth * Time.deltaTime);
		}
		
		// If the player has moved beyond the y margin...
		if (CheckYMargin ()) {
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp (transform.position.y, objectToFollow.position.y, ySmooth * Time.deltaTime);
		}
		
		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp (targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp (targetY, minXAndY.y, maxXAndY.y);
		
		// Set the camera's position to the target position with the same z component.
		//transform.position = new Vector3 (targetX, targetY, targetZ);
		
		//Vector3 targetPosition = player.TransformPoint (new Vector3 (targetX, targetY, targetZ));
		//				targetZ = Mathf.Clamp (targetZ, -2f, 14f);
		
		Vector3 targetPosition = objectToFollow.TransformPoint (new Vector3 (0f, 0f, targetZ));
		if (useSmoothDamp) {
			transform.position = Vector3.SmoothDamp (new Vector3 (transform.position.x, transform.position.y, targetZ), targetPosition, ref velocity, smoothTime);
		} else {
			transform.position = new Vector3 (objectToFollow.transform.position.x, objectToFollow.transform.position.y, targetZ);
		}
		//				Debug.Log (transform.position);
	}
}
