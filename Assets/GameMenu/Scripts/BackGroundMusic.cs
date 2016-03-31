using UnityEngine;
using System.Collections;

public class BackGroundMusic : MonoBehaviour
{
	public float maxVolume = 1f;

	//http://codedvelocity.com/development-blog/2013/12/19/continuous-background-music-across-scenes
	public AudioSource audioSource;

	private static BackGroundMusic instance = null;
	public static BackGroundMusic Instance {
		get { return instance; }
	}

	IEnumerator StartPlaying ()
	{
		yield return new WaitForSeconds (0.25f);
		audioSource.Play ();
		yield return new WaitForEndOfFrame ();
	}

	void Awake ()
	{
		DestroyObject ();
		if (instance != null && instance != this) {
			if (instance.audioSource.clip != audioSource.clip) {
				instance.audioSource.clip = audioSource.clip;
				instance.audioSource.volume = audioSource.volume;
				instance.audioSource.Play ();
			}
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
		instance = this;
		SetActive (PlayerPrefs.GetInt ("useBackgroundMusic") == 1);
		DontDestroyOnLoad (this.gameObject);
	}
	
	void Start ()
	{
		//				if (Application.loadedLevelName.Equals ("00 FacebookLogin")) {
		//						SetActive (PlayerPrefs.GetInt ("useBackgroundMusic") == 1);
		//				}
	}
	
	void DestroyObject ()
	{
		var levelName = Application.loadedLevelName;
		if (levelName.StartsWith ("M")) {
			StartCoroutine (DestroySlowly ());
		}
	}
	
	IEnumerator DestroySlowly ()
	{
		Stop (false);
		while (audioSource.volume > 0f) {
			yield return null;
		}
		Destroy (this.gameObject);
	}
	
	void Update ()
	{
		DestroyObject ();
//		if (Application.loadedLevelName.StartsWith ("00 World")) {
//			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -0.898659f);
//		}
	}

//		public void SetActive (bool value)
//		{
////				audioSource.enabled = value;
////				if (value)
////						Debug.Log ("volume is ON");
////				else
////						Debug.Log ("volume is OFF");
//				StartCoroutine (SetVolume (value));
//		}
//
//		IEnumerator SetVolume (bool value)
//		{
//				if (value) {
//						while (audioSource.volume < 1f) {
//								audioSource.volume += 0.01f * Time.fixedTime;
////								Debug.Log (audioSource.volume);
//								yield return null;
//						}
//				} else {
//						audioSource.volume = 0f;
//				}
//				yield return new WaitForEndOfFrame ();
//		}

	Coroutine lastCoroutineSetVolume = null;

	public void Stop (bool forceAudioStop = true)
	{
		SetActive (false, forceAudioStop);
	}
	
	public void Play (bool forceAudioStop = true)
	{
		SetActive (true, forceAudioStop);
	}
	
	public void SetActive (bool value, bool forceAudioStop = true)
	{			
		if (lastCoroutineSetVolume != null) {
			StopCoroutine (lastCoroutineSetVolume);
		}
		lastCoroutineSetVolume = StartCoroutine (SetVolume (value, forceAudioStop));
	}
	
	IEnumerator SetVolume (bool value, bool forceAudioStop = true)
	{
		if (value) {
			if (!audioSource.isPlaying) {
				audioSource.volume = 0f;
				audioSource.Play ();
			}
			while (audioSource.volume < maxVolume) {
				audioSource.volume += Time.deltaTime;
//								Debug.Log ("increasing volume: " + audioSource.volume);
				yield return null;
			}
		} else {
			if (audioSource.isPlaying && audioSource.volume > 0f) {
				if (forceAudioStop) {
					audioSource.volume = 0f;
				} else {
					while (audioSource.volume > 0f) {
						audioSource.volume -= Time.deltaTime;
//												Debug.Log ("decreasing volume: " + audioSource.volume);
						yield return null;
					}
				}
			}
		}
		yield return new WaitForEndOfFrame ();
	}

//		static bool AudioBegin = false; 
//
//		void Awake ()
//		{
//				if (canPlay) {//Application.loadedLevelName != "01 Level Select") {
//						if (!AudioBegin) {
//								audioSource.Play ();
//								DontDestroyOnLoad (gameObject.GetComponentInParent<Camera> ().gameObject);
//								AudioBegin = true;
//						}
//				}
//				canPlay = true;
//		}
//	
//		void Update ()
//		{
//				if (Application.loadedLevelName == "01 Level Select") {
//						audioSource.Stop ();
//						AudioBegin = false;
//						Destroy (gameObject.GetComponentInParent<Camera> ().gameObject);
//				}
//		}
}
