using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scene2 : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			SceneManager.LoadScene (2);

		}

	}

}
