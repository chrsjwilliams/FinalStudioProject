using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Food: Controls playing with food													*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			PlayingWithFood (float seconds)												*/
/*			PlayWithFood ()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class Food : MonoBehaviour {

	//	Public Variables
	public bool playingWithFood;				//	If this is true, player is playing with food
	public int timesPlayedWithFood;				//	Stores how many times player played with food
	public TextboxManager textboxManager;		//	Reference to the textboxManager
	public GameManager gm;						//	Reference to Game Manager
	public PlayerController player;				//	Reference to the player
	public Animator m_Animatior;				//	Reference to food's animator


	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		playingWithFood = false;
		m_Animatior = GetComponent<Animator> ();
		timesPlayedWithFood = 0;
		textboxManager = GameObject.FindGameObjectWithTag ("TextManager").GetComponent<TextboxManager> ();
	}


	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	PlayingWithFood: Disables input for x seconds										*/
	/*		param: float seconds - how many seconds the function waits for 					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator PlayingWithFood(float seconds)
	{
		playingWithFood = true;
		m_Animatior.SetBool ("PlayingWithFood", playingWithFood);
		yield return new WaitForSeconds (seconds);
		playingWithFood = false;
		m_Animatior.SetBool ("PlayingWithFood", playingWithFood);
		timesPlayedWithFood++;

	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	PLayWithFood: Raises suspicion if done too many times								*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void PlayWithFood()
	{
		StartCoroutine (PlayingWithFood (0.5f));

		if (timesPlayedWithFood > 3)
		{
			textboxManager.NotEating ();
			gm.m_Friends [Random.Range(0,3) % 3].GetComponent<FriendController> ().RaiseSuspicion ();
		}
		else
		{
			gm.m_Friends [Random.Range (0, 3) % 3].GetComponent<FriendController> ().LowerSuspicion ();
		}
	}
}
