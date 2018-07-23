using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour
{
	//// Static constructor useful for version checking
	//static Prefs()
	//{
	//	if (Version != "1.5")
	//	{
	//		// Invoke upgrade process if necessary.
	//	}
	//}

	//<summary>
	// Gets the version.
	//</summary>
	//<value>Theversion.</value>
	

	public static PlayerPrefsManager Instance = null;

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

	public string Version
	{
		get { return PlayerPrefs.GetString("version", "1.0"); }
	}
	
	public float MusicVolume
	{
		get { return PlayerPrefs.GetFloat(sMusicVolume, 0); }
		set { PlayerPrefs.SetFloat(sMusicVolume, value); }
	}

	public float SFXVolume
	{
		get { return PlayerPrefs.GetFloat(sSFXVolume, 0); }
		set { PlayerPrefs.SetFloat(sSFXVolume, value); }
	}

	public float SpeechVolume
	{
		get { return PlayerPrefs.GetFloat(sSpeechVolume, 0); }
		set { PlayerPrefs.SetFloat(sSpeechVolume, value); }
	}
	public void ResetAudio()
	{
		MusicVolume = 0;
		SFXVolume = 0;
		SpeechVolume = 0;
	}

	// Example of int pref
	//public int LastScore
	//{
	//	get { return PlayerPrefs.GetInt(sLastScore, 0); }
	//	set { PlayerPrefs.SetInt(sLastScore, value); }
	//}

	// Example of bool pref
	//public bool MusicEnabled
	//{
	//	get { return PlayerPrefs.GetInt("music enabled", 1) != 0; }
	//	set { PlayerPrefs.SetInt("music enabled", value ? 1 : 0); }
	//}
}
