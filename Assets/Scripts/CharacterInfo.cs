using UnityEngine;
using System.Collections;

public class CharacterInfo : MonoBehaviour
{
	public float moveSpeed = 1f;
	public float shareRadius = 3.0f;
	public float distanceFromCharacter = -1.0f;

	public string tag = "";
	public TileMap tileMap;
	public int tileLayer = 0;
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


	public bool makeScreenBlack = false;
	public float screenBlackAlphaThreshold = 0.3f;
	public float screenBlackDistance = 3f;
}

