using UnityEngine;
using System.Collections;

public class BackButtonHandler : MonoBehaviour
{
	ColorMixerMainMenuHandler cmmmh;

	// Use this for initialization
	void Start ()
	{
		cmmmh = GameObject.FindObjectOfType<ColorMixerMainMenuHandler> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time - cmmmh.LastTimePressedEscape > 1f) {
			cmmmh.EscapeTapCount = 0;
		}
		
		if (cmmmh.EscapeTapCount >= 2) {
			Application.Quit ();
		}
		
		if (Input.GetKeyDown (KeyCode.Escape)) {// || Input.GetKeyUp (KeyCode.Escape))) {
			if (cmmmh.CurrentCanvas != null) {
				if (!cmmmh.CurrentCanvas.name.Equals ("Canvas Main Menu")) {
					cmmmh.LoadLastCanvas ();
				} else {
					cmmmh.LastTimePressedEscape = Time.time;
					cmmmh.EscapeTapCount++;
				}
			}
		}
	}
}
