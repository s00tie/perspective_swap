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

	float currentTime;
	bool moveReady;
	List<Vector3> routePoints;
	Vector3 targetPos;

	void Start () {
		currentTime = 0;
		moveReady = true;
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

		int currentTileX = 0, currentTileY = 0;
		tileMap.getTileCoordinateAt(characterInfo.gameObject.transform.position, ref currentTileX, ref currentTileY);

		Tile[] route = tileMap.AStar(currentTileX, currentTileY,
		       	    	    	     currentTileX + tx, currentTileY + ty,
		        	    	         this.characterInfo.tag,
		                    	  	 1);

		if(route.Length > 0) {
			moveReady = false;
			routePoints = new List<Vector3>();
			foreach(Tile t in route) {
				routePoints.Add (t.gameObject.transform.position);
			}
			string d = "";
			foreach(Vector3 pos in routePoints) {
				d += "(" + pos.ToString() + ") -> ";
			}
			Debug.Log(d);
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

	void onWanderFinished() {
		moveReady = true;
	}
	
	void moveTo(Vector3 target_pos) {
		Vector3 this_pos = characterInfo.gameObject.transform.position;
		float distX = target_pos.x - this_pos.x;
		float distY = target_pos.y - this_pos.y;
		float move_speed = characterInfo.moveSpeed * Time.deltaTime;
		
		if(Mathf.Abs (distX) > move_speed) {
			characterInfo.gameObject.transform.Translate(move_speed * Mathf.Sign(distX), 0, 0);
		} else {
			characterInfo.gameObject.transform.Translate(distX, 0, 0);
		}
		
		if(Mathf.Abs (distY) > move_speed) {
			characterInfo.gameObject.transform.Translate(0, move_speed * Mathf.Sign(distY), 0);
		} else {
			characterInfo.gameObject.transform.Translate(0, distY, 0);
		}

		string animName = "";
		if(Mathf.Abs (distX) > Mathf.Abs (distY)) {
			if(distX < 0) {
				animName = "skinless_walk_left";
			} else {
				animName = "skinless_walk_right";
			}
		} else {
			if(distY < 0) {
				animName = "skinless_walk_down";
			} else {
				animName = "skinless_walk_up";
			}
		}
		if(animName.Length > 0) {
			Animator animator = characterInfo.gameObject.GetComponent<Animator>();
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
				this.moveTo(targetPos);
			} else {
				Debug.Log("next");
				if(routePoints.Count > 0) {
					targetPos = routePoints.Last();
					routePoints.RemoveAt (routePoints.Count - 1);
				} else {
					Debug.Log("fin");
					moveReady = true;
				}

			}
		}

	}

	void Update()  {
		currentTime += Time.deltaTime;
		if(currentTime >= this.wanderInterval) {
			if(this.moveReady) 
				this.wander();

			currentTime = 0f;
		}

		this.updateMovement();
	}


}