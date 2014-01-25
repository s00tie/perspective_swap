using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public Camera mainCamera;
	public Camera altCamera;
	public float mainSplitWidth = 0.66f;
	public float splitGap = 0.001f;
	private bool showingAltCamera;

	void Start() {
		mainCamera.enabled = true;
		altCamera.enabled = false;
		showingAltCamera = false;
	}

	void Update() {
		if (!showingAltCamera) {
			if (CharacterManager.Instance.nearbyCharacters != null) {

			}
		} else {
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
		Rect mainCameraViewport = mainCamera.rect;
		mainCameraViewport.width = mainSplitWidth;
		mainCamera.rect = mainCameraViewport;
	}

	public void HideAltCamera() {
		altCamera.enabled = false;
		altCamera.transform.parent = null;
		Rect mainCameraViewport = mainCamera.rect;
		mainCameraViewport.width = 1.0f;
		mainCamera.rect = mainCameraViewport;
	}
}
