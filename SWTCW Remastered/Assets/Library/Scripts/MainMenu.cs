using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Animator cam;
	public GameObject startText;
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public Slider[] audioSliders;
	public GameObject credits;
	public GameObject loadLevelScreen;
	public Slider levelProgressSldr;
	private bool bCreditsRolling;
	

	private void Start()
	{
		audioSliders[0].value = PlayerPrefsManager.Instance.MusicVolume;
		audioSliders[1].value = PlayerPrefsManager.Instance.SFXVolume;
	}

	public void ResetAudioSliders()
	{

		foreach (Slider slider in audioSliders)
		{
			slider.value = 0;
		}
	}

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

	public void LoadCampaign()
	{
		StartCoroutine(LoadNextLevelAsync());
	}

	IEnumerator LoadNextLevelAsync()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

		loadLevelScreen.SetActive(true);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			levelProgressSldr.value = progress;
			Debug.Log(progress);
			yield return null;
		}
	}
}
