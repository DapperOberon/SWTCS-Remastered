using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TX130 : MonoBehaviour {

	private Rigidbody rb;

	// Thrust variables
	protected const float forwardAcl = 8000f;
	protected const float backwardAcl = 4000f;
	protected const float boostAcl = 8500f;
	// Max boost time in seconds
	protected const float maxBoostTime = 10f;
	protected const float strafeAcl = 4000f;
	protected const float turnStrength = 4000f;

	// Hover variables
	private int layerMask;
	private const float hoverForce = 500f;
	// Hover height in meters
	private const float hoverHeight = 2f;
	private Transform hoverPointsParent;
	private Transform[] hoverPoints;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

		layerMask = 1 << LayerMask.NameToLayer("Player");
		layerMask = ~layerMask;

		// Set hoverpoints
		hoverPointsParent = transform.Find("HoverPoints");
		hoverPoints = hoverPointsParent.GetComponentsInChildren<Transform>();
	}

	// For Physics
	private void FixedUpdate()
	{
		// Hover
		RaycastHit hit;
		for(int i = 0; i < hoverPoints.Length; i++)
		{
			Transform hoverPoint = hoverPoints[i];
			if(Physics.Raycast(hoverPoint.position,
								-Vector3.up, out hit,
								hoverHeight,
								layerMask))
			{
				rb.AddForceAtPosition(Vector3.up * hoverForce * (1f - (hit.distance / hoverHeight)), hoverPoint.position);
			} else
			{
				if(transform.position.y > hoverPoint.position.y)
				{
					rb.AddForceAtPosition(hoverPoint.up * hoverForce, hoverPoint.position);
				} else
				{
					rb.AddForceAtPosition(hoverPoint.up * -hoverForce, hoverPoint.position);
				}
			}
		}
	}

	// Public method for child classes to access
	public void ApplyForces(float currThrust, bool bIsBoosting, float currBoostTime, float currStrafe, float currTurn)
	{
		// Forward
		if (Mathf.Abs(currThrust) > 0)
		{
			rb.AddForce(transform.forward * currThrust);
		}

		// Boost
		if (bIsBoosting && currBoostTime >= 0f)
		{
			rb.AddForce(transform.forward * currThrust);
			//currBoostTime -= Time.deltaTime; // TODO possibly find way to modify child variable
		}

		// Strafe
		if (Mathf.Abs(currStrafe) > 0)
		{
			rb.AddForce(transform.right * currStrafe);
		}

		// Turn
		if (currTurn > 0)
		{
			rb.AddRelativeTorque(Vector3.up * currTurn * turnStrength);
		}
		else if (currTurn < 0)
		{
			rb.AddRelativeTorque(Vector3.up * currTurn * turnStrength);
		}
	}
}
