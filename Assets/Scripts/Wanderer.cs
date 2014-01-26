using UnityEngine;
using System.Collections;

public class Wanderer : MonoBehaviour {

	// Use this for initialization


	public bool isPlayer;
	public int objectNumber;
	public int hasCollision= -1;
	void Start () {
		isPlayer=false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isPlayer==true){
			Vector3 wandererPosition= transform.position;
			wandererPosition.z+=1f*Time.deltaTime;
			transform.position=wandererPosition;
		}
	}

	void OnTriggerEnter(Collider collision)
	{
		//get the collision object when a new collision occurs
		int result= collision.gameObject.GetComponent<Wanderer>().objectNumber;
		hasCollision= result;
		//Debug.Log (objectNumber + " hit by " + result);
	}  
}
