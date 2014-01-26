using UnityEngine;
using System.Collections;

public class CharacterAnimationController : MonoBehaviour {
	public AnimationClip test = null;

	void Start() {

	}

	void Update() {
		if (!animation.isPlaying) {
			animator.Play("skinless_walk_left");
		}
	}
}
