using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TX130 : MonoBehaviour {

	private Rigidbody rb;

	public TX130Stats tankStats;
	// TODO remove variables and put in tankStats
	[Header("Thrust Settings")]
	public float thrustForce = 6000f;
	public float strafeForce = 20f;
	public float rudderForce = 2000f;
	public float slowingVelFactor = .99f;
	public float angleOfRoll = 5f;

	[Header("HoverSettings")]
	public float hoverHeight = 2f;
	public float maxGroundDist = 5f;
	public float hoverForce = 300f;
	public LayerMask whatIsGround;
	public PIDController hoverPID;
	public Transform[] hoverPoints;

	[Header("Physics Settings")]
	public Transform shipBody;
	public float hoverGravity = 20f;
	public float fallGravity = 80f;

	private float speed;
	private float drag;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		speed = Vector3.Dot(rb.velocity, transform.forward);
	}
	public void CalculateHover(float currStrafe, float currRudder)
	{
		RaycastHit hitInfo;

		foreach (Transform hoverPoint in hoverPoints)
		{
			Ray ray = new Ray(hoverPoint.position, -Vector3.up);
			if (Physics.Raycast(ray,
					   out hitInfo,
					 maxGroundDist,
					 whatIsGround))
			{
				print("Grounded...");
				float height = hitInfo.distance;
				float forcePercent = hoverPID.Seek(hoverHeight, height);
				Vector3 force = Vector3.up * hoverForce * forcePercent;
				Vector3 gravity = -Vector3.up * hoverGravity * height;
				rb.AddForceAtPosition(force, hoverPoint.position);
				rb.AddForceAtPosition(gravity, hoverPoint.position);
			}
			else
			{
				print("Not grounded...");
				Vector3 gravity = -Vector3.up * fallGravity;
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
			angle = angleOfRoll * -currRudder;
		}
		// If not turning
		else if (currRudder == 0)
		{
			angle = angleOfRoll * -currStrafe;
		}
		// If both
		else
		{
			angle = angleOfRoll * -currStrafe; // only strafe because strafe affects body roll more than rudder torque
		}

		//float angle = angleOfRoll * -input.currRudder;

		// Calcualte the rotation for new angle
		Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0, 0, angle);
		// Apply rotation
		shipBody.rotation = Quaternion.Lerp(shipBody.rotation, bodyRotation, Time.deltaTime * 10f);
	}

	public void CalculatePropulsion(float currThrust, float currStrafe, float currRudder)
	{
		// Torque
		// Calculate the yaw torque based on rudder and current angular velocity
		float rotationTorque = rudderForce * currRudder - rb.angularVelocity.y;
		// Apply torque
		rb.AddRelativeTorque(0, rotationTorque, 0);

		// Thrust

		// If not propelling, slow ship
		if (currThrust <= 0f || currStrafe <= 0f)
		{
			rb.velocity *= slowingVelFactor;
		}

		float propulsion = thrustForce * currThrust;
		rb.AddForce(transform.forward * propulsion);
		float sidePropulsion = strafeForce * currStrafe;
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

	
	//// Hover variables
	//private int layerMask;
	//private Transform hoverPointsParent;
	//private Transform[] hoverPoints;

	//// Use this for initialization
	//void Start () {
	//	rb = GetComponent<Rigidbody>();

	//	layerMask = 1 << LayerMask.NameToLayer("Player");
	//	layerMask = ~layerMask;

	//	// Set hoverpoints
	//	hoverPointsParent = transform.Find("HoverPoints");
	//	hoverPoints = hoverPointsParent.GetComponentsInChildren<Transform>();
	//}

	//// For Physics
	//private void FixedUpdate()
	//{
	//	// Hover
	//	RaycastHit hit;
	//	for (int i = 0; i < hoverPoints.Length; i++)
	//	{
	//		Transform hoverPoint = hoverPoints[i];
	//		if (Physics.Raycast(hoverPoint.position,
	//							-Vector3.up, out hit,
	//							tankStats.hoverHeight, 
	//							layerMask))
	//		{
	//			print("Ground beneath");
	//			rb.AddForceAtPosition(Vector3.up * tankStats.hoverForce * (1f - (hit.distance / tankStats.hoverHeight)), hoverPoint.position);
	//		}
	//		else
	//		{
	//			print("No ground");
	//			if (transform.position.y > hoverPoint.position.y)
	//			{
	//				rb.AddForceAtPosition(hoverPoint.up * tankStats.hoverForce, hoverPoint.position);
	//			}
	//			else
	//			{
	//				rb.AddForceAtPosition(hoverPoint.up * -tankStats.hoverForce, hoverPoint.position);
	//			}
	//		}
	//	}
	//}

	//// Public method for child classes to access
	//public void ApplyForces(float currThrust, bool bIsBoosting, float currBoostTime, float currStrafe, Vector3 currTurn)
	//{
	//	// Forward
	//	if (Mathf.Abs(currThrust) > 0)
	//	{
	//		rb.AddForce(transform.forward * currThrust);
	//	}

	//	// Boost
	//	if (bIsBoosting && currBoostTime >= 0f)
	//	{
	//		//rb.AddForce(transform.forward * currThrust);
	//		//currBoostTime -= Time.deltaTime; // TODO possibly find way to modify child variable
	//	}

	//	// Strafe
	//	if (Mathf.Abs(currStrafe) > 0)
	//	{
	//		rb.AddForce(transform.right * currStrafe);
	//	}

	//	// Turn
	//	if (currTurn.y > 0)
	//	{
	//		rb.AddRelativeTorque(currTurn * tankStats.turnStrength);
	//	}
	//	else if (currTurn.y < 0)
	//	{
	//		rb.AddRelativeTorque(currTurn * tankStats.turnStrength);
	//	}
	//}
}
