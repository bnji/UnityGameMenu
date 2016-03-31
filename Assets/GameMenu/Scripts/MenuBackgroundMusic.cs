using UnityEngine;
using System.Collections;

public class MenuBackgroundMusic : MonoBehaviour
{
	public float maxVolume = 1f;

	//http://codedvelocity.com/development-blog/2013/12/19/continuous-background-music-across-scenes
	public AudioSource audioSource;
	
	private static MenuBackgroundMusic instance = null;
	public static MenuBackgroundMusic Instance {
		get { return instance; }
	}
	
	IEnumerator StartPlaying ()
	{
		yield return new WaitForSeconds (0.25f);
		audioSource.Play ();
		yield return new WaitForEndOfFrame ();
	}

	void DestroyObject ()
	{
		var levelName = Application.loadedLevelName;
		if (levelName.StartsWith ("L")) {
//			Debug.Log ("Destroyed Background Menu Music");
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

//	void Awake ()
//	{
//		DestroyObject ();		
//		if (instance != null && instance != this) {
////			if (instance.audioSource.clip != audioSource.clip) {
////				instance.audioSource.clip = audioSource.clip;
////				instance.audioSource.volume = audioSource.volume;
////				instance.audioSource.Play ();
////			}
//			Destroy (this.gameObject);
//			return;
//		} else {
//			instance = this;
//		}
////		instance = this;
//		SetActive (true);//PlayerPrefs.GetInt ("useBackgroundMusic") == 1);
//		DontDestroyOnLoad (this.gameObject);
//	}
//
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
		SetActive (true);//PlayerPrefs.GetInt ("useBackgroundMusic") == 1);
		DontDestroyOnLoad (this.gameObject);
	}

	void Update ()
	{
		DestroyObject ();
	}
	
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
}
