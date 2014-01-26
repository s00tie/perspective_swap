using UnityEngine;
using System;

class GUIController: MonoBehaviour {

	public Texture2D lifeIndicator;
	public PlayerController playerController;

	string tagName = "";
	int layerId = 0;

	public void OnGUI() {


		if(GameStatus.ended) {

		} else {

			if(playerController != null && lifeIndicator != null) {
				
				float w = (lifeIndicator.width) * 3;
				GUI.Box (new Rect(10f, 10f, w, lifeIndicator.height), "");
				
				float x = 10f;
				float y = 10f;
				
				for(int i=0; i<playerController.lifeCount; ++i) {
					GUI.DrawTexture(new Rect(x, y, lifeIndicator.width, lifeIndicator.height),
					                lifeIndicator);
					x += lifeIndicator.width;
				}
				
			}
		}

	}

}