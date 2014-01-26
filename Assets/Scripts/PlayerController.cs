using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerCharacter = null;
	private CharacterInfo characterInfo = null;
	public TileMap tileMap;
	private float tileDestEpsilon = 0.00005f;
	public Occupation occupation = Occupation.BREAK;

	int currentTileX, currentTileY;
	int targetTileX, targetTileY;
	int tileMoveX, tileMoveY;

	public enum Occupation {
		NONE = 0,
		BREAK,
		CLIMB,
		FLOAT,
		MOVE,
		TUNNEL
	}

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

	void MoveTo(Vector3 target_pos) {
		Vector3 this_pos = playerCharacter.transform.position;
		float distX = target_pos.x - this_pos.x;
		float distY = target_pos.y - this_pos.y;
		float move_speed = characterInfo.moveSpeed;

		if(Mathf.Abs (distX) > move_speed) {
			playerCharacter.transform.Translate(move_speed * Mathf.Sign(distX), 0, 0);
		} else {
			playerCharacter.transform.Translate(distX, 0, 0);
		}

		if(Mathf.Abs (distY) > move_speed) {
			playerCharacter.transform.Translate(0, move_speed * Mathf.Sign(distY), 0);
		} else {
			playerCharacter.transform.Translate(0, distY, 0);
		}

		bool adjacentSpecial;
		Tile frontTile = tileMap.getTile(currentTileX + tileMoveX, currentTileY + tileMoveY, 0);
		switch (occupation) {
		case Occupation.BREAK:
			if(frontTile.isBreakable) {
				TileHolder.Instance.SwapInNewTile(0, currentTileX + tileMoveX, currentTileY + tileMoveY);
			}
				break;
		case Occupation.CLIMB:
			if(frontTile.isClimbable) {
				
			}
				break;
		case Occupation.FLOAT:
			if(frontTile.isFloatable) {
				
			}
				break;
		case Occupation.MOVE:
			if(frontTile.isMoveable) {
				
			}
				break;
		case Occupation.TUNNEL:
			if(frontTile.isTunnelable) {
				
			}
				break;
		}
	}
	
	private void CheckInput() {
		if(tileMap != null)  {
			int tx = targetTileX, ty = targetTileY;
			bool closeToDestination = Mathf.Abs(playerCharacter.transform.position.x - (tileMap.startPoint.x + targetTileX * tileMap.xStep) - tileMap.xStep / 2) < tileDestEpsilon &&
				Mathf.Abs(playerCharacter.transform.position.y - (tileMap.startPoint.y + targetTileY * tileMap.yStep) - tileMap.yStep / 2) < tileDestEpsilon;
			if(targetTileY == currentTileY && closeToDestination) {
				if(Input.GetAxis("Horizontal") < 0) {
					tx = (int)Mathf.Clamp(currentTileX - 1, 0, 9999);
					tileMoveX = 0;
				} else if(Input.GetAxis("Horizontal") > 0) {
					tx = (int)Mathf.Clamp(currentTileX + 1, 0, tileMap.width - 1);
					tileMoveX = 0;
				}
			}
			if (targetTileX == currentTileX && closeToDestination) {
				if(Input.GetAxis("Vertical") > 0) {
					ty = (int)Mathf.Clamp(currentTileY + 1, 0, 9999);
					tileMoveY = 0;
				} else if(Input.GetAxis("Vertical") < 0) {
					ty = (int)Mathf.Clamp(currentTileY - 1, 0, tileMap.height - 1);
					tileMoveY = 0;
				}
			}

			if (targetTileX != currentTileX) {
				tileMoveX = targetTileX - currentTileX;
			}
			if (targetTileY != currentTileY) {
				tileMoveY = targetTileY - currentTileY;
			}

			Tile t = tileMap.getTile(tx, ty, 0);
			if(!t.isBlock) {
				targetTileX = tx;
				targetTileY = ty;

				this.MoveTo(t.gameObject.transform.position);
				tileMap.getTileCoordinateAt(playerCharacter.transform.position, ref currentTileX, ref currentTileY);
			}

			///	Debug.Log (tileMap.getTileAt(transform.position, 0));
		} else {
			playerCharacter.transform.Translate(characterInfo.moveSpeed * Input.GetAxis("Horizontal"), 0, 0);
			playerCharacter.transform.Translate(0, characterInfo.moveSpeed * Input.GetAxis("Vertical"), 0);
		}
	}
}
