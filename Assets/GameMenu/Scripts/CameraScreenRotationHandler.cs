using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class CameraScreenRotationHandler : MonoBehaviour
{
	public static bool IsReady {
		get;
		private set;
	}

	public static float ScreenRatio { get { return (float)Screen.height / (float)Screen.width; } }
	public static bool IsChangingOrientation { get; private set; }
	public static Orientation lastScreenOrientation = Orientation.NOT_SET;
	public static bool IsPortraitMode { get { return newScreenOrientation == Orientation.PORTRAIT; } }
	private static Orientation newScreenOrientation = Orientation.UNKNOWN;

	public bool canSetOrthSizeOnStart = true;
	public bool canChangeCameraProperties = true;
	public bool isEnabled = true;
	[Description("Only set this property in Mad Level Selector")]
	public Camera
		cam;
	public CameraZoomMobile cameraZoomMobile;
	public float orthographicSizeOnStart = 1.5f;

	private float cameraZoomMobileOrthographicSizeMinOnStart = 1.5f;
	private float cameraZoomMobileOrthographicSizeMaxOnStart = 3.5f;
	private bool isLandscapeModeStart = false;
	private bool canSetLandscapeModeStart = true;
	private float ratio = 1f;

	public float landscapeCameraSizeMin = 2.25f;
	public float landscapeCameraSizeMax = 5.25f;
	public float portraitCameraSizeMin = 1.5f;
	public float portraitCameraSizeMax = 3.5f;

	private bool isParsSetOnStart = false;

	void Awake ()
	{
		IsReady = false;
		if (cam == null) {
			cam = Camera.main;
		}
		IsChangingOrientation = true;
//				orthographicSizeOnStart = Camera.main.orthographicSize;
		if (cameraZoomMobile != null) {
			cameraZoomMobileOrthographicSizeMinOnStart = cameraZoomMobile.orthographicSizeMin;
			cameraZoomMobileOrthographicSizeMaxOnStart = cameraZoomMobile.orthographicSizeMax;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isEnabled)
			return;

		if (lastScreenOrientation == Orientation.NOT_SET) {
			newScreenOrientation = Orientation.PORTRAIT;
		}

		switch (Input.deviceOrientation) {
		case DeviceOrientation.Portrait:
			newScreenOrientation = Orientation.PORTRAIT;
			break;
		case DeviceOrientation.PortraitUpsideDown:
			newScreenOrientation = Orientation.PORTRAIT;
			break;
		case DeviceOrientation.LandscapeLeft:
			newScreenOrientation = Orientation.LANDSCAPE;
			break;
		case DeviceOrientation.LandscapeRight:
			newScreenOrientation = Orientation.LANDSCAPE;
			break;
		case DeviceOrientation.FaceDown:
			if (lastScreenOrientation == Orientation.UNKNOWN || lastScreenOrientation == Orientation.PORTRAIT) {
				newScreenOrientation = Orientation.PORTRAIT;
			} else if (lastScreenOrientation == Orientation.UNKNOWN || lastScreenOrientation == Orientation.LANDSCAPE) {
				newScreenOrientation = Orientation.LANDSCAPE;
			}
			break;
		case DeviceOrientation.FaceUp:
			if (lastScreenOrientation == Orientation.UNKNOWN || lastScreenOrientation == Orientation.PORTRAIT) {
				newScreenOrientation = Orientation.PORTRAIT;
			} else if (lastScreenOrientation == Orientation.UNKNOWN || lastScreenOrientation == Orientation.LANDSCAPE) {
				newScreenOrientation = Orientation.LANDSCAPE;
			}
			break;
		default:
			break;
		}

		IsChangingOrientation = newScreenOrientation != lastScreenOrientation && newScreenOrientation != Orientation.UNKNOWN;

		if (IsChangingOrientation || !isParsSetOnStart) {
			if (canChangeCameraProperties) {
				SetParameters (newScreenOrientation);
			}
			if (canSetLandscapeModeStart) {
				var screenRatioOnStart = (float)Screen.height / (float)Screen.width;
				isLandscapeModeStart = screenRatioOnStart <= 1f;
//								Debug.Log ("Device started in " + (isLandscapeModeStart ? "LANDSCAPE" : "PORTRAIT") + " mode. Screen size: " + Screen.height + "x" + Screen.width + ". Ratio: " + screenRatioOnStart);
//								Debug.Log ("Device is Portrait Mode? " + IsPortraitMode);
				canSetLandscapeModeStart = false;
			}
//						Debug.Log ("Device orientation changed. From: " + lastScreenOrientation + ". To: " + newScreenOrientation);
			lastScreenOrientation = newScreenOrientation;
			isParsSetOnStart = true;
		}
		IsReady = true;
	}

	void SetParameters (Orientation newScreenOrientation)
	{
		var newCamSize = cam.orthographicSize;
		if (newScreenOrientation == Orientation.PORTRAIT) {
			if (isLandscapeModeStart) {
				ratio = (float)Screen.width / (float)Screen.height;
			} else {
				ratio = (float)Screen.height / (float)Screen.width;
			}
			if (cameraZoomMobile != null) {
				cameraZoomMobile.orthographicSizeMin = landscapeCameraSizeMin;// cameraZoomMobileOrthographicSizeMinOnStart * ((float)Screen.height / (float)Screen.width);// ratio;
				cameraZoomMobile.orthographicSizeMax = landscapeCameraSizeMax;// cameraZoomMobileOrthographicSizeMaxOnStart * ((float)Screen.height / (float)Screen.width);// ratio;
//								Debug.Log ("cameraZoomMobile.orthographicSizeMin has been set to: " + cameraZoomMobile.orthographicSizeMin);
			}
			newCamSize = PlayerPrefs.GetFloat ("cameraSizePortraitMode");
			newCamSize = (canSetCustomOrthSize && (newCamSize >= 2.25f && newCamSize <= 5.25f)) ? newCamSize : 2.25f;
		}
		if (newScreenOrientation == Orientation.LANDSCAPE) {
			if (cameraZoomMobile != null) {
				cameraZoomMobile.orthographicSizeMin = portraitCameraSizeMin;//cameraZoomMobileOrthographicSizeMinOnStart;
				cameraZoomMobile.orthographicSizeMax = portraitCameraSizeMax;//cameraZoomMobileOrthographicSizeMaxOnStart;
			}
			newCamSize = PlayerPrefs.GetFloat ("cameraSizeLandscapeMode");
			newCamSize = (canSetCustomOrthSize && (newCamSize >= 1.5f && newCamSize <= 5.25f)) ? newCamSize : 1.5f;
		}
		if (canSetOrthSizeOnStart && !GameHandler.UseZoom) {
			cam.orthographicSize = newCamSize;
		}
	}

	bool canSetCustomOrthSize = true;
}


public enum Orientation
{
	PORTRAIT,
	LANDSCAPE,
	NOT_SET,
	UNKNOWN
}
