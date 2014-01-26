using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(TileMap))]
class TileMapInspector: Editor {

	public int selectedLayerIndex;

	public override void OnInspectorGUI() {

		TileMap tm = this.target as TileMap;
		EditorGUILayout.BeginVertical();

		EditorGUI.BeginChangeCheck();

		tm.mapWidth = EditorGUILayout.IntField("Map Width", tm.mapWidth);
		tm.mapHeight = EditorGUILayout.IntField("Map Height", tm.mapHeight);
		tm.tileWidth = EditorGUILayout.IntField("Tile Width", tm.tileWidth);
		tm.tileHeight = EditorGUILayout.IntField("Tile Height", tm.tileHeight);
		tm.pixelPerUnit = EditorGUILayout.IntField("Pixel-Per-Unit", tm.pixelPerUnit);

		if(EditorGUI.EndChangeCheck()) {
			SceneView.RepaintAll();
		}

		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Tile Layer")) {
			Undo.RegisterCreatedObjectUndo(tm.addLayer(LayerType.TileLayer, "Layer " + tm.layers.Length.ToString()).gameObject, "Layer");
			this.Repaint();
		}
		if(GUILayout.Button("Add Object Layer" )) {
			Undo.RegisterCreatedObjectUndo(tm.addLayer(LayerType.ObjectLayer, "Layer " + tm.layers.Length.ToString()).gameObject, "Tile");
			this.Repaint();
		}
		GUILayout.EndHorizontal();

		TileLayer layerToRemove = null;
		selectedLayerIndex = TileGUIUtility.MakeSimpleList(
			selectedLayerIndex, 
			TileGUIUtility.GetStringArray(tm.layers), 
			TextAnchor.MiddleLeft,
			() => {
				


			},
			(index, is_selection) => {
				TileLayer layer = tm.layers[index];

				GUILayout.BeginHorizontal();
			    layer.visible = GUILayout.Toggle(layer.visible, "", GUILayout.Width(12));
				bool selection = GUILayout.Button(layer.name + " (" + layer.layerType.ToString() + ")",
			                                  	  is_selection ? TileGUIUtility.listEntryFocused : TileGUIUtility.listEntryNormal);
				GUILayout.EndHorizontal();
				layer.transparency = EditorGUILayout.Slider("Transparency", layer.transparency, 0f, 1f);

				layer.sortingLayer = EditorGUILayout.Popup("Sorting Layer", layer.sortingLayer, TileGUIUtility.GetSortingLayerNames());
				layer.sortingOrder = EditorGUILayout.IntField("Sorting Order", layer.sortingOrder);

				EditorGUILayout.Space();
				layer.layerGroup = EditorGUILayout.IntField("Group Id", layer.layerGroup);
				layer.layerTag = EditorGUILayout.TextField("Layer Tag", layer.layerTag);

			
				TileMapEditor editor = TileMapEditor.Get ();
				Tileset ts = tm.tilesets[editor.selectedTileSetIndex];
				TileInfo ti = ts.getTileInfo(editor.selectedTileX, editor.selectedTileY);
				if(ti != null) {
					if(GUILayout.Button("Fill with Current Tile", GUILayout.Width(200))) {
						int rows = tm.mapHeight / tm.tileHeight;
						int columns = tm.mapWidth / tm.tileWidth;
						for(int i=0; i<rows; ++i) {
							for(int j=0; j<columns; ++j) {
								layer.addTile(i, j, ti);
							}
						}
					}
				} else {
					GUILayout.Button("Fill with Current Tile", "button off", GUILayout.Width(200));
				}
				
				GUILayout.BeginHorizontal();

				GUILayout.FlexibleSpace();
				if(GUILayout.Button("Reset", GUILayout.Width(64))) {
					layer.ResetLayer();	
				}
				
				if(GUILayout.Button("Remove", GUILayout.Width(64))) {
					layerToRemove = layer;
				}
				
				GUILayout.EndHorizontal();
				Rect r = GUILayoutUtility.GetLastRect();
				EditorGUILayout.Space();
				GUI.Box (new Rect((Screen.width - r.width) / 2 + 1, r.y + r.height + 8f, r.width + 5f, 1), "");

				return selection;
			}

		);
		if(layerToRemove != null) {
			tm.RemoveLayer(layerToRemove);
		}

