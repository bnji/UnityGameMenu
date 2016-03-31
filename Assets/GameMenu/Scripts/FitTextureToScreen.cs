﻿using UnityEngine;
using System.Collections;

public class FitTextureToScreen : MonoBehaviour
{

//		public int fitToScreenWidth = 1;
//		public int fitToScreenHeight = 1;

		// Use this for initialization
		void Awake ()
		{
				ResizeSpriteToScreen (gameObject, Camera.main, 1, 1);
		}

		void ResizeSpriteToScreen (GameObject theSprite, Camera theCamera, int fitToScreenWidth, int fitToScreenHeight)
		{        
				SpriteRenderer sr = theSprite.GetComponent<SpriteRenderer> ();
		
				theSprite.transform.localScale = new Vector3 (1, 1, 1);
		
				float width = sr.sprite.bounds.size.x;
				float height = sr.sprite.bounds.size.y;
		
				float worldScreenHeight = (float)(theCamera.orthographicSize * 2.0);
				float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);
		
				if (fitToScreenWidth != 0) {
						Vector2 sizeX = new Vector2 (worldScreenWidth / width / fitToScreenWidth, theSprite.transform.localScale.y);
						theSprite.transform.localScale = sizeX;
				}
		
				if (fitToScreenHeight != 0) {
						Vector2 sizeY = new Vector2 (theSprite.transform.localScale.x, worldScreenHeight / height / fitToScreenHeight);
						theSprite.transform.localScale = sizeY;
				}
		}
}
