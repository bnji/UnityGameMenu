using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColorMixerMainMenuHandler : MonoBehaviour
{
	public Text textHighscoreList;
	public Text textConfirmRemoveAllData;
	public List<Canvas> canvasList;
	public List<Text> tutorialTexts;
	public RawImage tutorialImage;
	public Image buttonSound;
	public Image buttonFX;
	public Sprite soundOn;
	public Sprite soundOff;
	public Sprite fxOn;
	public Sprite fxOff;

	private bool canHideTextConfirmRemoveAllData = false;
	private Color textConfirmRemoveAllDataColorBefore;
	private float textConfirmRemoveAllDataAlpha = 1f;
	private float lastTimePressedRemoveAllData = 0f;
	private int tutorialIndex = 0;
	private int tapCountBeforeRemovingData = 5;
	private int removeAllDataTapCount = 0;

	public float LastTimePressedEscape { get; set; }

	public int EscapeTapCount { get; set; }

	public Canvas CurrentCanvas { get; private set; }

	public Canvas lastCanvas = null;

	void Start ()
	{
		ActivateMusic (PlayerPrefs.GetInt ("useBackgroundMusic") == 1);
		if (buttonFX != null) {
			buttonFX.sprite = PlayerPrefs.GetInt ("useSoundFX") == 1 ? fxOn : fxOff;
		}
		textConfirmRemoveAllDataColorBefore = textConfirmRemoveAllData.color;
		LastTimePressedEscape = 0f;
		EscapeTapCount = 0;
		CurrentCanvas = lastCanvas;
	}

	void Update ()
	{
		if (canHideTextConfirmRemoveAllData && textConfirmRemoveAllData != null) {
			textConfirmRemoveAllDataAlpha -= Time.time * 0.01f;
			textConfirmRemoveAllData.color = new Color (textConfirmRemoveAllData.color.r, textConfirmRemoveAllData.color.g, textConfirmRemoveAllData.color.b, textConfirmRemoveAllDataAlpha);
			canHideTextConfirmRemoveAllData = textConfirmRemoveAllData.color.a > 0f;
		}
		if (removeAllDataTapCount > 0 && Time.time - lastTimePressedRemoveAllData > 2f) {
			removeAllDataTapCount = 0;
			if (textConfirmRemoveAllData != null) {
				textConfirmRemoveAllData.text = "";
				textConfirmRemoveAllData.color = textConfirmRemoveAllDataColorBefore;
				canHideTextConfirmRemoveAllData = true;
			}
		}
	}

	public void RemoveAllData ()
	{
		lastTimePressedRemoveAllData = Time.time;
		removeAllDataTapCount++;
		if (textConfirmRemoveAllData != null) {
			var tapCountLeft = (tapCountBeforeRemovingData - removeAllDataTapCount);
			var tapCountTimesText = tapCountLeft > 1 ? "times" : "time";
			if (tapCountLeft > 0) {
				//				canHideTextConfirmRemoveAllData = true;
				textConfirmRemoveAllData.color = new Color (1f, 1f, 1f, 1f);
				textConfirmRemoveAllData.text = "Tap " + tapCountLeft + " more " + tapCountTimesText + " to remove all data.";
			} else {
				textConfirmRemoveAllData.text = "";
			}
		}
		if (removeAllDataTapCount >= tapCountBeforeRemovingData) {
			PlayerPrefs.DeleteAll ();
			GameInitializer.ResetGameState ();
//			var cmgh = GameObject.FindObjectOfType<ColorMixerGameHandler> ();
//			if (cmgh != null) {
//				cmgh.ToggleMusic (PlayerPrefs.GetInt ("useBackgroundMusic") == 1);
//			}
			ActivateMusic (PlayerPrefs.GetInt ("useBackgroundMusic") == 1);
			if (buttonFX != null) {
				buttonFX.sprite = (PlayerPrefs.GetInt ("useSoundFX") == 1) ? fxOn : fxOff;
			}
			textConfirmRemoveAllData.text = "All game data has been removed!";
			textConfirmRemoveAllDataAlpha = 1f;
			removeAllDataTapCount = 0;
			canHideTextConfirmRemoveAllData = true;
		}
	}

	public void LoadLastCanvas ()
	{
		LoadCanvas (lastCanvas);
	}

	public void OpenURL (string url)
	{
		Application.OpenURL (url);
	}

	public void LoadTutorial (Canvas canvas)
	{
		TutorialGoTo (0);
		LoadCanvas (canvas);
	}

	public void TutorialGoTo (int index)
	{
		if (index >= 0 && index < tutorialTexts.Count) {
			tutorialIndex = index;
		}
		ActivateTutorialText ();
	}

	public void TutorialNext ()
	{
		if (tutorialIndex < tutorialTexts.Count - 1) {
			tutorialIndex++;
		} else {
			tutorialIndex = 0;
			//			LoadCanvas ("Canvas Main Menu");
		}
		TutorialGoTo (tutorialIndex);
	}

	public void TutorialPrevious ()
	{
		if (tutorialIndex > 0) {
			tutorialIndex--;
		} else {
			tutorialIndex = tutorialTexts.Count - 1;
			//			LoadCanvas ("Canvas Main Menu");
		}
		TutorialGoTo (tutorialIndex);
	}

	void ActivateTutorialText ()
	{
		for (int i = 0; i < tutorialTexts.Count; i++) {
			tutorialTexts [i].gameObject.SetActive (i == tutorialIndex);
		}
	}

	public void LoadHighscores (Canvas canvas)
	{
		textHighscoreList.text = "";
		for (int i = 0; i < 10; i++) {
			var score = PlayerPrefs.GetInt ("highscoreRank" + i);
			textHighscoreList.text += "" + (i + 1) + ": " + score + "\r\n";
		}
		LoadCanvas (canvas);
	}

	public void LoadCanvas (Canvas canvasToActivate)
	{
		LoadCanvas (canvasToActivate.name);
	}

	public void LoadCanvas (string canvasToActivateName)
	{
		PlayAudio ();
		foreach (var canvas in canvasList) {
			if (canvas.name.Equals (canvasToActivateName)) {
				lastCanvas = CurrentCanvas;
				CurrentCanvas = canvas;
				//				Debug.Log ("Last canvas: " + lastCanvas + ", current canvas: " + CurrentCanvas);
				canvas.gameObject.SetActive (true);
			} else {
				canvas.gameObject.SetActive (false);
			}
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
		//		Debug.Log ("using sound: " + isUsingBackgroundMusic);
	}

	public void ToggleFX ()
	{
		PlayAudio ();
		var isUsingFX = PlayerPrefs.GetInt ("useSoundFX") == 1;
		PlayerPrefs.SetInt ("useSoundFX", isUsingFX ? 0 : 1);
		if (buttonFX != null) {
			buttonFX.sprite = isUsingFX ? fxOff : fxOn;
		}
		//		Debug.Log ("using fx: " + isUsingFX);
	}

	public void RestorePurchases ()
	{
		PlayAudio ();
//		SoomlaStore.RestoreTransactions ();
	}

	public void BuyItem (string storeItemId)
	{
		PlayAudio ();
//		PurchaseHandler.BuyItem (storeItemId);
	}

	public void StartFreeModeGame ()
	{
		PlayAudio ();
		PlayerPrefs.SetInt ("isTimeAttack", 0);
		LoadLevel ("Level1", 0.2f);
	}

	public void StartTimeAttackGame ()
	{
		PlayAudio ();
		PlayerPrefs.SetInt ("isTimeAttack", 1);
		LoadLevel ("Level1", 0.2f);
	}

	public void Credits ()
	{
		PlayAudio ();
		LoadLevel ("MenuCredits", 0.2f);
	}

	public void Tutorial ()
	{
		PlayAudio ();
		LoadLevel ("MenuTutorial", 0.2f);
	}

	public void BackToMainMenu ()
	{
		PlayAudio ();
		LoadLevel ("MainMenu", 0.2f);
	}

	void PlayAudio ()
	{
		var fxSound = GameObject.FindObjectOfType<FxSound> ();
		if (fxSound != null) {
			fxSound.Play ();
		}
	}

	void LoadLevel (string levelName, float waitTime = 0.2f)
	{
		StartCoroutine (LoadLevelCO (levelName, waitTime));
	}

	IEnumerator LoadLevelCO (string levelName, float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		Application.LoadLevel (levelName);
		yield return new WaitForEndOfFrame ();
	}
}