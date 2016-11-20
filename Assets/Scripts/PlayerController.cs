using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	PlayerController: 																	*/
/*																						*/
/*			Functions:																	*/
/*					Start()																*/
/*					LoseComposure()														*/
/*					GainComposure()														*/
/*					LowerSelfImage()													*/
/*					InitiateConversation()												*/
/*					GoToRestroom()														*/
/*					PlayWithFood()														*/
/*					EatFood()															*/
/*					Update()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
public class PlayerController : MonoBehaviour 
{

	//	Public Variables
	public bool isTalking;
	public float playerComposure;
	public float playerSelfImage;
	public Slider playerComposureBar;
	public Slider playerSelfImageBar;
	public GameObject[] friends;

	//	Private Variables
	[SerializeField] private float m_StartingComposure;
	[SerializeField] private float m_StartingSelfImage;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void Start () 
	{
		m_StartingComposure = 10f;
		m_StartingSelfImage = 10f;
		playerComposure = m_StartingComposure;
		playerSelfImage = m_StartingSelfImage;

		//	These sliders are for debugging purposes only. They will not be in the release version of the game.
		playerComposureBar = GameObject.Find ("Composure Slider").GetComponent<Slider> ();
		playerSelfImageBar = GameObject.Find ("Self Image Slider").GetComponent<Slider> ();

		friends = GameObject.FindGameObjectsWithTag ("Friend");
	
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LoseComposure: 																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void LoseComposure()
	{
		playerComposure--;
		playerComposureBar.value = playerComposure;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GainComposure: 																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void GainComposure()
	{
		playerComposure++;
		playerComposureBar.value = playerComposure;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LowerSelfImage: 																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void LowerSelfImage()
	{
		playerSelfImage--;
		playerSelfImageBar.value = playerSelfImage;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	InitiateConversation:																*/
/*																						*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void InitiateConversation()
	{
		//	click spam stuff goes here
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GoToRestroom: 																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void GoToRestroom()
	{
		//	the pause screen
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	PlayWithFood: 																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void PlayWithFood()
	{
		//	Lowers suspicion initally but after x number of times raises suspicion
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	EatFood: 																			*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void EatFood()
	{
		for (int i = 0; i < friends.Length; i++)
		{
			friends [i].SendMessage ("LowerSuspicion");
		}
		LowerSelfImage ();
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame														*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
	
	}
}
