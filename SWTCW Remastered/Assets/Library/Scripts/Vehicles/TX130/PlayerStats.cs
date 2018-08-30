using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject {

	[HideInInspector]
	public float currBoostTime = 0f; // Used for accesing from other objects (e.g., GUI)
}
