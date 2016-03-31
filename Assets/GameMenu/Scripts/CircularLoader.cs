using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircularLoader
{
	float degreesTotal = 360f;
	Image circularImage;

	public CircularLoader (Image image)
	{
		circularImage = image;
	}

	public bool Update (bool canUpdate, float interval)
	{
		bool isDone = true;
		if (canUpdate) {
			var degreesPerSecond = (360f / interval) * Time.deltaTime;
			if (degreesTotal > 0f) {
				degreesTotal -= degreesPerSecond;
			}
			degreesTotal = degreesTotal >= 0f ? degreesTotal : 0f;
			if (circularImage != null) {
				var amount = degreesTotal * (1f / 360f);
				circularImage.fillAmount = amount;
				isDone = amount <= 0f;
			}
		} else {
			degreesTotal = 360f;
		}
		return isDone;
	}
}