using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	PlateController: Houses functionality of plate										*/
/*		Functions:																		*/
/*			Awake ()																	*/
/*			Start ()																	*/
/*			EatFood ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class PlateController : MonoBehaviour 
{
	//	Public Variables
	public AudioClip chew;							//	Audio clip for chewing
	public Image foodImage;							//	Image of the food
	public PlayerController player;					//	Reference to player

	//	Private Variables
	private AudioSource m_AudioSource;				//	AudioSource for plate

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Awake: Runs once at the begining of the game before Start (). Initalizes variables.	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Awake ()
	{
		m_AudioSource = GetComponent<AudioSource> ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		foodImage = GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	EatFood: Functionality of eating food												*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void EatFood()
	{
		player.LowerSelfImage ();
		Color newAlpha = new Color (foodImage.color.r, foodImage.color.g, foodImage.color.b, foodImage.color.a * 0.85f);
		m_AudioSource.PlayOneShot (chew, 0.5f);
		foodImage.color = newAlpha;

	}
}
