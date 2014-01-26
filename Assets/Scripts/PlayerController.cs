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

	void MoveTo(Vector3 target_pos) {
		Vector3 this_pos = playerCharacter.transform.position;
		float distX = target_pos.x - this_pos.x;
		float distY = target_pos.y - this_pos.y;
		float move_speed = characterInfo.moveSpeed * Time.deltaTime;

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
	}
	
	private void CheckInput() {
		if(tileMap != null)  {
			int tx = targetTileX, ty = targetTileY;
			Debug.Log (Mathf.Abs(playerCharacter.transform.position.x - (tileMap.startPoint.x + targetTileX * tileMap.xStep) - tileMap.xStep / 2));
			bool closeToDestination = Mathf.Abs(playerCharacter.transform.position.x - (tileMap.startPoint.x + targetTileX * tileMap.xStep) - tileMap.xStep / 2) < tileDestEpsilon &&
				Mathf.Abs(playerCharacter.transform.position.y - (tileMap.startPoint.y + targetTileY * tileMap.yStep) - tileMap.yStep / 2) < tileDestEpsilon;

			Animator animator = this.gameObject.GetComponent<Animator>();
			if(targetTileY == currentTileY && closeToDestination) {
				if(Input.GetAxis("Horizontal") < 0) {
					tx = (int)Mathf.Clamp(currentTileX - 1, 0, 9999);
					animator.Play("skinless_walk_left");
				} else if(Input.GetAxis("Horizontal") > 0) {
					tx = (int)Mathf.Clamp(currentTileX + 1, 0, tileMap.width - 1);
					animator.Play("skinless_walk_right");
				}
			}
			if (targetTileX == currentTileX && closeToDestination) {
				if(Input.GetAxis("Vertical") > 0) {
					ty = (int)Mathf.Clamp(currentTileY + 1, 0, 9999);
					animator.Play("skinless_walk_up");
				} else if(Input.GetAxis("Vertical") < 0) {
					ty = (int)Mathf.Clamp(currentTileY - 1, 0, tileMap.height - 1);
					animator.Play("skinless_walk");
				}
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
