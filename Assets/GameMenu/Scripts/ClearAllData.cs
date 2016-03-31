using UnityEngine;
using System.Collections;

public class ClearAllData : MonoBehaviour
{
	public int tapsBeforeReset = 5;
	public int counter = 0;
	public string textMeshText = "";
	public TextMesh textMesh;

	TextFadeHandler textHandler;

	void Start ()
	{
		textHandler = GameObject.FindObjectOfType<TextFadeHandler> ();
	}

	void OnMouseDown ()
	{
		counter++;
		if (counter < tapsBeforeReset) {
			if (textHandler != null) {
				if (textMesh != null) {
					textMesh.text = "Press " + (tapsBeforeReset - counter) + " more times to clear data";
				}
				textHandler.forceStart = true;
				textHandler.CanStart = true;
			} else {
				Debug.LogWarning ("TextHandler is null!");
			}
		} else {
			if (textHandler != null) {
				if (textMesh != null) {
					textMesh.text = textMeshText;
				}
				textHandler.forceStart = true;
				textHandler.CanStart = true;
			} else {
				Debug.LogWarning ("TextHandler is null!");
			}
			PlayerPrefs.DeleteAll ();
			GameInitializer.ResetGameState ();
			counter = 0;
		}
		//				if (PlayerPrefs.GetInt ("useSoundFx") == 1) {
		//						var fxSound = GameObject.FindObjectOfType<FxSound> ();
		//						if (fxSound != null) {
		//								fxSound.Play ();
		//						}
		//				}
	}
}
