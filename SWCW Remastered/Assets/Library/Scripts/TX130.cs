using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TX130 : MonoBehaviour {

	private Rigidbody rb;
	private float deadZone = 0.1f;

	public float forwardAcl = 100f;
	public float backwardAcl = 25f;
	private float currThrust = 0f;

	public float boostAcl = 2f;
	public bool bIsBoosting = false; // TODO make private
	public float maxBoostTime = 10f;
	[HideInInspector]
	public float currBoostTime = 0f;

	public float strafeAcl = 50f;
	private float currStrafe = 0f;

	public float turnStrength = 10f;
	float currTurn = 0f;

	private int layerMask;
	public float hoverForce = 9f;
	public float hoverHeight = 2f;
	public GameObject[] hoverPoints;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

		layerMask = 1 << LayerMask.NameToLayer("Player");
		layerMask = ~layerMask;

		currBoostTime = maxBoostTime;
	}

	// Update is called once per frame
	private void Update() {
		// Main Thrust
		float aclAxis = Input.GetAxis("Vertical");
		if (aclAxis > deadZone)
		{
			currThrust = aclAxis * forwardAcl;
		} else if (aclAxis < deadZone)
		{
			currThrust = aclAxis * backwardAcl;
		} else
		{
			currThrust = 0;
		}

		// Boost Thrust
		if(!bIsBoosting && currBoostTime < maxBoostTime)
		{
			currBoostTime += Time.deltaTime;
		}

		bool boostInput = Input.GetButton("Boost");
		if (boostInput)
		{
			bIsBoosting = true;
			currThrust = boostAcl;
		}
		else
		{
			bIsBoosting = false;
		}

		// Strafe Thrust
		float strafeAxis = Input.GetAxis("Strafe");
		if(Mathf.Abs(strafeAxis) > deadZone)
		{
			currStrafe = strafeAxis * strafeAcl;

		} else
		{
			currStrafe = 0;
		}

		// Turning
		float turnAxis = Input.GetAxis("Horizontal");
		if(Mathf.Abs(turnAxis) > deadZone)
		{
			currTurn = turnAxis;
		} else
		{
			currTurn = 0;
		}
	}

	// For Physics
	private void FixedUpdate()
	{
		// Hover
		RaycastHit hit;
		for(int i = 0; i < hoverPoints.Length; i++)
		{
			GameObject hoverPoint = hoverPoints[i];
			if(Physics.Raycast(hoverPoint.transform.position,
								-Vector3.up, out hit,
								hoverHeight,
								layerMask))
			{
				rb.AddForceAtPosition(Vector3.up * hoverForce * (1f - (hit.distance / hoverHeight)), hoverPoint.transform.position);
			} else
			{
				if(transform.position.y > hoverPoint.transform.position.y)
				{
					rb.AddForceAtPosition(hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
				} else
				{
					rb.AddForceAtPosition(hoverPoint.transform.up * -hoverForce, hoverPoint.transform.position);
				}
			}
		}


		// Forward
		if(Mathf.Abs(currThrust) > 0)
		{
			rb.AddForce(transform.forward * currThrust);
		}

		// Boost
		if (bIsBoosting && currBoostTime >= 0f)
		{
			rb.AddForce(transform.forward * currThrust);
			currBoostTime -= Time.deltaTime;
		}

		// Strafe
		if (Mathf.Abs(currStrafe) > 0)
		{
			rb.AddForce(transform.right * currStrafe);
		}

		// Turn
		if (currTurn > 0){
			rb.AddRelativeTorque(Vector3.up * currTurn * turnStrength);
		} else if (currTurn < 0)
		{
			rb.AddRelativeTorque(Vector3.up * currTurn * turnStrength);
		}
	}
}
