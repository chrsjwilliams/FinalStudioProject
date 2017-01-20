using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	RestroomHandler: Connects Toggle Restroom to UI										*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			ToggleRestroom ()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class RestroomHandler : MonoBehaviour {

	public GameManager gm;					//	Reference to GameManager
	public PlayerController player;			//	Referecne to player
	public TextboxManager textboxManager;	//	Reference to TextboxManager

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		textboxManager = GameObject.FindGameObjectWithTag ("TextManager").GetComponent<TextboxManager> ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	ToggleRestroom: Toggles Restroom and connects functionality to UI					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void ToggleRestroom()
	{
		if (!player.isTalking) 
		{
			gm.ToggleRestroom ();
			player.GainComposure ();
			if (gm.restroomVisitCount > 3 && !gm.visitingRestroom) 
			{
				textboxManager.RestroomDialouge ();
				gm.m_Friends [Random.Range(0, 3) % 3].GetComponent<FriendController> ().RaiseSuspicion ();
			}
		}
	}
}
