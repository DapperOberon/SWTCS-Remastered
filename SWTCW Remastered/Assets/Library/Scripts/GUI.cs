using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public Slider boostBar;
	private TX130Player player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<TX130Player>();
	}
	
	// Update is called once per frame
	void Update () {
		boostBar.value = player.getCurrBoostTime();
	}
}
