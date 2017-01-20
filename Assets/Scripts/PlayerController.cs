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
/*																						*/
/*--------------------------------------------------------------------------------------*/	
public class PlayerController : MonoBehaviour 
{

	//	Public Variables							
	public bool isTalking;									//	Indicates if player is talking (Might use this)
	public float playerComposure;							//	Reference to player's current composure level
	public float playerSelfImage;							//	Reference to player's current self image
	public Slider playerComposureBar;						//	Reference to player's composure bar [Debugging purposes]
	public Slider playerSelfImageBar;						//	Reference to player's self image bar [Debuggins purposes]
	public GameObject[] friends;							//	Reference to all friend in the scene

	//	Private Variables
	[SerializeField] private float m_StartingComposure;		//	Starting composure level
	[SerializeField] private float m_StartingSelfImage;		// Starting self image level

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void Start () 
	{
		//	Sets starting composure and self image levels
		m_StartingComposure = 3f;
		m_StartingSelfImage = 5f;
		playerComposure = m_StartingComposure;
		playerSelfImage = m_StartingSelfImage;

		//	These sliders are for debugging purposes only. They will not be in the release version of the game.
		playerComposureBar = GameObject.Find ("Composure Slider").GetComponent<Slider> ();
		playerSelfImageBar = GameObject.Find ("Self Image Slider").GetComponent<Slider> ();

		//	Finds friends and puts them in an array
		friends = GameObject.FindGameObjectsWithTag ("Friend");
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LoseComposure: 																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	public void LoseComposure()
	{
		playerComposure--;
		if (playerComposure < 0)
		{
			LowerSelfImage ();
			playerComposure = 0;
		}
		playerComposureBar.value = playerComposure;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GainComposure: 																		*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	public void GainComposure()
	{
		playerComposure++;
		playerComposureBar.value = playerComposure;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LowerSelfImage: 																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	public void LowerSelfImage()
	{
		playerSelfImage--;
		playerSelfImageBar.value = playerSelfImage;
	}
}
