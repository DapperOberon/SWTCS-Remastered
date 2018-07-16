using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public Animator cam;
	public GameObject startText;
	public GameObject mainMenu;

	private void OnGUI()
	{
		if (Input.GetButtonDown("Start"))
		{
			StartCoroutine(TransitionToMainMenu());
		}
	}

	IEnumerator TransitionToMainMenu()
	{
		cam.Play("ZoomOutFromStart");
		yield return new WaitForSeconds(cam.GetCurrentAnimatorStateInfo(0).length);
		startText.SetActive(false);
		mainMenu.SetActive(true);
	}
}
