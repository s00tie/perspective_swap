using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {
	public Camera mainCamera;
	public Camera altCamera;
	public float mainSplitWidth = 0.66f;
	public float splitGap = 0.001f;
	private bool showingAltCamera;
	private int targetIndex = 0;
	private CharacterInfo oldTargetCharacter = null;
	private bool readyToRetarget = true;
	public TileMap tileMap;

	void Start() {
		mainCamera.enabled = true;
		altCamera.enabled = false;
		HideAltCamera();
		//mainCamera.transform.parent = CharacterManager.Instance.playerCharacter.transform;
		//mainCamera.transform.position = new Vector3(0, 0, -100);
	}

	void Update() {
		/*if (mainCamera.transform.parent != CharacterManager.Instance.playerCharacter.transform)
		{
			mainCamera.transform.parent = CharacterManager.Instance.playerCharacter.transform;
			mainCamera.transform.position = new Vector3(0, 0, -100);
		}

		Vector3 mainCameraPos = mainCamera.transform.position;
		mainCameraPos.z = -100;
		mainCamera.transform.position = mainCameraPos;
		Vector3 altCameraPos = altCamera.transform.position;
		altCameraPos.z = -100;
		altCamera.transform.position = altCameraPos;*/

		if (CharacterManager.Instance.nearbyCharacters.Count > 0) {
			if (!showingAltCamera) {
				/* TODO: should probably start at closest */
				targetIndex = 0;
				ShowAltCamera(CharacterManager.Instance.nearbyCharacters[targetIndex], true);
			} 
			// Switch between possible characters
			else if (Input.GetKeyDown(KeyCode.Tab))
			{
				// Go to next character
				List<CharacterInfo> nearbyCharacter = CharacterManager.Instance.nearbyCharacters;
				List<CharacterInfo> characters = CharacterManager.Instance.characters;
				int currentIndex = characters.IndexOf(oldTargetCharacter);
				CharacterInfo currentInfo = null;

				int targetIndex = -1;
				while( targetIndex < 0 )
				{
					++currentIndex;
					if( currentIndex >= characters.Count )
					{
						currentIndex = 0;
					}

					currentInfo = characters[currentIndex];
					if( nearbyCharacter.Contains( currentInfo ))
					{
						targetIndex = nearbyCharacter.IndexOf( currentInfo );
					}

				}


				ShowAltCamera(CharacterManager.Instance.nearbyCharacters[targetIndex], true);
			}
			else if (Input.GetAxis("Target") != 0 && readyToRetarget) {
				targetIndex++;
				if (targetIndex >= CharacterManager.Instance.nearbyCharacters.Count) {
					targetIndex = 0;
				}
				ShowAltCamera(CharacterManager.Instance.nearbyCharacters[targetIndex], true);
				readyToRetarget = false;
			} else if (Input.GetAxis("Target") == 0 && !readyToRetarget) {
				readyToRetarget = true;
			}
			// Old selection is out of range, swap to closest
			else if( !CharacterManager.Instance.nearbyCharacters.Contains( oldTargetCharacter ) )
			{
				targetIndex = 0;
				ShowAltCamera(CharacterManager.Instance.nearbyCharacters[targetIndex], true);
			}
		} else if (showingAltCamera){
			if (CharacterManager.Instance.nearbyCharacters != null) {
				HideAltCamera();
			}
		}
	}
	
	public void ShowAltCamera(CharacterInfo targetCharacter, bool rightSide) {
		Vector3 cameraShift = targetCharacter.transform.position;
		if (oldTargetCharacter != null) {
			cameraShift -= oldTargetCharacter.transform.position;
			cameraShift.z = 0;
		}
		oldTargetCharacter = targetCharacter;
		altCamera.enabled = true;
		//altCamera.transform.parent = targetCharacter.transform;
		float altCameraX = mainSplitWidth + splitGap;
		float altCameraWidth = 1 - altCameraX;
		Rect altCameraViewport = altCamera.rect;
		altCameraViewport.x = altCameraX;
		altCameraViewport.width = altCameraWidth;
		altCamera.rect = altCameraViewport;
		altCamera.transform.Translate(cameraShift);
		Rect mainCameraViewport = mainCamera.rect;
		mainCameraViewport.width = mainSplitWidth;
		mainCamera.rect = mainCameraViewport;
		showingAltCamera = true;
		tileMap.showLayerInGroupWithTag(targetCharacter.tag, 1, false);
	}

	public void HideAltCamera() {
		altCamera.enabled = false;
		//altCamera.transform.parent = null;
		altCamera.transform.position = new Vector3();
		Rect mainCameraViewport = mainCamera.rect;
		mainCameraViewport.width = 1.0f;
		mainCamera.rect = mainCameraViewport;
		showingAltCamera = false;
	}
}
