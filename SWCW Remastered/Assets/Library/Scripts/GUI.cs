using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public Slider boostBar;
	private TX130 tank;

	// Use this for initialization
	void Start () {
		tank = FindObjectOfType<TX130>();
	}
	
	// Update is called once per frame
	void Update () {
		boostBar.value = tank.currBoostTime;
	}
}
