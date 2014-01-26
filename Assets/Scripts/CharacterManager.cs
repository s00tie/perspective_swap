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
		if(playerCharacter == null)
			return;

		nearbyCharacters.Clear();
		for (int i = 0; i < characters.Count; i++) {
			CharacterInfo testCharacter = characters[i];

			float distanceFromPlayer  = (testCharacter.transform.position - playerCharacter.transform.position).sqrMagnitude;
			if (testCharacter != playerCharacter && distanceFromPlayer < playerCharacter.shareRadius * playerCharacter.shareRadius) {

				testCharacter.distanceFromCharacter = distanceFromPlayer;
				nearbyCharacters.Add(testCharacter);
			}
		}

		nearbyCharacters.Sort(SortByDistance);
	}

	// Sort the nearby characters by distance
	private static int SortByDistance(CharacterInfo o1, CharacterInfo o2) {
		 if( o1.distanceFromCharacter < o2.distanceFromCharacter )
			return -1;
		 else 
			return 1;
	}
	
}

