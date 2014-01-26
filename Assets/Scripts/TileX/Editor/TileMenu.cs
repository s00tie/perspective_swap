using UnityEditor;
using UnityEngine;

class TileMenu {

	[MenuItem("TileX/Tile Editor", false, 1)]
	static void OpenTileEditor() {
		TileMapEditor.Show();
	}

}
