using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private CharacterInfo characterInfo = null;

	void Start () {
		characterInfo = gameObject.GetComponent<CharacterInfo>();
		if (characterInfo ==  null) {
			enabled = false;
		}
	}

	void Update () {
		CheckInput();
	}
	
	private void CheckInput() {
		transform.Translate(characterInfo.moveSpeed * Input.GetAxis("Horizontal"), 0, 0);
		transform.Translate(0, characterInfo.moveSpeed * Input.GetAxis("Vertical"), 0);
	}
}
