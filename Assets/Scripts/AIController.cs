using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class AIController: MonoBehaviour {

	public CharacterInfo characterInfo;
	public TileMap tileMap;

	public int wanderRadius = 1;
	public float wanderInterval = 5f;
	public bool enableWandering = true;

	float currentTime;
	bool moveReady;
	List<Vector3> routePoints;
	Vector3 targetPos;
	
	int currentTileX = 0, currentTileY = 0;

	Action finishCallback = null;

	void Start () {
		currentTime = 0;
		moveReady = true;
		characterInfo = GetComponent<CharacterInfo>();
	}

	bool isClose(Vector3 p1, Vector3 p2) {
		return Mathf.Abs (p2.x - p1.x) < 0.00005f &&
				Mathf.Abs (p2.y - p1.y) < 0.00005f;
	}

	void wander() {
		if(tileMap == null)
			return;

		int tx = UnityEngine.Random.Range(-wanderRadius, wanderRadius);
		int ty = UnityEngine.Random.Range(-wanderRadius, wanderRadius);

		tileMap.getTileCoordinateAt(characterInfo.gameObject.transform.position, ref currentTileX, ref currentTileY);
		this.nagivateTo(currentTileX + tx, currentTileY + ty);
	}

	void onWanderFinished() {
		moveReady = true;
	}

	public void nagivateTo(int x, int y, Action cb = null) {
		tileMap.getTileCoordinateAt(characterInfo.gameObject.transform.position, ref currentTileX, ref currentTileY);
		Tile[] route = tileMap.AStar(currentTileX, currentTileY,
		                             x, y,
		                             this.characterInfo.tag,
		                             1);
		
		if(route.Length > 0) {
			moveReady = false;
			routePoints = new List<Vector3>();
			foreach(Tile t in route) {
				routePoints.Add (t.gameObject.transform.position);
			}
			if(routePoints.Count > 0)
				targetPos = routePoints[routePoints.Count - 1];

			finishCallback = cb;
			/*
			iTween.MoveTo(characterInfo.gameObject,
			              iTween.Hash("path", routePoints.ToArray(), 
			            			  "time", routePoints.Count * tileMap.xStep / characterInfo.moveSpeed, 
			            			  "oncomplete", "onWanderFinished",
			            			  "oncompletetarget", this.gameObject,
			            			  "easetype", iTween.EaseType.linear));
*/
		}
	}
	
	public static void MoveTo(Vector3 target_pos, float speed, GameObject go) {
		Vector3 this_pos = go.transform.position;
		float distX = target_pos.x - this_pos.x;
		float distY = target_pos.y - this_pos.y;
		float move_speed = speed * Time.deltaTime;
		
		if(Mathf.Abs (distX) > move_speed) {
			go.transform.Translate(move_speed * Mathf.Sign(distX), 0, 0);
		} else {
			go.transform.Translate(distX, 0, 0);
		}
		
		if(Mathf.Abs (distY) > move_speed) {
			go.transform.Translate(0, move_speed * Mathf.Sign(distY), 0);
		} else {
			go.transform.Translate(0, distY, 0);
		}

		string animName = "";
		if(Mathf.Abs(distX) < 0.0000005f &&
		   Mathf.Abs(distY) < 0.0000005f) {
			return;
			
		}

		if(Mathf.Abs (distX) > Mathf.Abs (distY)) {
			if(distX < 0) {
				animName = "walk_left";
			} else {
				animName = "walk_right";
			}
		} else {
			if(distY < 0) {
				animName = "walk_down";
			} else {
				animName = "walk_up";
			}
		}
		if(animName.Length > 0) {
			Animator animator = go.GetComponent<Animator>();
			if(animator != null) {
				animator.StopPlayback();
				animator.Play(animName);
			}
		}
	}

	void updateMovement() {
		if(!moveReady) {
			Vector3 pos = this.characterInfo.gameObject.transform.position;
			if(!isClose (pos, targetPos)) {
				AIController.MoveTo(targetPos, characterInfo.moveSpeed, characterInfo.gameObject);
			} else {
				if(routePoints.Count > 0) {
					targetPos = routePoints.Last();
					routePoints.RemoveAt (routePoints.Count - 1);
				} else {
					moveReady = true;
					if(finishCallback != null)
						finishCallback();
				}

			}
		}
	}

	void Update()  {
		if(enableWandering) {
			currentTime += Time.deltaTime;
			if(currentTime >= this.wanderInterval) {
				if(this.moveReady) 
					this.wander();
					
				currentTime = 0f;
			}
		}
		this.updateMovement();
	}


}