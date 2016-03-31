using UnityEngine;
using System.Collections;

public interface IPause
{
	void OnPauseGame ();

	void OnResumeGame (PauseGameOptions options);
}