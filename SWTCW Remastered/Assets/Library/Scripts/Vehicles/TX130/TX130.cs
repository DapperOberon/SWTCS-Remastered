using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TX130 : MonoBehaviour {

	public TX130Stats tankStats;

	[Header("Hover Settings")]
	public LayerMask whatIsGround;
	public PIDController hoverPID;
	public Transform[] hoverPoints;

	[Header("Cosmetic Settings")]
	public Transform shipBody;

	private Rigidbody rb;


	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void CalculateHover(float currStrafe, float currRudder)
	{
		RaycastHit hitInfo;

		foreach (Transform hoverPoint in hoverPoints)
		{
			Ray ray = new Ray(hoverPoint.position, -Vector3.up);
			if (Physics.Raycast(ray,
					   out hitInfo,
					 tankStats.maxGroundDist,
					 whatIsGround))
			{
				print("Grounded...");
				float height = hitInfo.distance;
				float forcePercent = hoverPID.Seek(tankStats.hoverHeight, height);
				Vector3 force = Vector3.up * tankStats.hoverForce * forcePercent;
				Vector3 gravity = -Vector3.up * tankStats.hoverGravity * height;
				rb.AddForceAtPosition(force, hoverPoint.position);
				rb.AddForceAtPosition(gravity, hoverPoint.position);
			}
			else
			{
				print("Not grounded...");
				Vector3 gravity = -Vector3.up * tankStats.fallGravity;
				rb.AddForceAtPosition(gravity, hoverPoint.position);
			}
		}

		// Purely cosmetic

		//Calculate the amount of pitch and roll based on the ground using projection
		Vector3 projection = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
		Quaternion rotation = Quaternion.LookRotation(projection, Vector3.up);

		// Move ship to match round rotation
		rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.deltaTime * 10f));

		// Calculate angle to turn based on rudder
		float angle = 0;

		// If not strafeing
		if (currStrafe == 0)
		{
			angle = tankStats.angleOfRoll * -currRudder;
		}
		// If not turning
		else if (currRudder == 0)
		{
			angle = tankStats.angleOfRoll * -currStrafe;
		}
		// If both
		else
		{
			angle = tankStats.angleOfRoll * -currStrafe; // only strafe because strafe affects body roll more than rudder torque
		}

		//float angle = angleOfRoll * -input.currRudder;

		// Calcualte the rotation for new angle
		Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0, 0, angle);
		// Apply rotation
		shipBody.rotation = Quaternion.Lerp(shipBody.rotation, bodyRotation, Time.deltaTime * 10f);
	}

	public void CalculatePropulsion(float currThrust, float currStrafe, float currRudder, bool bIsBoosting)
	{
		// Torque
		// Calculate the yaw torque based on rudder and current angular velocity
		float rotationTorque = tankStats.turnStrength * currRudder - rb.angularVelocity.y;
		// Apply torque
		rb.AddRelativeTorque(0, rotationTorque, 0);

		// Thrust

		// If not propelling, slow ship
		if (currThrust <= 0f || currStrafe <= 0f)
		{
			rb.velocity *= tankStats.slowingVelFactor;
		}

		float propulsion;

		propulsion = tankStats.forwardForce * currThrust;

		if (bIsBoosting)
		{
			propulsion *= tankStats.boostMultiplier;
		}

		rb.AddForce(transform.forward * propulsion);
		float sidePropulsion = tankStats.strafeForce * currStrafe;
		rb.AddForce(transform.right * sidePropulsion);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		foreach (Transform hoverPoint in hoverPoints)
		{
			Ray ray = new Ray(hoverPoint.position, -Vector3.up);
			Gizmos.DrawRay(ray);
		}
	}
}
