using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LeaveDinner: Connected to UI button on phone										*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			PlayerIsDone ()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class LeaveDinner : MonoBehaviour 
{
	//	Public Variables
	public GameManager gm;		//	Reference to Game Manager

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () {
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	PlayerIsDone: Initiates LeaveDinner() in GameManager.cs								*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void PlayerIsDone()
	{
		gm.SendMessage("LeaveDinner");
	}
}
