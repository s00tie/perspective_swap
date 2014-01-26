using UnityEngine;
using System;

class GUIController: MonoBehaviour {

	public Texture2D lifeIndicator;
	public PlayerController playerController;

	string tagName = "";
	int layerId = 0;

	public void OnGUI() {

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

			GUILayout.BeginArea(new Rect(0, Screen.height - 100, 300, 100), "", "box");
			if(GUILayout.Button("A*")) {
				playerController.tileMap.AStar(playerController.currentTileX, playerController.currentTileY,
				                               2, 18,
				                               "",
				                               1);
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label("layer tag");
			tagName = GUILayout.TextField(tagName);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("layer id");
			string t = GUILayout.TextField(layerId.ToString());
			layerId = int.Parse(t);
			GUILayout.EndHorizontal();

			if(GUILayout.Button ("Layer layers")) {
				playerController.tileMap.showLayerInGroupWithTag(tagName, layerId);
			}

			GUILayout.EndArea();
		}

	}

}