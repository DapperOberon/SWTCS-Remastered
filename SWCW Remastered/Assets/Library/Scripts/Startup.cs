using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour {

	public float timeTillNextLevelLoad;
	
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(timeTillNextLevelLoad);
		Scenes.Instance.LoadNextLevel();
	}
}
