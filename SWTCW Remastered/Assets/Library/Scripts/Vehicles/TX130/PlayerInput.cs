using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	// Thruster axis name
	public string verticalAxisName = "Vertical";
	// Rudder axis name
	public string horizontalAxisName = "Horizontal";
	// Brake button
	public string strafeAxisName = "Strafe";

	// Current force values
	[HideInInspector] public float currThruster;
	[HideInInspector] public float currRudder;
	[HideInInspector] public float currStrafe;

	// Update is called once per frame
	void Update () {
		currThruster = Input.GetAxis(verticalAxisName);
		currRudder = Input.GetAxis(horizontalAxisName);
		currStrafe = Input.GetAxis(strafeAxisName);
	}
}
