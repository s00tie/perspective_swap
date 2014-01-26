using UnityEngine;
using System.Collections;

public class HighlightTextScript : MonoBehaviour {
	public string buttonName = "default";




	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//desiredRot = Camera.main.transform.rotation; //for getting info
	}



	void OnMouseEnter(){
		//change the color
		renderer.material.color = Color.green;
	}

	void OnMouseExit(){
		renderer.material.color = Color.white;
	}

	void OnMouseUp(){
		if(buttonName == "Play"){
			Application.LoadLevel("main");
		}
		if(buttonName == "Settings"){
			Camera.main.GetComponent<MainMenuCamera>().rotToSettings = true;
			Camera.main.GetComponent<MainMenuCamera>().rotToHelp = false;
			Camera.main.GetComponent<MainMenuCamera>().rotToMain = false;
			Camera.main.GetComponent<MainMenuCamera>().desiredRot = Camera.main.GetComponent<MainMenuCamera>().SettingsCameraQat;
			//Camera.main.transform.rotation = Quaternion.Slerp (Camera.main.transform.rotation, new Quaternion(Camera.main.transform.rotation.x,Camera.main.transform.rotation.y+90, Camera.main.transform.rotation.z,Camera.main.transform.rotation.w),0.01f);
		}
		if(buttonName == "Help"){
			Camera.main.GetComponent<MainMenuCamera>().rotToHelp = true;
			Camera.main.GetComponent<MainMenuCamera>().rotToSettings = false;
			Camera.main.GetComponent<MainMenuCamera>().rotToMain = false;
			Camera.main.GetComponent<MainMenuCamera>().desiredRot = Camera.main.GetComponent<MainMenuCamera>().HelpCameraQat;
		}
		if(buttonName == "Exit"){
			Application.Quit();
		}
		if(buttonName == "Main"){
			Camera.main.GetComponent<MainMenuCamera>().rotToHelp = false;
			Camera.main.GetComponent<MainMenuCamera>().rotToSettings = false;
			Camera.main.GetComponent<MainMenuCamera>().rotToMain = true;
			Camera.main.GetComponent<MainMenuCamera>().desiredRot = Camera.main.GetComponent<MainMenuCamera>().MainCameraQat;
		}
	}
}