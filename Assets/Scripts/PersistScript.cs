using UnityEngine;
using System.Collections;

public class PersistScript : MonoBehaviour {

	private static PersistScript persistScript;

	void Awake () {
		if (persistScript == null) {
			DontDestroyOnLoad (this);
			persistScript = this;
		} else {
			Destroy (this.gameObject);
		}
	}
	
	void Update () {
	
	}
}
