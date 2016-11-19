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
/*					GoToRestroom()														*/
/*					PlayWithFood()														*/
/*					EatFood()															*/
/*					Update()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	

public class PlayerController : MonoBehaviour 
{

	//	Public Variables
	public float playerComposure;
	public float playerSelfImage;
	public Slider playerComposureBar;
	public Slider playerSelfImageBar;

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
/*	GoToRestroom: 																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void GoToRestroom()
	{
		
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	PlayWithFood: 																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void PlayWithFood()
	{

	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	EatFood: 																			*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void EatFood()
	{
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
