using UnityEngine;
using System.Collections;

public class FxSound : MonoBehaviour
{
	private AudioSource audioSource;

	private static FxSound instance = null;
	public static FxSound Instance {
		get { return instance; }
	}
	
	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}
		DontDestroyOnLoad (this.gameObject);
		audioSource = GetComponent<AudioSource> ();
	}

	public void Play ()
	{
		if (audioSource != null) {
			if (PlayerPrefs.GetInt ("useSoundFX") == 1) {
				audioSource.Play ();
			}
		}
	}
}
