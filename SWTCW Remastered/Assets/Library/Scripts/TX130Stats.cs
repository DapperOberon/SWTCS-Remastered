using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/TX130Stats")]
public class TX130Stats : ScriptableObject {

	// Thrust variables
	public float forwardAcl = 8000f;
	public float backwardAcl = 4000f;
	public float boostAcl = 8500f;
	// Max boost time in seconds
	public float maxBoostTime = 10f;
	public float strafeAcl = 4000f;
	public float turnStrength = 4000f;

	// Hover variables
	public float hoverForce = 500f;
	// Hover height in meters
	public float hoverHeight = 2f;
}
