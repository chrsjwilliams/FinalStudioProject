using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///*--------------------------------------------------------------------------------------*/
///*																						*/
///*	Conversation:	 																	*/
///*																						*/
///*			Functions:																	*/
///*					Start()																*/
///*					SetComfortLevel(float newComfortLevel)								*/
///*					Update()															*/
///*					FixedUpdate()														*/
///*																						*/
///*					Why use both Update and FixedUpdate?								*/
///*																						*/
///*					This is the easiest way I thought to implement the opposite			*/
///*					movements that change at different time intervals.					*/																	*/
///*																						*/
///*--------------------------------------------------------------------------------------*/
public class Conversation : MonoBehaviour 
{
	//	Public Variables
	public bool startSlider;						//	Bool to let us know when to start the slider
	public float decrement;							//	The value to decrement the slider by
	public float increment;							//	The value to increment the slider by
	public float comfortLevel;						//	Reference to current level of the slider
	public Slider conversationSteerer;				//	Reference to the Conversation slider
	public PlayerController player;					//	Reference to the player

	//	Private Variables
	[SerializeField] private float m_ComfortLevel;	//	Current value of the slider

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void Start () 
	{
		//	SLider does not start unless called
		startSlider = false;

		//	Sets values
		decrement = 0.15f;
		increment = 0.07f;
		m_ComfortLevel = 0.8f;

		//	Sets reference to slider
		conversationSteerer = GetComponent<Slider> ();

		//	Sets reference to player
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();

		//	Sets comfort level
		comfortLevel = m_ComfortLevel;
		conversationSteerer.value = comfortLevel;
	}


/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	SetComfortLevel: Sets value on Conversation steerer slider							*/
/*				param: float newComfortLevel: sets the value to this					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	public void SetComfortlevel(float newComfortLevel)
	{
		m_ComfortLevel = newComfortLevel;
		comfortLevel = m_ComfortLevel;
		conversationSteerer.value = comfortLevel;
	}
		
/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame														*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void Update()
	{
		//	Each click increments our ability to steer a conversation
		if (startSlider && Input.GetMouseButtonDown(0))
		{
			m_ComfortLevel = m_ComfortLevel + increment;
			comfortLevel = m_ComfortLevel;
			conversationSteerer.value = comfortLevel;
		}
	}
	
/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	FixedUpdate: Called once per fixed amount											*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void FixedUpdate ()
	{
		if (conversationSteerer.value < 0) 
		{
			conversationSteerer.value = 0;
		}

		if (conversationSteerer.value > 1) 
		{
			conversationSteerer.value = 1;
		}
			
		if (startSlider) 
		{
			//	The lower the composer the more difficult it is to steer a conversation
			if (player.playerComposure > 4) 
			{
				m_ComfortLevel = m_ComfortLevel - decrement;
				comfortLevel = m_ComfortLevel;
				conversationSteerer.value = comfortLevel;
			} 
			else if (player.playerComposure > 3) 
			{
				m_ComfortLevel = m_ComfortLevel - (decrement * 1.1f);
				comfortLevel = m_ComfortLevel;
				conversationSteerer.value = comfortLevel;
			} 
			else if (player.playerComposure > 2) 
			{
				m_ComfortLevel = m_ComfortLevel - (decrement * 1.3f);
				comfortLevel = m_ComfortLevel;
				conversationSteerer.value = comfortLevel;
			} 
			else
			{
				m_ComfortLevel = m_ComfortLevel - (decrement * 1.5f);
				comfortLevel = m_ComfortLevel;
				conversationSteerer.value = comfortLevel;
			}
		}
	}
}
