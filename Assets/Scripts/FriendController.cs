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
	public bool talkingToPlayer;							//	Keeps friend highlihgted
	public string friendName;								//	Stores string containing Friend's name
	public float suspicionLevel;							//	Public reference to friend's suspicion level
	public bool [] conversationCheck;						//	Logic gates to prevent the same conversation happening twice
	public TextboxManager textbox;							//	Reference to the Textbox Manager (This is where the conversation is)
	public CanvasGroup textpanel;							//	Reference to canvas group to hide textbox when not in use
	public Slider suspicionBar;								//	Suspicion bar for debug purposes
	public Button button;									//	Reference to friend's button
	public ColorBlock buttonColor;							//	Highlights the friend being spoken to
	public Color friendIndicator;							//	The color of the highlighted friend
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
		talkingToPlayer = false;

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

		friendIndicator = new Color (0.65f, 0.93f, 1.0f);

		button = GetComponent<Button> ();




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
	public void LowerSuspicion()
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
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame (Will be used for visual fx)							*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if (talkingToPlayer)
		{
			buttonColor = button.colors;
			buttonColor.normalColor = friendIndicator;
			button.colors = buttonColor;
		}
	}
}
