using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private CharacterInfo characterInfo = null;
	public TileMap tileMap;

	int currentTileX, currentTileY;
	int targetTileX, targetTileY;

	void Start () {
		characterInfo = gameObject.GetComponent<CharacterInfo>();
		if (characterInfo ==  null) {
			enabled = false;
		}
		if(tileMap != null)
			tileMap.getTileCoordinateAt(this.gameObject.transform.position, ref currentTileX, ref currentTileY);
		targetTileX = currentTileX;
		targetTileY = currentTileY;
	}

	void Update () {
		CheckInput();
	}

	void MoveTo(Vector3 target_pos) {
		Vector3 this_pos = this.gameObject.transform.position;
		float distX = target_pos.x - this_pos.x;
		float distY = target_pos.y - this_pos.y;
		float move_speed = characterInfo.moveSpeed;

		if(Mathf.Abs (distX) > move_speed) {
			transform.Translate(move_speed * Mathf.Sign(distX), 0, 0);
		} else {
			transform.Translate(distX, 0, 0);
		}

		if(Mathf.Abs (distY) > move_speed) {
			transform.Translate(0, move_speed * Mathf.Sign(distY), 0);
		} else {
			transform.Translate(0, distY, 0);
		}
	}
	
	private void CheckInput() {
		if(tileMap != null)  {
			int tx = targetTileX, ty = targetTileY;
			if(targetTileY == currentTileY) {
				if(Input.GetKey(KeyCode.LeftArrow)) {
					tx = (int)Mathf.Clamp(currentTileX - 1, 0, 9999);
				} else if(Input.GetKey(KeyCode.RightArrow)) {
					tx = (int)Mathf.Clamp(currentTileX + 1, 0, tileMap.width - 1);
				}
			}
			if(targetTileX == currentTileX) {
				if(Input.GetKey(KeyCode.UpArrow)) {
					ty = (int)Mathf.Clamp(currentTileY + 1, 0, 9999);
				} else if(Input.GetKey(KeyCode.DownArrow)) {
					ty = (int)Mathf.Clamp(currentTileY - 1, 0, tileMap.height - 1);
				}
			}
			Tile t = tileMap.getTile(tx, ty, 0);
			if(!t.isBlock && !t.isHole) {
				targetTileX = tx;
				targetTileY = ty;

				this.MoveTo(t.gameObject.transform.position);
				tileMap.getTileCoordinateAt(this.gameObject.transform.position, ref currentTileX, ref currentTileY);
			}

			///	Debug.Log (tileMap.getTileAt(transform.position, 0));
		} else {
			transform.Translate(characterInfo.moveSpeed * Input.GetAxis("Horizontal"), 0, 0);
			transform.Translate(0, characterInfo.moveSpeed * Input.GetAxis("Vertical"), 0);
		}
	}
}
