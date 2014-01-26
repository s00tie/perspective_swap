using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileHolder : MonoBehaviour
{
	private static TileHolder instance = null;
	public static TileHolder Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindGameObjectWithTag("Globals").GetComponent<TileHolder>();
			}
			return instance;
		}
	}

	[SerializeField]
	public List<Tile> dynamicTiles = null;
	TileMap tileMap = null;

	public void SwapInNewTile(int listIndex, int x, int y) {
		tileMap.getTile(x, y, 0).renderer.enabled = false;
		GameObject.Instantiate(dynamicTiles[listIndex], tileMap.startPoint + new Vector3(x * tileMap.xStep, y * tileMap.yStep, 0), Quaternion.identity);
	}

}