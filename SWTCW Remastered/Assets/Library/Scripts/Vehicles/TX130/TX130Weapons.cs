using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TX130Weapons : MonoBehaviour {

	public AudioSource gunAudio;
	public float laserPitchRand;
	public float gunAccuracy;
	public float fireRate;
	public float fireVelocity;
	public float timeToDestroy;
	public float gunRecenterSpeed = 1f;
	public GameObject ammo;
	public Transform ammoParent;
	private Rigidbody ammorb;
	public Transform[] guns;
	public Transform[] gunPoints;
	public Transform aimPoint;

	private AutoTarget targeting;
	private float fireCountdown;

	private void Start()
	{
		targeting = GetComponent<AutoTarget>();
		
	}

	// Update is called once per frame
	void Update () {
		
		foreach (Transform gun in guns)
		{
			if (targeting.GetSelectedObj() != null)
			{
				//gunAccuracyV3 = new Vector3(Random.Range(-gunAccuracy, gunAccuracy), Random.Range(-gunAccuracy, gunAccuracy), Random.Range(-gunAccuracy, gunAccuracy));
				Quaternion targetRot = Quaternion.LookRotation(targeting.GetSelectedObj().GetComponent<Renderer>().bounds.center - gun.position);

				//gun.LookAt(targeting.GetSelectedObj().transform);
				gun.rotation = Quaternion.RotateTowards(gun.rotation, targetRot, gunRecenterSpeed);
			}
			else
			{
				gun.rotation = Quaternion.RotateTowards(gun.rotation, aimPoint.rotation, gunRecenterSpeed);
			}
		}

		if(fireCountdown <= 0f && Input.GetButton("Fire1"))
		{
			Fire();
			fireCountdown = 1f / fireRate;
		}

		fireCountdown -= Time.deltaTime;
	}

	public void Fire()
	{
		for (int i = 0; i < gunPoints.Length; i++)
		{
			Debug.Log("Firing from " + gunPoints[i]);
			GameObject a = Instantiate(ammo, gunPoints[i].position, gunPoints[i].rotation);
			a.transform.parent = ammoParent;
			a.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(-gunAccuracy, gunAccuracy), Random.Range(-gunAccuracy, gunAccuracy), fireVelocity));
			Destroy(a, timeToDestroy);
		}
		gunAudio.pitch = Random.Range(1 - laserPitchRand, 1 + laserPitchRand);
		gunAudio.Play();
	}
}
