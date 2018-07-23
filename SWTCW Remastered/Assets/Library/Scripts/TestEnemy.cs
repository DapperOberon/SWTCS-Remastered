using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour {

	public float health;
	private AutoTarget autoTarget;

	private void Start()
	{
		autoTarget = FindObjectOfType<AutoTarget>();
	}

	private void Update()
	{
		if(health <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		if (autoTarget.targets.Contains(this.gameObject))
		{
			autoTarget.ClearSelection();
			autoTarget.targets.Remove(this.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("AllyBullets"))
		{
			float damage = collision.gameObject.GetComponent<WeaponDamage>().GetDamage();
			health -= damage;
		}
	}
}
