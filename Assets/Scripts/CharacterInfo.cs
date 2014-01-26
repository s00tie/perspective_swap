using UnityEngine;
using System.Collections;

public class CharacterInfo : MonoBehaviour
{
	public float moveSpeed = 0.2f;
	public float shareRadius = 3.0f;
	public float distanceFromCharacter = -1.0f;

	public string tag = "";
	public TileMap tileMap;

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

