using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	FriendController: This houses the central logic for the 3 friends.					*/
/*					  This will be attached to 3 UI Button Objects (friends)			*/
/*																						*/
/*			Functions:																	*/
/*					Start()																*/
/*					RaiseSuspicion()													*/
/*					LowerSuspicion()													*/
/*					InitiateConversation()												*/
/*					Update()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class FriendController : MonoBehaviour 
{
	//	Public Variables
	public string friendName;								//	Stores string containing Friend's name
	public float suspicionLevel;							//	Public reference to friend's suspicion level
	public bool [] conversationCheck;						//	Logic gates to prevent the same conversation happening twice
	public TextboxManager textbox;							//	Reference to the Textbox Manager (This is where the conversation is)
	public CanvasGroup textpanel;							//	Reference to canvas group to hide textbox when not in use
	public Slider suspicionBar;								//	Suspicion bar for debug purposes
	public GameObject player;								//	Friend's reference to the player

	//	Private Variables
	[SerializeField] private float m_SuspicionLevel;		//	Stores the friend's suspicion level

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void Start () 
	{
		//	Ready to do the first conversation
		conversationCheck = new bool[] { true, false, false, false, false };

		//	Sets reference to Textbox Manager
		textbox = GameObject.Find ("Textbox Manager").GetComponent<TextboxManager>();

		//	Sets reference to text panel
		textpanel = textbox.panel.GetComponent<CanvasGroup> ();

		//	Hides text panel
		textpanel.alpha = 0f;

		//	Makes sure player cannot interact with text panel
		textpanel.blocksRaycasts = false;

		//	Find slider for debug purposes
		suspicionBar = GameObject.Find ("Suspicion Level").GetComponent<Slider> ();

		//	Sets suspicion to 0. This is suspicion level 1
		m_SuspicionLevel = 0;
		suspicionLevel = m_SuspicionLevel;
		suspicionBar.value = suspicionLevel;

		//	Sets refercen to player
		player = GameObject.FindGameObjectWithTag ("Player");
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	RaiseSuspicion:	Raises friend's suspicion (0 to 3)	level 1 to 	4					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	public void RaiseSuspicion()
	{
		m_SuspicionLevel++;
		suspicionLevel = m_SuspicionLevel;
		suspicionBar.value = suspicionLevel;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LowerSuspicion:	Raises friend's suspicion (3 to 0)	level 4 to 	1					*/
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
/*	InitiateConversation: When clicked on initiates conversation logic in 				*/
/* 						  textbox manager												*/
/*																						*/
/*																						*/
/*--------------------------------------------------------------------------------------*/	
	void InitiateConversation()
	{
		//	Loads file appropriate for this friend's name and susupicion level
		textbox.LoadAndRunConversationWith (this);

		//	Makes text panel visible
		textpanel.alpha = 1f;
		textpanel.blocksRaycasts = true;

	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame (Will be used for visual fx)							*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void Update () 
	{
	
	}
}
