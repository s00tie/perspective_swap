using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TileMap: MonoBehaviour {

	public TileLayer[] layers { 
		get {
			return this.gameObject.GetComponentsInChildren<TileLayer>();
		}
	}

	[SerializeField]
	public List<Tileset> tilesets = new List<Tileset>();

	public int sortingOrder = 1;
	public string sortingLayer = "";

	public int mapWidth = 256;
	public int mapHeight = 256;
	public int tileWidth = 32;
	public int tileHeight = 32;
	public int pixelPerUnit = 100;

	public void Awake() {
		foreach(Tileset ts in this.tilesets) {
			ts.CreateTileSprites(this);
		}
	}

	public TileLayer addLayer(LayerType t, string name) {
		GameObject obj = new GameObject();
		obj.transform.parent = this.gameObject.transform;
		obj.name = name;

		TileLayer l = obj.AddComponent<TileLayer>();
		l.parentMap = this;
		l.layerType = t;
		l.name = name;
		l.init(this.width, this.height);

		return l;
	}

	public void RemoveLayer(TileLayer t) {
		DestroyImmediate(t.gameObject);
	}

	public Vector3 startPoint {
		get {
			return this.gameObject.transform.position - new Vector3((float)this.mapWidth / this.pixelPerUnit / 2,
			                                                        (float)this.mapHeight / this.pixelPerUnit / 2);
		}
	}

	public Vector3 endPoint {
		get {
			return this.startPoint + new Vector3(this.worldWidth, this.worldHeight);
		}
	}

	public float xStep {
		get {
			return (float)this.tileWidth / this.pixelPerUnit; 
		}
	}

	public float yStep {
		get {
			return (float)this.tileHeight / this.pixelPerUnit;
		}
	}

	public float worldWidth {
		get {
			return (float)this.mapWidth / this.pixelPerUnit;
		}
	}

	public float worldHeight {
		get {
			return (float)this.mapHeight / this.pixelPerUnit;
		}
	}

	public Rect worldMapRect {
		get {
			Vector3 sp = this.startPoint;
			return new Rect(sp.x, sp.y, this.worldWidth, this.worldHeight);
		}
	}

	public int width {
		get {
			return this.mapWidth / this.tileWidth;
		}
	}

	public int height {
		get {
			return this.mapHeight / this.tileHeight;
		}
	}

	public Tile getTileAt(Vector3 pos, int layerId) {
		TileLayer[] layers = this.layers;
		if(layerId >= 0 && layerId < layers.Length) {
			return layers[layerId].getTileAt(pos - this.startPoint);
		}
		return null;
	}

	public Tile getTile(int x, int y, int layerId) {
		TileLayer[] layers = this.layers;
		if(layerId >= 0 && layerId < layers.Length) {
			return layers[layerId].getTile(x, y);
		}
		return null;
	}

	public void getTileCoordinateAt(Vector3 pos, ref int x, ref int y) {
		Vector3 dis = pos - this.startPoint;
		x = (int)Mathf.Floor((dis.x) / this.xStep);
		y = (int)Mathf.Floor((dis.y) / this.yStep);
	}
	
	public SpriteRenderer previewTile {
		get {
			TilePreview tp = this.gameObject.GetComponentInChildren<TilePreview>();
			if(tp == null) {
				GameObject previewObj = new GameObject();
				previewObj.transform.parent = this.gameObject.transform;
				previewObj.name = "PreviewTile";
				tp = previewObj.AddComponent<TilePreview>();
				SpriteRenderer sr = previewObj.AddComponent<SpriteRenderer>();
				sr.sortingOrder = 20;
			}
			return tp.gameObject.GetComponent<SpriteRenderer>();
		}
	}

	// shows layers with tag = tag in a group, hides all others
	public void showLayerInGroupWithTag(string tag, int groupId) {
		TileLayer[] layers = this.layers;
		foreach(TileLayer layer in layers) {
			if(layer.layerGroup == groupId) {
			    if(layer.layerTag == tag) {
					layer.visible = true;
				} else {
					layer.visible = false;
				}
			}
		}
	}

	int getAStarScore(int sx, int sy, int tx, int ty) {
		return (int)Mathf.Abs (tx - sx) * 10 + (int)Mathf.Abs(ty - sy) * 10;
	}

	void addAStarNode(List<Tile> t, Tile n) {
		int index = t.IndexOf(n);
		if(index != -1) {
			if(n.astarScore < t[index].astarScore) {
				t[index] = n;
			}
			return;
		}

		for(int i=0; i<t.Count; ++i) {
			if(t[i].astarScore <= n.astarScore) {
				t.Insert (i, n);
				return;
			}
		}
		t.Add (n);
	}

	Tile checkAStarNode(List<Tile> c, int x, int y, int tx, int ty, string tag, int groupId) {
		Tile t;
		foreach(TileLayer layer in this.layers) {
			if(layer.layerGroup == groupId) {
				if(layer.layerTag == tag) {
					t = layer.getTile(x, y);
					if(t != null && t.isBlock)
						return null;
				}
			}
		}
		t = this.getTile(x, y, 0);
		if(t != null && t.isBlock)
			return null;

		if(c.Contains(t))
			return null;
		t.astarScore = getAStarScore(x + 1, y, tx, ty) + 1;
		return t;
	}


	bool addAdjacentNodes(List<Tile> t, List<Tile> c, Tile from, int x, int y, int tx, int ty, string tag, int groupId) {
		if(t.Count > 100)
			return true;
		int cx = x, cy = y;
		Tile tile = null;
		if(cx < this.width - 1) {
			tile = this.checkAStarNode(c, cx + 1, cy, tx, ty, tag, groupId);
			if(tile != null) {
				tile.astarFrom = from;
				addAStarNode(t, tile);
			}
			if(cx + 1 == tx && cy == ty)
				return true;
		}
		if(cy > 0) {
			tile = this.checkAStarNode(c, cx - 1, cy, tx, ty, tag, groupId);
			if(tile != null) {
				tile.astarFrom = from;
				addAStarNode(t, tile);
			}
			if(cx - 1 == tx && cy == ty)
				return true;
		}
		if(cy < this.height - 1) {
			tile = this.checkAStarNode(c, cx, cy + 1, tx, ty, tag, groupId);
			if(tile != null) {
				tile.astarFrom = from;
				addAStarNode(t, tile);
			}
			if(cx == tx && cy + 1 == ty)
				return true;
		}
		if(cy > 0) {
			tile = this.checkAStarNode(c, cx, cy - 1, tx, ty, tag, groupId);
			if(tile != null) {
				tile.astarFrom = from;
				addAStarNode(t, tile);
			}
			if(cx == tx && cy - 1 == ty)
				return true;
		}
		return false;
	}

	// AStar against all layers, excluding non-visible layers and all layers in groupId = groupId without tag = tag
	public Tile[] AStar(int startX, int startY, int endX, int endY, string tag, int groupId) {
		List<Tile> currentTiles = new List<Tile>();
		List<Tile> closeNodes = new List<Tile>();

		int cx = startX, cy = startY;
		Tile currentTile = this.getTile (startX, startY, 0);
		while(!addAdjacentNodes(currentTiles, closeNodes, currentTile, cx, cy, endX, endY, tag, groupId)) {
			currentTile = currentTiles.Last();
			currentTiles.RemoveAt (currentTiles.Count - 1);
			if(currentTiles.Count == 0) {
				return new Tile[] { };
			}
			closeNodes.Add (currentTile);

			cx = currentTile.x;
			cy = currentTile.y;
		}
		Tile from = currentTile.astarFrom;
		Debug.Log(startX.ToString() + " " + startY.ToString());
		while(from != null && from.x != startX || from.y != startY) {
			Debug.Log (from);
			from = from.astarFrom;
		}
		Debug.Log (from);
		return new Tile[]{};
	}

}