using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/TX130Stats")]
public class TX130Stats : ScriptableObject {

	[Header("Force Settings")]
	// Thrust variables
	public float forwardForce = 6000f;
	public float backwardForce = 4000f;
	public float boostMultiplier = 2f;
	// Max boost time in seconds
	public float maxBoostTime = 10f;
	public float strafeForce = 3000f;
	public float turnStrength = 2000f;
	public float slowingVelFactor = .99f;

	[Header("Hover Settings")]
	// Hover variables
	public float hoverForce = 2000f;
	// Hover height in meters
	public float hoverHeight = 5f;
	public float maxGroundDist = 10f;

	[Header("Gravity Settings")]
	// Gravity variables
	public float hoverGravity = 98f;
	public float fallGravity = 294f;

	[Header("Cosmetic Settings")]
	// Cosmetic variables
	public float angleOfRoll = 5f;
}
