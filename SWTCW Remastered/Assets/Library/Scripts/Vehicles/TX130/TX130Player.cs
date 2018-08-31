using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(TX130))]
public class TX130Player : MonoBehaviour {

	public TX130Stats tankStats;
	private PlayerInput input;
	private TX130 tankRef;

	private float currThrust;
	private float currStrafe;
	private float currRudder;
	private bool bIsBoosting;
	private float currBoostTime = 0f;

	private void Start()
	{
		input = GetComponent<PlayerInput>();
		tankRef = GetComponent<TX130>();

		// Reset boost to max at start of scene
		currBoostTime = tankStats.maxBoostTime;
	}


	private void FixedUpdate()
	{
		tankRef.CalculateHover(currStrafe, currRudder);
		tankRef.CalculatePropulsion(currThrust, currStrafe, currRudder, bIsBoosting);
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
	}

	private void GetInput()
	{
		currThrust = input.currThruster;
		currStrafe = input.currStrafe;
		currRudder = input.currRudder;
		bIsBoosting = input.bIsBoosting;

		if(!bIsBoosting && currBoostTime < tankStats.maxBoostTime)
		{
			currBoostTime += Time.deltaTime;
		} else if (bIsBoosting && currBoostTime >= 0f)
		{
			currBoostTime -= Time.deltaTime;
		}
	}

	public float getCurrBoostTime()
	{
		return currBoostTime;
	}
}
