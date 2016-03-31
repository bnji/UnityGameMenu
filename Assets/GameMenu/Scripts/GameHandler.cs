using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;

public class GameState
{
	public string LastScene {
		get;
		set;
	}

	public float Score {
		get;
		set;
	}
}

public class GameHandler : MonoBehaviour
{
	public static bool UseZoom { get; set; }
	//
	//
	//		public static void SaveGameState ()
	//		{
	//				var player = GameObject.FindObjectOfType<Player> ();
	//				var loadedLevelName = Application.loadedLevelName;
	//				var lastScene = loadedLevelName != "00 Menu" ? loadedLevelName : PlayerPrefs.GetString ("lastScene");
	//				PlayerPrefs.SetString ("lastScene", lastScene);
	//				if (player != null && player.data != null) {
	//						PlayerPrefs.SetFloat ("Score", player.data.Score);
	//				}
	//		}
	
	//		public static void ResetGameState ()
	//		{
	////				PlayerPrefs.SetString ("checkPoint", "");
	////				PlayerPrefs.SetString ("lastScene", "");
	//				PlayerPrefs.SetFloat ("ScoreTemporary", 0f);
	//				//				Debug.Log ("score: " + PlayerPrefs.GetFloat ("score"));
	//		}
	//	
	public static GameState LoadGameState ()
	{
		var gameState = new GameState ();
		gameState.LastScene = PlayerPrefs.GetString ("lastScene");
//				gameState.Score = PlayerPrefs.GetFloat ("Score");
//		
//				var player = GameObject.FindObjectOfType<Player> ();
//				if (player != null && player.data != null) {
//						player.data.Score = gameState.Score;
//				}
		return gameState;
	}
}
