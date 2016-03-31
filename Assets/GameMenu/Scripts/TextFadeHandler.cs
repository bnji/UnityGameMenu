using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextFadeHandler : MonoBehaviour
{
	public bool sendMessage = true;
	public float delayTime = 0f;
	public bool useCustomColor = false;
	public bool canRunMoreThanOnce = true;
	public float speed = 0.008f;
	public List<TextMesh> textMeshes;
	public Transform startPosition;
	public bool useStartPosition = true;

	private float startTime = 0f;
	private TextMesh currentTextMesh;
	private int count = 0;
	public bool CanStart {
		get;
		set;
	}
	private bool isDone = false;
	private bool isFadingIn = true;
	private bool isFadingOut = false;

	public bool forceStart = false;
	public List<Transform> otherPositions;

	public void SetText (string text)
	{
		currentTextMesh.text = text;
	}

	public void AppendText (string text)
	{
		currentTextMesh.text += text;
	}

	void Start ()
	{
		startTime = Time.time;
		Setup ();
	}

	// Use this for initialization
	protected void Setup ()
	{
		CanStart = forceStart;
		//				Debug.Log ("TextFadeHandler");
		if (textMeshes != null && textMeshes.Count > 0) {
			foreach (var tm in textMeshes) {
				Color cOld = tm.color;
				Color cNew = new Color (cOld.r, cOld.g, cOld.b, 0f);
				if (!useCustomColor) {
					var gh = GameObject.FindObjectOfType<ColorMixerGameHandler> ();
					if (gh != null) {
						cNew = gh.textColor;
					}
				}
				tm.color = cNew;
				if (useStartPosition) {
					tm.transform.position = startPosition.position;
				}
			}
			currentTextMesh = textMeshes [count];
		} else {
			isDone = true;
//						Debug.Log ("OnTextHandlerIsDone");
			if (sendMessage) {
				gameObject.SendMessageUpwards ("OnTextHandlerIsDone", SendMessageOptions.DontRequireReceiver);
			}
		}

	}

	public float timeBeforeFadingOut = 1f;
	float fadeBetweenCounter = 0f;
	
	// Update is called once per frame
	void Update ()
	{

		if (Time.time - startTime < delayTime) {
			return;
		}
		
//				Debug.Log ("text fade handler. can start: " + CanStart + ", isdone? " + isDone);

		if (!CanStart)
			return;
		if (isDone)
			return;

		var realSpeed = speed * Time.deltaTime;
				
		if (currentTextMesh.color.a < 1f && isFadingIn) {
			Color cOld = currentTextMesh.color;
			var fadeInSpeed = cOld.a + realSpeed + (float)(1.0 / Mathf.Clamp ((float)currentTextMesh.text.ToCharArray ().Length, 1f, 10f)) * realSpeed;
			//var fadeInSpeed = cOld.a + speed * Time.deltaTime + (float)(1.0 / Mathf.Clamp ((float)currentTextMesh.text.ToCharArray ().Length, 1f, 10f)) * 0.1f;
			Color cNew = new Color (cOld.r, cOld.g, cOld.b, fadeInSpeed);
			currentTextMesh.color = cNew;
		} 
		if (currentTextMesh.color.a >= 1f && isFadingIn) {
			isFadingIn = false;
			isFadingOut = true;
		}
		if (isFadingOut) {
			fadeBetweenCounter += speed * Time.deltaTime;
		}

		if (currentTextMesh.color.a > 0f && isFadingOut && timeBeforeFadingOut < fadeBetweenCounter) {
			Color cOld = currentTextMesh.color;
			var fadeOutSpeed = cOld.a - realSpeed - (float)(1.0 / Mathf.Clamp ((float)currentTextMesh.text.ToCharArray ().Length, 1f, 10f)) * realSpeed;
			//var fadeOutSpeed = cOld.a - speed * Time.deltaTime - (float)(1.0 / Mathf.Clamp ((float)currentTextMesh.text.ToCharArray ().Length, 1f, 10f)) * 0.1f;
			Color cNew = new Color (cOld.r, cOld.g, cOld.b, fadeOutSpeed);
			currentTextMesh.color = cNew;
		} 
		if (currentTextMesh.color.a <= 0f && isFadingOut) {
			isFadingIn = true;
			isFadingOut = false;
			fadeBetweenCounter = 0f;
			count++;
			isDone = count >= textMeshes.Count;
			if (!isDone) {
				currentTextMesh = textMeshes [count];
				if (otherPositions != null && otherPositions.Count > 0) {
					var otherPosition = otherPositions [Random.Range (0, otherPositions.Count)];
					currentTextMesh.transform.position = otherPosition.position;
				}
			}
			if (isDone && count >= textMeshes.Count) {
//								Debug.Log ("OnTextHandlerIsDone 2");
				if (canRunMoreThanOnce) {
					CanStart = false;
					isDone = false;
				}
				if (sendMessage) {
					gameObject.SendMessageUpwards ("OnTextHandlerIsDone", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		
	}
	
	void OnStartTextFade ()
	{
//				Debug.Log ("OnStartTextFade - CanStart = true");
		CanStart = true;
		if (canRunMoreThanOnce) {
			isDone = false;
		}
	}

	void OnCollidedWithTriggerBox ()
	{
		CanStart = true;
		if (canRunMoreThanOnce) {
			isDone = false;
		}
	}
}