using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed;

	[Header("Drive Settings")]
	public float driveForce = 100f;
	public float strafeForce = 20f;
	public float slowingVelFactor = .99f;
	public float brakingVelFactor = .95f;
	public float angleOfRoll = 10f;

	[Header("Hover Settings")]
	public float hoverHeight = 1.5f;
	public float maxGroundDist = 5f;
	public float hoverForce = 300f;
	public LayerMask whatIsGround;
	public PIDController hoverPID;
	public Transform[] hoverPoints;

	[Header("Physics Settings")]
	public Transform shipBody;
	public float terminalVelocity = 100f;
	public float hoverGravity = 20f;
	public float fallGravity = 80f;

	Rigidbody rb;
	PlayerInput input;
	float drag;
	bool isOnGround;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		input = GetComponent<PlayerInput>();

		// Calculate the ship's drag value
		drag = driveForce / terminalVelocity;

		//// Setup layermask
		//whatIsGround = 1 << LayerMask.NameToLayer("Player");
		//whatIsGround = ~whatIsGround;
	}

	private void FixedUpdate()
	{
		// Calculate the current speed by using dot product. Tells how much of ship's velocity is in the "forward" direction.
		speed = Vector3.Dot(rb.velocity, transform.forward);

		// Calculate forces
		CalculateHover();
		CalculatePropulsion();
	}

	private void CalculateHover()
	{
		// Ground NOrmal
		Vector3 centerGroundNormal = Vector3.zero;

		// Calculate a ray that points straight down from the ship
		Ray centerRay = new Ray(transform.position, -transform.up);

		// Declare a variable that will hold result of raycast
		RaycastHit centerHitInfo;

		// Determine if ship is on ground
		isOnGround = Physics.Raycast(centerRay, out centerHitInfo, maxGroundDist, whatIsGround);

		if (isOnGround)
		{
			print("grounded");
			centerGroundNormal = centerHitInfo.normal.normalized;

			// Determine how high off ground
			float height = centerHitInfo.distance;
			// Save normal of ground
			centerGroundNormal = centerHitInfo.normal.normalized;
			// Use PID to determine hover force
			float forcePercent = hoverPID.Seek(hoverHeight, height);

			// Calculate total hover force
			Vector3 force = centerGroundNormal * hoverForce * forcePercent;
			// Calcualte force direction of gravity to make it adhere to ground
			Vector3 gravity = -centerGroundNormal * hoverGravity * height;

			// Calulate hover force and gravity
			rb.AddForce(force, ForceMode.Acceleration);
			rb.AddForce(gravity, ForceMode.Acceleration);
		}
		else
		{
			print("not grounded");
			centerGroundNormal = Vector3.up;

			// Calculate stronger gravity
			Vector3 gravity = -centerGroundNormal * fallGravity;
			rb.AddForce(gravity, ForceMode.Acceleration);
		}
		// Puperly cosmetic

		//Calculate the amount of pitch and roll based on the ground using projection
		Vector3 projection = Vector3.ProjectOnPlane(transform.forward, centerGroundNormal);
		Quaternion rotation = Quaternion.LookRotation(projection, centerGroundNormal);

		// Move ship to match round rotation
		rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.deltaTime * 10f));

		// Calculate angle to turn based on rudder
		float angle = 0;

		// If not strafing
		if (input.currStrafe == 0)
		{
			 angle = angleOfRoll * -input.currRudder;
		}
		// If not turning
		else if (input.currRudder == 0)
		{
			angle = angleOfRoll * -input.currStrafe;
		}
		// If both
		else
		{
			angle = angleOfRoll * -input.currRudder * input.currStrafe;
		}

		//float angle = angleOfRoll * -input.currRudder;

		// Calcualte the rotation for new angle
		Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0, 0, angle);
		// Apply rotation
		shipBody.rotation = Quaternion.Lerp(shipBody.rotation, bodyRotation, Time.deltaTime * 10f);
	}

	private void CalculatePropulsion()
	{
		// Calculate the yaw torque based on rudder and current angular velocity
		float rotationTorque = input.currRudder - rb.angularVelocity.y;
		// Apply torque
		rb.AddRelativeTorque(0, rotationTorque, 0, ForceMode.VelocityChange);

		//// Calculate the current sideways speed by using dot which tells us how much is left or right
		//float sidewaysSpeed = Vector3.Dot(rb.velocity, transform.right);

		////Cacluate desired friction to side of vehicle which causes it not to drift to the side
		//// Divide fixedDeltaTime to add drift
		//Vector3 sideFriction = -transform.right * (sidewaysSpeed / Time.fixedDeltaTime);

		//// Apply friction
		//rb.AddForce(sideFriction, ForceMode.Acceleration);

		// If not propelling, slow ship
		if(input.currThruster <= 0f || input.currStrafe <= 0f)
		{
			rb.velocity *= slowingVelFactor;
		}

		// Braking requires to be on ground
		if (!isOnGround)
			return;

		// Everything from here on is only if on ground

		//Calculate and apply the amount of propulsion force by multiplying the drive force by the amount of applied thruster and subtracting the drag amount
		float propulsion = driveForce * input.currThruster - drag * Mathf.Clamp(speed, 0, terminalVelocity);
		rb.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
		float sidePropulsion = strafeForce * input.currStrafe - drag * Mathf.Clamp(speed, 0, terminalVelocity);
		rb.AddForce(transform.right * sidePropulsion, ForceMode.Acceleration);
	}
}
