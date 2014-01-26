using UnityEngine;
using System;

class GUIController: MonoBehaviour {

	public Texture2D lifeIndicator;
	public PlayerController playerController;

	string tagName = "";
	int layerId = 0;

	public void OnGUI() {


		if(GameStatus.ended) {

			GUILayout.BeginArea(new Rect(Screen.width / 2 - 200,
			                             Screen.height / 2- 100,
			                             400,
			                             200), "", "box");

			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();

			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.fontSize = 40;
			GUILayout.Label("Conguratulations !!!!");

			GUILayout.EndHorizontal();

			
			GUI.skin.label.fontSize = 30;
			GUILayout.Label(" People Saved = " + GameStatus.PeopleSaved.ToString());

			
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Exit", GUILayout.Width(120))) {

			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();


			GUILayout.EndArea();


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