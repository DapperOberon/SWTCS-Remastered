using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour {

	public static Audio Instance = null;

	public AudioMixerGroup musicGroup;
	public AudioMixerGroup sfxGroup;
	public AudioMixerGroup speechGroup;

	private const string sMusicVolume = "MusicVolume";
	private const string sSFXVolume = "SFXVolume";
	private const string sSpeechVolume = "SpeechVolume";

	private void Awake()
	{
		//Check if instance already exists
		if (Instance == null)
		{
			//if not, set instance to this
			Instance = this;
		}
		//If instance already exists and it's not this:
		else if (Instance != this)
		{
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);
		}
	}

	private void OnGUI()
	{
		musicGroup.audioMixer.SetFloat(sMusicVolume, PlayerPrefsManager.Instance.MusicVolume);
		sfxGroup.audioMixer.SetFloat(sSFXVolume, PlayerPrefsManager.Instance.SFXVolume);
		speechGroup.audioMixer.SetFloat(sSpeechVolume, PlayerPrefsManager.Instance.SpeechVolume);
	}
}
