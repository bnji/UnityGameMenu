using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColorMixerMenuHandler : MonoBehaviour
{
	public ColorMixerGameHandler cmgh;
	public Image buttonSound;
	public Image buttonFX;
	public Sprite soundOn;
	public Sprite soundOff;
	public Sprite fxOn;
	public Sprite fxOff;

	void Start ()
	{
		ActivateMusic (PlayerPrefs.GetInt ("useBackgroundMusic") == 1);
		if (buttonFX != null) {
			buttonFX.sprite = PlayerPrefs.GetInt ("useSoundFX") == 1 ? fxOn : fxOff;
		}
	}

	public void ToggleMusic ()
	{
		PlayAudio ();
		var isUsingBackgroundMusic = PlayerPrefs.GetInt ("useBackgroundMusic") == 1;
		ActivateMusic (isUsingBackgroundMusic ^ true);
		PlayerPrefs.SetInt ("useBackgroundMusic", isUsingBackgroundMusic ? 0 : 1);
	}

	void ActivateMusic (bool isUsingBackgroundMusic)
	{
		var bgm = GameObject.FindObjectOfType<BackGroundMusic> ();
		if (bgm != null) {
			if (isUsingBackgroundMusic) {
				bgm.Play ();
			} else {
				bgm.Stop ();
			}
		}
		var mbgm = GameObject.FindObjectOfType<MenuBackgroundMusic> ();
		if (mbgm != null) {
			if (isUsingBackgroundMusic) {
				mbgm.Play ();
			} else {
				mbgm.Stop ();
			}
		}
		if (buttonSound != null) {
			buttonSound.sprite = isUsingBackgroundMusic ? soundOn : soundOff;
		}
	}

	public void ToggleFX ()
	{
		PlayAudio ();
		var isUsingFX = PlayerPrefs.GetInt ("useSoundFX") == 1;
		PlayerPrefs.SetInt ("useSoundFX", isUsingFX ? 0 : 1);
		if (buttonFX != null) {
			buttonFX.sprite = isUsingFX ? fxOff : fxOn;
		}
		if (cmgh != null) {
			cmgh.UseSoundFx = !isUsingFX;
		}
		//		Debug.Log ("using fx: " + isUsingFX);
	}

	public void TogglePause ()
	{
		PlayAudio ();
		cmgh.PauseOrResume ();
	}

	public void LoadLevel (string levelName)
	{
		PlayAudio ();
		cmgh.LoadLevel (levelName);
	}

	public void RestartGame ()
	{
		PlayAudio ();
		cmgh.RestartGame ();
	}

	void PlayAudio ()
	{
		var fxSound = GameObject.FindObjectOfType<FxSound> ();
		if (fxSound != null) {
			fxSound.Play ();
		}
	}
}
