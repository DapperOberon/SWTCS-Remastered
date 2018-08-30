using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TX130))]
public class TX130AI : TX130
{

	//public float deadZone = 0.1f;
	//private float currThrust = 0f;
	//private bool bIsBoosting = false;
	//private float currBoostTime = 0f;
	//private float currStrafe = 0f;
	//private float currTurn = 0f;

	//float angleDiff;
	//Vector3 crossProd;

	//private TX130 tankRef;
	//private GameObject playerRef;
	//public float radius = 10f;

	//// Use this for initialization
	//void Start()
	//{
	//	currBoostTime = base.tankStats.maxBoostTime;

	//	// Get tank reference
	//	tankRef = GetComponent<TX130>();

	//	// Get player reference
	//	playerRef = GameObject.FindGameObjectWithTag("Player");
	//}

	//// Update is called once per frame
	//void Update()
	//{
	//	GetMoveCommand();
	//}

	//private void FixedUpdate()
	//{
	//	// Using tank ref apply forces
	//	tankRef.ApplyForces(currThrust, bIsBoosting, currBoostTime, currStrafe, currTurn * crossProd * angleDiff);
	//}

	//public float getCurrBoostTime()
	//{
	//	return currBoostTime;
	//}

	//private void GetMoveCommand()
	//{
	//	//float distance = Vector3.Distance(transform.position, playerRef.transform.position);
	//	//var direction = transform.position - playerRef.transform.position;
	//	//direction.

	//	//// Main Thrust
	//	//if (distance > radius)
	//	//{
	//	//	print("Moving foward to player");
	//	//	currThrust = 1 * TX130.forwardAcl;
	//	//}
	//	//else
	//	//{
	//	//	if (tankRef.rb.velocity.magnitude > deadZone)
	//	//	{
	//	//		print("AI stopping...");
	//	//		currThrust = -1 * TX130.backwardAcl;
	//	//	}
	//	//	else
	//	//	{
	//	//		print("AI stopped.");
	//	//	}
	//	//}

	//	//// Main Thrust
	//	//float aclAxis = Input.GetAxis("Vertical");
	//	//if (aclAxis > deadZone)
	//	//{
	//	//	currThrust = aclAxis * TX130.forwardAcl;
	//	//}
	//	//else if (aclAxis < deadZone)
	//	//{
	//	//	currThrust = aclAxis * TX130.backwardAcl;
	//	//}
	//	//else
	//	//{
	//	//	currThrust = 0;
	//	//}

	//	// Boost Thrust
	//	//if (!bIsBoosting && currBoostTime < TX130.maxBoostTime)
	//	//{
	//	//	currBoostTime += Time.deltaTime;
	//	//}
	//	//else if (bIsBoosting && currBoostTime >= 0f)
	//	//{
	//	//	currBoostTime -= Time.deltaTime;
	//	//}

	//	//bool boostInput = Input.GetButton("Boost");
	//	//if (boostInput)
	//	//{
	//	//	bIsBoosting = true;
	//	//	currThrust = TX130.boostAcl;
	//	//}
	//	//else
	//	//{
	//	//	bIsBoosting = false;
	//	//}

	//	// Strafe Thrust
	//	//float strafeAxis = Input.GetAxis("Strafe");
	//	//if (Mathf.Abs(strafeAxis) > deadZone)
	//	//{
	//	//	currStrafe = strafeAxis * TX130.strafeAcl;

	//	//}
	//	//else
	//	//{
	//	//	currStrafe = 0;
	//	//}

	//	////Turning
	//	//float turnAxis = Input.GetAxis("Horizontal");
	//	//if (Mathf.Abs(turnAxis) > deadZone)
	//	//{
	//	//	currTurn = turnAxis;
	//	//}
	//	//else
	//	//{
	//	//	currTurn = 0;
	//	//}

	//	// d.toliaferro
	//	// Turning
	//	Vector3 targetDelta = playerRef.transform.position - transform.position;

	//	// get the angle between transofmr.forward and target delta
	//	angleDiff = Vector3.Angle(transform.forward, targetDelta);

	//	// get its cross product, which is the axis of rotation to get from one vector to the other
	//	crossProd = Vector3.Cross(transform.forward, targetDelta);

	//	// apply torque along that axis according to the magnitude of the angle.
	//	if(Mathf.Abs(angleDiff) > deadZone)
	//	{
	//		currTurn = base.tankStats.turnStrength;
	//	}
	//}
}
