using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
	public string sceneToLoad = "";
	public float loadAfter = 1f;
	public bool useLastScene = false;
	public AudioClip audioOnMouseClick;
	public float audioVolume = 1f;

	void OnMouseDown ()
	{
		StartCoroutine (PlayAudio (PlayerPrefs.GetInt ("useSoundFx") == 1));
		var gameState = GameHandler.LoadGameState ();
		if (useLastScene) {
			Debug.Log ("last scene: " + gameState.LastScene);
			Application.LoadLevel (gameState.LastScene);
		} else {
			Application.LoadLevel (sceneToLoad);
		}
	}
	
	IEnumerator PlayAudio (bool isUsingSoundFx)
	{
		if (audioOnMouseClick == null) {
			yield return new WaitForEndOfFrame ();
		} else {
			AudioSource.PlayClipAtPoint (audioOnMouseClick, transform.position, isUsingSoundFx ? audioVolume : 0f);
			yield return new WaitForSeconds (audioOnMouseClick.length);
		}
	}
}
