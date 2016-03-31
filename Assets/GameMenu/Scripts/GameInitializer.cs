using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour
{
	// Use this for initialization
	void Awake ()
	{
		if (PlayerPrefs.GetInt ("hasRunBefore") == 0) {
			ResetGameState ();
		}
	}

	public static void ResetGameState ()
	{
		PlayerPrefs.SetInt ("hasRunBefore", 1);
		PlayerPrefs.SetInt ("useZoom", 1);
		PlayerPrefs.SetInt ("useBackgroundMusic", 1);
		PlayerPrefs.SetInt ("useSoundFX", 1);
		PlayerPrefs.SetFloat ("cameraSizePortraitMode", 2.25f);
		PlayerPrefs.SetFloat ("cameraSizeLandscapeMode", 1.5f);
	}
	
}
