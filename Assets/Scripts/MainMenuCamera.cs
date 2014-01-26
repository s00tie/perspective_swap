using UnityEngine;
using System.Collections;

public class MainMenuCamera : MonoBehaviour {
	public Quaternion desiredRot;
	public Quaternion SettingsCameraQat;
	public Quaternion HelpCameraQat;
	public Quaternion MainCameraQat;

	public bool rotToSettings = false;
	public bool rotToHelp = false;
	public bool rotToMain = false;
	// Use this for initialization
	void Start () {
		SettingsCameraQat = new Quaternion(0.06f,0.73f,-.06f,0.67f);
		HelpCameraQat = new Quaternion(0.05f,-.74f,0.07f,0.67f);
		MainCameraQat = new Quaternion(.09f,.04f,.004f,.995f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, desiredRot, .1f);
	}
}
