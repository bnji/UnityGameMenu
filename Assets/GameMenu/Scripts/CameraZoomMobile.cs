using UnityEngine;
using System.Collections;

public class CameraZoomMobile : MonoBehaviour
{
	public bool disableCameraFollowOnMultiTouch = true;
	public CameraFollowFinger cameraFollowFinger;
	public float mouseWheelZoomSensitivity = 0.5f;
	public float orthographicSizeMin = 1.5f;
	public float orthographicSizeMax = 5f;
	public bool canUseZoom = false;

	private bool isDisabled = false;
	private float distOnTouch = float.MaxValue;
	private bool hasRegisteredBothFingers = false;

	public void Enable ()
	{
		this.enabled = true;
		canUseZoom = true;
	}

	public Vector2 GetTouchPosition (Touch touch)
	{
		var targetPos = Camera.main.ScreenToWorldPoint (touch.position);
		return new Vector2 (targetPos.x, targetPos.y);
	}

	public void OnPauseGame (PauseGameOptions options)
	{
		isDisabled = true;
	}

	public void OnResumeGame (PauseGameOptions options)
	{
		isDisabled = false;
	}

	void Update ()
	{
		if (isDisabled)
			return;

		if (!canUseZoom)
			return;

		if (Input.touchCount == 0) {
//			Debug.Log (Input.mouseScrollDelta + " - " + Input.mouseScrollDelta.sqrMagnitude);
			Camera.main.orthographicSize += Input.mouseScrollDelta.y * mouseWheelZoomSensitivity;
			Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
			if (disableCameraFollowOnMultiTouch) {
				cameraFollowFinger.isDisabled = false;
			}
		}

		if (Input.touchCount < 2) {
			distOnTouch = float.MaxValue;
			if (hasRegisteredBothFingers) {
//										player.controller.canControlPlayer = true;
//										player.ShowHUD ();
				hasRegisteredBothFingers = false;
				var newCameraSize = Camera.main.orthographicSize;
//										if (Input.deviceOrientation == DeviceOrientation.FaceDown || Input.deviceOrientation == DeviceOrientation.FaceUp || Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
//												PlayerPrefs.SetFloat ("cameraSizeLandscapeMode", newCameraSize);
//										} else if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
//												PlayerPrefs.SetFloat ("cameraSizePortraitMode", newCameraSize * 1.5f);
//										}

				if (CameraScreenRotationHandler.IsPortraitMode) {
					PlayerPrefs.SetFloat ("cameraSizePortraitMode", newCameraSize);
//												Debug.Log ("saved camera size: " + newCameraSize + " in PORTRAIT mode");
				} else {
					PlayerPrefs.SetFloat ("cameraSizeLandscapeMode", newCameraSize);
//												Debug.Log ("saved camera size: " + newCameraSize + " in LANDSCAPE mode");
				}
			}
		}
		if (Input.touchCount == 2) {
			var t1 = Input.GetTouch (0);
			var t2 = Input.GetTouch (1);
			var t1Pos = GetTouchPosition (t1);
			var t2Pos = GetTouchPosition (t2);
			var t1ViewPortPoint = Camera.main.WorldToViewportPoint (t1Pos);
			var t2ViewPortPoint = Camera.main.WorldToViewportPoint (t2Pos);
			var dist = Vector2.Distance (t1ViewPortPoint, t2ViewPortPoint);
			if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began) {
				if (distOnTouch == float.MaxValue) {
					distOnTouch = dist;
				}
				if (disableCameraFollowOnMultiTouch) {
					cameraFollowFinger.isDisabled = true;
				}
			}
			if (t1.phase == TouchPhase.Stationary || t2.phase == TouchPhase.Stationary) {
				distOnTouch = dist;
			}
			if (t1.phase == TouchPhase.Moved && t2.phase == TouchPhase.Moved) {
//								Debug.Log ("dist: " + dist + ", distOnTouch: " + distOnTouch);
				// zoom out
				if (dist < distOnTouch) {
					Camera.main.orthographicSize += dist * mouseWheelZoomSensitivity * (Camera.main.orthographicSize / 5f);
				}
				// zoom in
				if (dist > distOnTouch) {
					Camera.main.orthographicSize -= dist * mouseWheelZoomSensitivity * (Camera.main.orthographicSize / 5f);
				}
				Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
			}
		}
//				if (Input.GetAxis ("Zoom") > 0) { // back (zoom in)
//						Camera.main.orthographicSize += Input.GetAxis ("Zoom") * mouseWheelZoomSensitivity;
//			
//				}
//				if (Input.GetAxis ("Zoom") < 0) { // forward (zoom in)
//						Camera.main.orthographicSize += Input.GetAxis ("Zoom") * mouseWheelZoomSensitivity;
//				}
//				Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
	}
}
