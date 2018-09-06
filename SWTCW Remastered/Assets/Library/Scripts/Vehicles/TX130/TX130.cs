using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TX130 : MonoBehaviour {

	public TX130Stats tankStats;

	[Header("Hover Settings")]
	public LayerMask whatIsGround;
	public PIDController hoverPID;
	public float hoverLookahead;
	public float hoverLookaheadMax;
	public float lookaheadLerpSpeed;
	public float lookaheadSpeed;
	public float maxLookaheadSpeed;
	public Transform hoverParent;
	public Transform[] hoverPoints;
	private bool[] hoverPointsGrounded = new bool[4];

	[Header("Cosmetic Settings")]
	public Transform shipBody;

	private Rigidbody rb;
	private Vector3 hoverParentVector;
	private Vector3 hoverParentVectorMax;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		hoverParentVector = new Vector3(0, 0, hoverLookahead);
		hoverParentVectorMax = new Vector3(0, 0, hoverLookaheadMax);
	}

	private void Update()
	{
		for(int i = 0; i < hoverPointsGrounded.Length; i++)
		{

		}

		if (bIsGrounded(hoverPointsGrounded))
		{
			if (rb.velocity.magnitude >= lookaheadSpeed && rb.velocity.magnitude <= maxLookaheadSpeed)
			{
				//hoverParent.localPosition = hoverParentVector;
				hoverParent.localPosition = Vector3.Lerp(hoverParent.localPosition, hoverParentVector, lookaheadLerpSpeed);
			}
			else if (rb.velocity.magnitude >= maxLookaheadSpeed)
			{
				hoverParent.localPosition = Vector3.Lerp(hoverParent.localPosition, hoverParentVectorMax, lookaheadLerpSpeed);
			}
			else
			{
				hoverParent.localPosition = Vector3.zero;
			}
		}
		else
		{
			hoverParent.localPosition = Vector3.zero;
		}
		
	}

	bool bIsGrounded(bool[] hoverPointsGrounded)
	{
		for (int i = 0; i < hoverPointsGrounded.Length; i++)
		{
			if (hoverPointsGrounded[i])
			{
				return hoverPointsGrounded[i];
			}
		}
		return false;
	}

	public void CalculateHover(float currStrafe, float currRudder)
	{
		RaycastHit hitInfo;

		for(int i = 0; i < hoverPoints.Length; i++)
		{
			Ray ray = new Ray(hoverPoints[i].position, -Vector3.up);
			if (Physics.Raycast(ray,
					    out hitInfo,
		    tankStats.maxGroundDist,
					  whatIsGround))
			{
				print("Grounded...");
				hoverPointsGrounded[i] = true;
				float height = hitInfo.distance;
				Vector3 normal = hitInfo.normal.normalized;
				float forcePercent = hoverPID.Seek(tankStats.hoverHeight, height);
				//Vector3 force = Vector3.up * tankStats.hoverForce * forcePercent;
				//Vector3 gravity = -Vector3.up * tankStats.hoverGravity * height;
				Vector3 force = normal * tankStats.hoverForce * forcePercent;
				Vector3 gravity = -Vector3.up * tankStats.hoverGravity * height;
				rb.AddForceAtPosition(force, hoverPoints[i].position);
				rb.AddForceAtPosition(gravity, hoverPoints[i].position);
			}
			else
			{
				print("Not grounded...");
				hoverPointsGrounded[i] = false;
				Vector3 gravity = -Vector3.up * tankStats.fallGravity;
				rb.AddForceAtPosition(gravity, hoverPoints[i].position);
			}
		}
		//foreach (Transform hoverPoint in hoverPoints)
		//{
		//	Ray ray = new Ray(hoverPoint.position, -Vector3.up);
		//	if (Physics.Raycast(ray,
		//			   out hitInfo,
		//			 tankStats.maxGroundDist,
		//			 whatIsGround))
		//	{
		//		print("Grounded...");
		//		float height = hitInfo.distance;
		//		Vector3 normal = hitInfo.normal.normalized;
		//		float forcePercent = hoverPID.Seek(tankStats.hoverHeight, height);
		//		//Vector3 force = Vector3.up * tankStats.hoverForce * forcePercent;
		//		//Vector3 gravity = -Vector3.up * tankStats.hoverGravity * height;
		//		Vector3 force = normal * tankStats.hoverForce * forcePercent;
		//		Vector3 gravity = -Vector3.up * tankStats.hoverGravity * height;
		//		rb.AddForceAtPosition(force, hoverPoint.position);
		//		rb.AddForceAtPosition(gravity, hoverPoint.position);
		//	}
		//	else
		//	{
		//		print("Not grounded...");
		//		Vector3 gravity = -Vector3.up * tankStats.fallGravity;
		//		rb.AddForceAtPosition(gravity, hoverPoint.position);
		//	}
		//}

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
