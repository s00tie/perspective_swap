using UnityEngine;
using System.Collections;

public class instructionScript : MonoBehaviour {
	public Texture2D WASDImg;
	public Texture2D TabImg;
	public Texture2D SpaceImg;
	private Texture2D currentTexture; 
	public GameObject player; 
	public GUIStyle _style;
	public string instruction = "To Move";
	// Use this for initialization
	void Start () {
		currentTexture = WASDImg;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
			currentTexture = TabImg;
			instruction = "To switch between characters";
		}

	}

	void OnGUI() {
		if(currentTexture != null)
		{
			GUI.DrawTexture(new Rect(200, 10, 200, 200), currentTexture, ScaleMode.ScaleToFit, true);
			GUI.Label(new Rect(510, 150, 200, 60), instruction, _style);
			
		}
	}
}
