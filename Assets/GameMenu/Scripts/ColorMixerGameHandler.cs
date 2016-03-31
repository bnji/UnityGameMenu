using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net;
using UnityEngine.SceneManagement;

public class ColorMixerGameHandler : MonoBehaviour
{
	public Text textHighscore;
	public Text textNewHighscore;
	public Text textPausedCurrentScore;
	public Text textScore;
	public Text textTime;
	public Text textTurretsLeft;
	public Color textColor;
	public float timeLeft = 20f;
	public Image imageActivateTurretRefill;
	public Image imageButtonTimeLeftRefill;
	public Button buttonTime;
	public Canvas canvasPause;
	public Canvas canvasIngame;
	public Canvas canvasHighscore;
	public List<Canvas> canvasList;

	private float timeTotal;
	private float lastTimeSetTimeText = 0f;
	private float timeSpent = 0f;
	private int gameScore = 0;
	private bool isGamePaused = false;
	private bool isTimeAttack = false;
	private bool isTurretReady = false;
	private CircularLoader turretCircularLoader;
	private CircularLoader buttonTimeLeftCircularLoader;

	public bool UseSoundFx {
		get;
		set;
	}

	private static ColorMixerGameHandler instance = null;

	public static ColorMixerGameHandler Instance {
		get { return instance; }
	}

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}
		instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	void Start ()
	{
		turretCircularLoader = new CircularLoader (imageActivateTurretRefill);
		buttonTimeLeftCircularLoader = new CircularLoader (imageButtonTimeLeftRefill);
		isTimeAttack = PlayerPrefs.GetInt ("isTimeAttack") == 1;
		timeTotal = timeLeft;
		UseSoundFx = PlayerPrefs.GetInt ("useSoundFX") == 1;
		SetTimeText (true);
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PauseOrResume ();
		}
		if (buttonTimeLeftCircularLoader != null) {
			buttonTimeLeftCircularLoader.Update (timeLeft > 0f, timeTotal - 1f);
		}
		if (Time.time - lastTimeSetTimeText > 1f) {
			SetTimeText ();
		}
		if (turretCircularLoader != null) {
			isTurretReady = turretCircularLoader.Update (!isTurretReady, 3f);
		}
	}

	void SetTimeText (bool resetTime = false)
	{
		if (textHighscore != null) {
			textHighscore.text = "";
		}
		if (textTime != null) {
			if (resetTime) {
				timeSpent = 0f;
			} else {
				timeSpent++;
			}
			if (timeLeft <= 0) {
				var highscore = PlayerPrefs.GetInt ("highscore");
				var isHighscoreSet = false;
				var isNextScoreSet = false;
				for (int i = 0; i < 10; i++) {
					var id = "highscoreRank" + i;
					// 1 = 1000
					// 2 = 500
					// 3 = 0
					var score = PlayerPrefs.GetInt (id); // 700
					//if score is higher than 2 but lower than 1 set 2 to 700
					if (!isHighscoreSet && gameScore >= score) {
						PlayerPrefs.SetInt (id, gameScore);
						isHighscoreSet = true;
					}
					if (isHighscoreSet && !isNextScoreSet) {
						isNextScoreSet = true;
						PlayerPrefs.SetInt ("highscoreRank" + (i + 1), score);
					}
				}
				var isNewHighscore = gameScore > highscore;
				if (isNewHighscore) {
					PlayerPrefs.SetInt ("highscore", gameScore);
				}
				if (textNewHighscore != null) {
					textNewHighscore.gameObject.SetActive (isNewHighscore);
				}
				if (textHighscore != null) {
					textHighscore.text = "Score: " + gameScore;
				}
				gameScore = 0;
				PauseOrResume (canvasHighscore.name);
			}
			if (isTimeAttack) {
				timeLeft--;
				textTime.text = "" + timeLeft;
			} else {
				//				textTime.text = "";
				if (buttonTime != null) {
					buttonTime.gameObject.SetActive (false);
				}
			}
			lastTimeSetTimeText = Time.time;
		}
	}

	public void LoadLevel (string levelName, float waitTime = 0.2f)
	{
		if (isGamePaused) {
			PauseOrResume ();
		}
		StartCoroutine (LoadLevelCO (levelName, waitTime));
	}

	IEnumerator LoadLevelCO (string levelName, float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		SceneManager.LoadScene (levelName);
		yield return new WaitForEndOfFrame ();
	}

	public void RestartGame ()
	{
		LoadLevel (SceneManager.GetActiveScene ().name);
	}

	public void PauseOrResume (string canvasNameToLoad = null)
	{
		isGamePaused = isGamePaused ^ true;
		if (string.IsNullOrEmpty (canvasNameToLoad)) {
			if (textPausedCurrentScore != null) {
				textPausedCurrentScore.text = "Score: " + gameScore;
			}
			LoadCanvas (isGamePaused ? canvasPause : canvasIngame);
		} else {
			LoadCanvas (canvasNameToLoad);
		}
		if (isGamePaused) {
			Time.timeScale = 0.0f;
		} else {
			Time.timeScale = 1.0f;
		}
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			if (isGamePaused) {
				go.SendMessage ("OnPauseGame", new PauseGameOptions () { HideCursor = false, MenuName = "" }, SendMessageOptions.DontRequireReceiver);
			} else {
				go.SendMessage ("OnResumeGame", new PauseGameOptions () { HideCursor = false, MenuName = "" }, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void LoadCanvas (Canvas canvasToActivate)
	{
		if (canvasToActivate != null) {
			LoadCanvas (canvasToActivate.name);
		}
	}

	void LoadCanvas (string canvasToActivateName)
	{
		foreach (var canvas in canvasList) {
			if (canvas.name.Equals (canvasToActivateName)) {
				canvas.gameObject.SetActive (true);
			} else {
				canvas.gameObject.SetActive (false);
			}
		}
	}
}
