using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
	private static CharacterManager instance = null;
	public static CharacterManager Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindGameObjectWithTag("Globals").GetComponent<CharacterManager>();
			}
			return instance;
		}
	}

	public List<CharacterInfo> characters = null;
	public List<CharacterInfo> nearbyCharacters = null;
	public CharacterInfo playerCharacter = null;
	public CharacterInfo targetCharacter = null;

	void Start () {
		GameObject[] foundCharacters = GameObject.FindGameObjectsWithTag("Character");
		for (int i = 0; i < foundCharacters.Length; i++) {
			CharacterInfo foundInfo = foundCharacters[i].GetComponent<CharacterInfo>();
			if (foundInfo != null) {
				characters.Add(foundInfo);
			}
		}
	}

	void Update () {
		nearbyCharacters.Clear();
		for (int i = 0; i < characters.Count; i++) {
			CharacterInfo testCharacter = characters[i];
			if (testCharacter != playerCharacter && (testCharacter.transform.position - playerCharacter.transform.position).sqrMagnitude < playerCharacter.shareRadius * playerCharacter.shareRadius) {
				nearbyCharacters.Add(testCharacter);
			}
		}
	}
}

