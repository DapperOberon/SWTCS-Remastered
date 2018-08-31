using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour {

	public float maxHealth;
	private float health;
	private AutoTarget[] autoTargets;

	private void Start()
	{
		health = maxHealth;
		autoTargets = FindObjectsOfType<AutoTarget>();
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
		foreach(AutoTarget autoTarget in autoTargets)
		{
			if (autoTarget.targets.Contains(this.gameObject))
			{
				autoTarget.ClearSelection();
				autoTarget.targets.Remove(this.gameObject);
			}
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

	public float GetScaledHealth()
	{
		return health / maxHealth;
	}
}
