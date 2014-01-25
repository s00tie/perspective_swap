using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public Camera mainCamera;
	public Camera altCamera;
	public float mainSplitWidth = 0.66f;
	public float splitGap = 0.001f;
	private bool showingAltCamera;
	private int targetIndex = 0;

	void Start() {
		mainCamera.enabled = true;
		altCamera.enabled = false;
		showingAltCamera = false;
	}

	void Update() {
		if (CharacterManager.Instance.nearbyCharacters.Count > 0) {
			if (!showingAltCamera) {
				/* TODO: should probably start at closest */
				targetIndex = 0;
				ShowAltCamera(CharacterManager.Instance.nearbyCharacters[targetIndex], true);
			} else if (Input.GetAxis("Target") != 0) {
				targetIndex++;
				if (targetIndex >= CharacterManager.Instance.nearbyCharacters.Count) {
					targetIndex = 0;
				}
				ShowAltCamera(CharacterManager.Instance.nearbyCharacters[targetIndex], true);
			}
		} else if (showingAltCamera){
			if (CharacterManager.Instance.nearbyCharacters == null) {
				HideAltCamera();
			}
		}
	}
	
	public void ShowAltCamera(CharacterInfo targetCharacter, bool rightSide) {
		altCamera.enabled = true;
		altCamera.transform.parent = targetCharacter.transform;
		float altCameraX = mainSplitWidth + splitGap;
		float altCameraWidth = 1 - altCameraX;
		Rect altCameraViewport = altCamera.rect;
		altCameraViewport.x = altCameraX;
		altCameraViewport.width = altCameraWidth;
		altCamera.rect = altCameraViewport;
		altCamera.transform.Translate(-altCamera.transform.position.x, -altCamera.transform.position.y, 0);// = new Vector3(0.0f, 0.0f, altCamera.transform.position.z);
		Rect mainCameraViewport = mainCamera.rect;
		mainCameraViewport.width = mainSplitWidth;
		mainCamera.rect = mainCameraViewport;
		showingAltCamera = true;
	}

	public void HideAltCamera() {
		altCamera.enabled = false;
		altCamera.transform.parent = null;
		Rect mainCameraViewport = mainCamera.rect;
		mainCameraViewport.width = 1.0f;
		mainCamera.rect = mainCameraViewport;
		showingAltCamera = false;
	}
}
