using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	BlurController: Executes the blur effect when playing with food						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			EatingFood (float seconds)													*/
/*			BlurScreen ()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class BlurController : MonoBehaviour {

	public bool blurNow;				//	If true blurs the screen
	public float blurTimer;				//	How many seconds the screen blurs for
	public PlayerController player;		//	Reference to player controller
	private Animator m_Animator;		//	Reference to GameObjects animator

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		m_Animator = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	EatingFood: Disables input for x seconds and blurs screen							*/
	/*		param: float seconds - how many seconds the function waits for 					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator EatingFood(float seconds)
	{
		blurNow = true;
		m_Animator.SetBool ("LosingComposure", blurNow);
		yield return new WaitForSeconds (seconds);
		blurNow = false;
		m_Animator.SetBool ("LosingComposure", blurNow);

	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	BlurScreen: Blurs screen based on player's composure								*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void BlurScreen()
	{
		blurTimer = 0.5f / player.playerComposure;
		StartCoroutine (EatingFood (blurTimer));
	}
}
