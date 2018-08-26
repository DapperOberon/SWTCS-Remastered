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
		currBoostTime = TX130.maxBoostTime;
		
		// Get tank reference
		tankRef = GetComponent<TX130>();
	}

	// Update is called once per frame
	void Update () {
		GetInput();
	}

	private void FixedUpdate()
	{
		// Using tank ref apply forces
		tankRef.ApplyForces(currThrust, bIsBoosting, currBoostTime, currStrafe, currTurn);
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
			currThrust = aclAxis * TX130.forwardAcl;
		}
		else if (aclAxis < deadZone)
		{
			currThrust = aclAxis * TX130.backwardAcl;
		}
		else
		{
			currThrust = 0;
		}

		// Boost Thrust
		if (!bIsBoosting && currBoostTime < TX130.maxBoostTime)
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
			currThrust = TX130.boostAcl;
		}
		else
		{
			bIsBoosting = false;
		}

		// Strafe Thrust
		float strafeAxis = Input.GetAxis("Strafe");
		if (Mathf.Abs(strafeAxis) > deadZone)
		{
			currStrafe = strafeAxis * TX130.strafeAcl;

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
