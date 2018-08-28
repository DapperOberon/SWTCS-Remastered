using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TX130))]
public class TX130Player : TX130 {

	private float deadZone = 0.1f;
	private float currThrust = 0f;
	private bool bIsBoosting = false;
	private float currBoostTime = 0f;
	private float currStrafe = 0f;
	private float currTurn = 0f;

	private TX130 tankRef;

	// Use this for initialization
	void Start () {		
		// Get tank reference
		tankRef = GetComponent<TX130>();

		currBoostTime = tankRef.tankStats.maxBoostTime;
	}

	// Update is called once per frame
	void Update () {
		GetInput();
	}

	private void FixedUpdate()
	{
		// Using tank ref apply forces
		tankRef.ApplyForces(currThrust, bIsBoosting, currBoostTime, currStrafe, currTurn * Vector3.up);
	}

	public float getCurrBoostTime()
	{
		return currBoostTime;
	}

	private void GetInput()
	{
		// Main Thrust
		float aclAxis = Input.GetAxis("Vertical");
		if (aclAxis > deadZone)
		{
			currThrust = aclAxis * tankRef.tankStats.forwardAcl;
		}
		else if (aclAxis < deadZone)
		{
			currThrust = aclAxis * tankRef.tankStats.backwardAcl;
		}
		else
		{
			currThrust = 0;
		}

		// Boost Thrust
		if (!bIsBoosting && currBoostTime < tankRef.tankStats.maxBoostTime)
		{
			currBoostTime += Time.deltaTime;
		} else if (bIsBoosting && currBoostTime >= 0f)
		{
			currBoostTime -= Time.deltaTime;
		}

		bool boostInput = Input.GetButton("Boost");
		if (boostInput)
		{
			bIsBoosting = true;
			currThrust = tankRef.tankStats.boostAcl;
		}
		else
		{
			bIsBoosting = false;
		}

		// Strafe Thrust
		float strafeAxis = Input.GetAxis("Strafe");
		if (Mathf.Abs(strafeAxis) > deadZone)
		{
			currStrafe = strafeAxis * tankRef.tankStats.strafeAcl;

		}
		else
		{
			currStrafe = 0;
		}

		// Turning
		float turnAxis = Input.GetAxis("Horizontal");
		if (Mathf.Abs(turnAxis) > deadZone)
		{
			currTurn = turnAxis;
		}
		else
		{
			currTurn = 0;
		}
	}
}
