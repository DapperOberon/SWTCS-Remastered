using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	// Thruster axis name
	public string verticalAxisName = "Vertical";
	// Rudder axis name
	public string horizontalAxisName = "Horizontal";
	// Strafe axis name
	public string strafeAxisName = "Strafe";
	// Boost button name
	public string boostButtonName = "Boost";

	// Current force values
	[HideInInspector] public float currThruster;
	[HideInInspector] public float currRudder;
	[HideInInspector] public float currStrafe;
	[HideInInspector] public bool bIsBoosting;

	// Update is called once per frame
	void Update () {
		currThruster = Input.GetAxis(verticalAxisName);
		currRudder = Input.GetAxis(horizontalAxisName);
		currStrafe = Input.GetAxis(strafeAxisName);
		bIsBoosting = Input.GetButton(boostButtonName);
	}
}
