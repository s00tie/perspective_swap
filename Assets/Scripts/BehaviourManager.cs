using UnityEngine;
using System.Collections;

public class BehaviourManager : MonoBehaviour {
	
	public GameObject wandererPrefab;

	public GameObject gameCamera;
	// Use this for initialization
	public int playerNumber=-1;

	GameObject curPlayer;
	
	public ArrayList wandererList = new ArrayList();
	
	
	void Start () {
		
		for(int i=0; i<3; i++){
			GameObject curObject=(GameObject)Instantiate(wandererPrefab, new Vector3(2*i,2,0), transform.rotation);
			curObject.GetComponent<Wanderer>().objectNumber=i;
			wandererList.Add(curObject);
		}

		curPlayer=(GameObject)wandererList[0];
		
	}
	
	// Update is called once per frame
	void Update () {
		curPlayer.GetComponent<Wanderer>().isPlayer=true;
		playerNumber=curPlayer.GetComponent<Wanderer>().objectNumber;


		Vector3 cameraPosition= gameCamera.transform.position;
		GameObject curWanderer=curPlayer;
		cameraPosition.x=curWanderer.transform.position.x;
		cameraPosition.z=curWanderer.transform.position.z;
		gameCamera.transform.position=cameraPosition;

		foreach(GameObject g in wandererList){
			Wanderer wandererData= g.GetComponent<Wanderer>();
			if(wandererData.objectNumber!=playerNumber && wandererData.hasCollision!=-1){
				curPlayer.GetComponent<Wanderer>().isPlayer=false;
				curPlayer.GetComponent<Wanderer>().hasCollision=-1;
				curPlayer= (GameObject)wandererList[wandererData.objectNumber];
				curPlayer.GetComponent<Wanderer>().isPlayer=true;
				curPlayer.GetComponent<Wanderer>().hasCollision=-1;
			}

		}






		ArrayList inputKeys= GetComponent<PlayerInputManager>().getInputKeys();
		Vector3 playerMovement= curPlayer.transform.position;
		if(inputKeys.Contains(KeyCode.W)){
			playerMovement.z+=2.0f * Time.deltaTime;
		}
		if(inputKeys.Contains(KeyCode.S)){
			playerMovement.z-=2.0f * Time.deltaTime;
		}
		if(inputKeys.Contains(KeyCode.A)){
			playerMovement.x-=2.0f * Time.deltaTime;
		}
		if(inputKeys.Contains(KeyCode.D)){
			playerMovement.x+=2.0f * Time.deltaTime;
		}
		curPlayer.transform.position=playerMovement;


	}
}
