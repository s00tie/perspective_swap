using UnityEngine;
using System.Collections;

public class CharacterInfo : MonoBehaviour
{
	public float moveSpeed = 1f;
	public float shareRadius = 3.0f;
	public float distanceFromCharacter = -1.0f;

	public string tag = "";
	public TileMap tileMap;
	public GameObject interactionBubble = null;

	public enum Occupation {
		NONE = 0,
		BREAK,
		CLIMB,
		FLOAT,
		MOVE,
		TUNNEL
	}
	public Occupation occupation = Occupation.BREAK;
}

