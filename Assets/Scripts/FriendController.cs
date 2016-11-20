using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	FriendController: 																	*/
/*																						*/
/*			Functions:																	*/
/*					Start()																*/
/*					RaiseSuspicion()													*/
/*					LowerSuspicion()													*/
/*					InitiateConversation(GameObject person)								*/
/*					Update()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/

public class FriendController : MonoBehaviour 
{
	//	Public Variables
	public bool isTalking;
	public float suspicionLevel;
	public Slider suspicionBar;
	public GameObject player;

	//	Private Variables
	[SerializeField] private float m_SuspicionLevel;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void Start () 
	{
		suspicionBar = GameObject.Find ("Suspicion Level").GetComponent<Slider> ();
		m_SuspicionLevel = 3;
		suspicionLevel = m_SuspicionLevel;
		suspicionBar.value = suspicionLevel;

		player = GameObject.FindGameObjectWithTag ("Player");
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	RaiseSuspicion:																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void RaiseSuspicion()
	{
		m_SuspicionLevel++;
		suspicionLevel = m_SuspicionLevel;
		suspicionBar.value = suspicionLevel;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LowerSuspicion:																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void LowerSuspicion()
	{
		m_SuspicionLevel--;
		suspicionLevel = m_SuspicionLevel;
		suspicionBar.value = suspicionLevel;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	InitiateConversation:																*/
/*					param:	GameObject person											*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void InitiateConversation(GameObject person)
	{
		//	Initiates conversation with other people as long as they are not already talking
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame														*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void Update () 
	{
	
	}
}
