using UnityEngine;
using System.Collections;

public class PlayerInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public ArrayList inputKeys = new ArrayList();
	public ArrayList getInputKeys(){
		return inputKeys;
	}
	// Update is called once per frame
	void Update () {


		if(Input.GetKeyDown(KeyCode.S) && !inputKeys.Contains(KeyCode.S)){
			inputKeys.Add(KeyCode.S);
		}
		else if(Input.GetKeyUp(KeyCode.S) && inputKeys.Contains(KeyCode.S)){
			inputKeys.Remove(KeyCode.S);
		}

		if(Input.GetKeyDown(KeyCode.W) && !inputKeys.Contains(KeyCode.W)){
			inputKeys.Add(KeyCode.W);
		}else if(Input.GetKeyUp(KeyCode.W) && inputKeys.Contains(KeyCode.W)){
			inputKeys.Remove(KeyCode.W);
		}

		if(Input.GetKeyDown(KeyCode.A)&& !inputKeys.Contains(KeyCode.A) ){
			inputKeys.Add(KeyCode.A);
		}else if(Input.GetKeyUp(KeyCode.A) && inputKeys.Contains(KeyCode.A)){
			inputKeys.Remove(KeyCode.A);
		}

		if(Input.GetKeyDown(KeyCode.D)&& !inputKeys.Contains(KeyCode.D) ){
			inputKeys.Add(KeyCode.D);
		}else if(Input.GetKeyUp(KeyCode.D) && inputKeys.Contains(KeyCode.D)){
			inputKeys.Remove(KeyCode.D);
		}




	}
}