		EditorGUILayout.EndVertical();

	}

	public void OnSceneGUI() {
		TileMap tm = this.target as TileMap;

		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

		Vector3 pos = tm.startPoint;
		int rows = tm.mapHeight / tm.tileHeight;
		int columns = tm.mapWidth / tm.tileWidth;

		for(int i=0; i<=columns; ++i) {
			Handles.DrawLine(new Vector3((float)i * tm.xStep, 0) + pos,
			                 new Vector3((float)i * tm.xStep, (float)tm.worldHeight) + pos);
		}
		for(int i=0; i<=rows; ++i) {
			Handles.DrawLine(new Vector3(0, (float)i * tm.yStep) + pos,
			                 new Vector3((float)tm.worldWidth, (float)i * tm.yStep) + pos);
		}


		Event evt = Event.current;
		Camera sceneCam = SceneView.currentDrawingSceneView.camera;
		Vector2 mousePos = Event.current.mousePosition;
		mousePos.y = sceneCam.pixelHeight - mousePos.y;
		Vector3 p = sceneCam.ScreenPointToRay(mousePos).origin;
		Vector3 off = (p - pos) * tm.pixelPerUnit ;
		int x = (int)Mathf.Floor(off.x / tm.tileWidth);
		int y = (int)Mathf.Floor(off.y / tm.tileHeight);

		TileMapEditor editor = TileMapEditor.Get ();
		SpriteRenderer previewRenderer = tm.previewTile;
		if(x >= 0 && x < columns &&
		   y >= 0 && y < rows) {
			if(editor.selectedTileSetIndex != -1) {
				Tileset ts = tm.tilesets[editor.selectedTileSetIndex];
				TileInfo ti = ts.getTileInfo(editor.selectedTileX, editor.selectedTileY);
				previewRenderer.sprite = ti.sprite;
				previewRenderer.color = new Color(255, 255, 255, 0.5f);
				previewRenderer.gameObject.transform.rotation = Quaternion.AngleAxis(ti.direction, Vector3.forward);
				previewRenderer.gameObject.transform.position = new Vector3(pos.x + x * tm.xStep + tm.xStep / 2, 
				                                                            pos.y + y * tm.yStep + tm.yStep / 2, 
				                                                            tm.gameObject.transform.position.z);

				if(editor.isErase)
					TileGUIUtility.DrawSceneBezierRect(new Rect(pos.x + x * tm.xStep, pos.y + y * tm.yStep, tm.xStep, tm.yStep), 6, Color.red);
				else
					TileGUIUtility.DrawSceneBezierRect(new Rect(pos.x + x * tm.xStep, pos.y + y * tm.yStep, tm.xStep, tm.yStep), 6, Color.green);

				if(evt.type == EventType.MouseDown) {
					if(evt.button == 0) {
						TileLayer layer;
						if(tm.layers.Length == 0)
							layer = tm.addLayer(LayerType.TileLayer, "Layer 0");
						else
							layer = tm.layers[selectedLayerIndex];
						if(layer != null) {
							if(editor.isErase) {
								layer.removeTile(x, y);
							} else {
								Tile t = layer.addTile(x, y, ti);
								Undo.RegisterCreatedObjectUndo(t.gameObject, "Tile");
							}
						}
					} else {
						ti.direction = (ti.direction + 90) % 360;
					}

				}
			} 

			if(evt.type == EventType.mouseDrag)
				evt.Use();
		}else {
			previewRenderer.sprite = null;
		}



	}

}
