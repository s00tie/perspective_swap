using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerCharacter = null;
	private CharacterInfo characterInfo = null;
	public TileMap tileMap;
	private float tileDestEpsilon = 0.00005f;

	public int lifeCount = 3;

	public int currentTileX, currentTileY;
	public int targetTileX, targetTileY;

	void Start () {
		characterInfo = playerCharacter.GetComponent<CharacterInfo>();
		if (characterInfo ==  null) {
			enabled = false;
		}
		if(tileMap != null)
			tileMap.getTileCoordinateAt(playerCharacter.transform.position, ref currentTileX, ref currentTileY);
		targetTileX = currentTileX;
		targetTileY = currentTileY;
	}

	void Update () {
		CheckInput();
	}

	bool isClose(Vector3 p1, Vector3 p2) {
		return Mathf.Abs (p2.x - p1.x) < 0.00005f &&
			Mathf.Abs (p2.y - p1.y) < 0.00005f;
	}
	
	private void CheckInput() {
		if(tileMap != null)  {
			int tx = targetTileX, ty = targetTileY;
			bool closeToDest = isClose (this.playerCharacter.transform.position, tileMap.getTile(targetTileX, targetTileY, 0).gameObject.transform.position);
		
			if(targetTileY == currentTileY && closeToDest) {
				if(Input.GetAxis("Horizontal") < 0) {
					tx = (int)Mathf.Clamp(currentTileX - 1, 0, 9999);
				} else if(Input.GetAxis("Horizontal") > 0) {
					tx = (int)Mathf.Clamp(currentTileX + 1, 0, tileMap.width - 1);
				}
			}
			if (targetTileX == currentTileX && closeToDest) {
				if(Input.GetAxis("Vertical") > 0) {
					ty = (int)Mathf.Clamp(currentTileY + 1, 0, 9999);
				} else if(Input.GetAxis("Vertical") < 0) {
					ty = (int)Mathf.Clamp(currentTileY - 1, 0, tileMap.height - 1);
				}
			}
			Tile t = tileMap.getTile(tx, ty, 0);
			if(!t.isBlock) {
				targetTileX = tx;
				targetTileY = ty;

				AIController.MoveTo(t.gameObject.transform.position, characterInfo.moveSpeed, playerCharacter);
				tileMap.getTileCoordinateAt(playerCharacter.transform.position, ref currentTileX, ref currentTileY);
			}

			///	Debug.Log (tileMap.getTileAt(transform.position, 0));
		} else {
			playerCharacter.transform.Translate(characterInfo.moveSpeed * Input.GetAxis("Horizontal"), 0, 0);
			playerCharacter.transform.Translate(0, characterInfo.moveSpeed * Input.GetAxis("Vertical"), 0);
		}
	}
}
