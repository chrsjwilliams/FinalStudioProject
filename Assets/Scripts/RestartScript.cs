using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class RestartScript : MonoBehaviour
{
	public void StartGame()
	{
		// "Stage1" is the name of the first scene we created.
		Application.LoadLevel("First");
	}
}