using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public Animator cam;
	public GameObject startText;
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject credits;
	private bool bCreditsRolling;

	private void OnGUI()
	{
		if (Input.GetButtonDown("Start"))
		{
			StartCoroutine(TransitionToMainMenu());
		}

		if(bCreditsRolling && (Input.GetButtonDown("Cancel")))
		{
			StopCredits();
		}
	}

	IEnumerator TransitionToMainMenu()
	{
		cam.Play("ZoomOutFromStart");
		yield return new WaitForSeconds(cam.GetCurrentAnimatorStateInfo(0).length);
		startText.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void RollCredits(bool choice)
	{
		bCreditsRolling = choice;
		Cursor.visible = false;
	}

	public void StopCredits()
	{
		Cursor.visible = true;
		credits.SetActive(false);
		optionsMenu.SetActive(true);
		bCreditsRolling = false;
	}
}
