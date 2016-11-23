using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * 
 * 		TODO:	Come up with name of game
 * 				Find better variable names and be consistent with them
 * 				Implement some visual fx
 * 
 */ 

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	TextboxManager: 																	*/
/*																						*/
/*			Functions:																	*/
/*					Start()																*/
/*					DisableInputTime()													*/
/*					SteerConversationTimer()											*/
/*					InitiateConversation(FirendController friend)						*/
/*					Update()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class TextboxManager : MonoBehaviour 
{

	public bool disableInput;							//	Bool to disable ALL input after click event
	public bool steerConversation;						//	Disables progressing through conversation text when in click event
	public float comfortLevelReference;					//	Reference to player's comfort level
	public bool[] conversationPasses;					//	Reference to conversationCheck in FriendController script
	public GameObject conversation;						//	Reference to the Conversation Script
	public GameObject panel;							//	Reference to Panel
	public UnityEngine.UI.Text theText;					//	Reference to text in text box
	public TextAsset textFile;							//	The file where the dialogue comes from
	public string[] textLines;							//	Lines of text in the textFile

	public int currentLine;								//	Line of text being displayed
	public int endAtLine;								//	Last line of text file

	public PlayerController player;						//	reference to the player

	private Vector3 m_Hide;								//	Hides conversation steering slider
	private Vector3 m_Appear;							//	Makes conversation steering slider appear
	private FriendController m_FriendReference;			//	Reference to the friend the player is talking to

	// Use this for initialization
	void Start ()
	{
		//	We can take in input
		disableInput = false;

		//	Counts the sliders events in each conversation
		conversationPasses = new bool[] { false, false, false, false };

		//	Sets reference to conversation steering slider
		conversation = GameObject.Find ("Conversation Slider");

		//	Sets reference to comfort level
		comfortLevelReference = conversation.GetComponent<Conversation> ().comfortLevel;

		m_Hide = new Vector3 (0, 0, 0);
		m_Appear = new Vector3 (0.6f, 0.6f, 0.6f);

		//	Hides conversation steering slider until necessary
		conversation.transform.localScale = m_Hide;

		//	Sets reference to player
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		//	Sets reference to text
		theText = GameObject.FindGameObjectWithTag("Text").GetComponent<UnityEngine.UI.Text> ();

		//	Loads blank file to avoid null reference error since a conversation hasn't started yet
		textFile = Resources.Load ("blank") as TextAsset;
		if (textFile != null)
		{
			//	Splits text file at the "Enter" key also known as \n
			textLines = (textFile.text.Split ('\n'));
		}

		if (endAtLine == 0)
		{
			//	Last line is length - 1
			endAtLine = textLines.Length - 1;
		}
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	DisableInputTime: Disables input for x seconds										*/
/*		param: float seconds - how many seconds the function waits for 					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	IEnumerator DiableInputTimer(float seconds)
	{
		disableInput = true;
		yield return new WaitForSeconds (seconds);
		disableInput = false;

	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	SteerConversationTimer: Player steers conversation for x seconds					*/
/*		param: float seconds - how many seconds the function waits for 					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	IEnumerator SteerConversationTimer(float seconds)
	{
		
		//	Makes the player keep click until the number of seconds is done
		yield return new WaitForSeconds (seconds);

		//	Checks if conversation steerer reached 0 [This can change easily]
		if (conversation.GetComponent<Conversation>().conversationSteerer.value == 0)
		{
			//	If the conversatin takes a bad turn a friend's suspicion is raised
			m_FriendReference.RaiseSuspicion ();

			//	We are done with the conversation steering slider so we hide it
			conversation.transform.localScale = m_Hide;

			//	We no longer want the slider to move
			conversation.GetComponent<Conversation>().startSlider = false;

			//	We are no longer attempting to steer a conversation
			steerConversation = false;

			//	Sets a new starting comfort level in the case of a failure
			conversation.GetComponent<Conversation> ().SetComfortlevel (0.5f);

			//	These are put in place to make the movement from the failure state appear seemless
			if (conversationPasses[2])
			{
				currentLine = 32;
			}
			else if (conversationPasses[1])
			{
				currentLine = 24;
			}
			else if (conversationPasses[0])
			{
				currentLine = 10;
			}
		}

		/*-------------------This happens if we succeed---------------------------------*/

		//	We are done with the conversation steering slider so we hide it
		conversation.transform.localScale = m_Hide;

		//	We no longer want the slider to move
		conversation.GetComponent<Conversation>().startSlider = false;

		//	Rewards the player for perfroming well. The next conversation will be 50% easier based on how they perfromed
		comfortLevelReference = comfortLevelReference * 1.5f;

		//	We are no longer attempting to steer a conversation
		steerConversation = false;
	}
		
/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LoadAndRunConversationWith: Navigates through conversations and handles the			*/
/*								conversation's states									*/
/*		param: FriendController friend - Lets us know which friend and what 			*/
/*										 suspcion level they have						*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	public void LoadAndRunConversationWith(FriendController friend)
	{
		//	Sets our friend reference incase we need it outside this function
		m_FriendReference = friend;

		//	All conversation stuff for GianCarlo goes in here
		if (friend.friendName.Equals ("GC")) 
		{
			//	We check GC's suspciion level and load the appropriate file and split each line at the neter key "\n"
			if (friend.conversationCheck[3] && friend.suspicionLevel == 3) 
			{
				textFile = Resources.Load ("GC_SusLvl4") as TextAsset;

				textLines = (textFile.text.Split ('\n'));
				endAtLine = textLines.Length - 1;

				/*
				 * 		Logic for suspicion level 4 goes here
				 */
			}
			else if (friend.conversationCheck[2] && friend.suspicionLevel == 2)
			{
				textFile = Resources.Load ("GC_SusLvl3") as TextAsset;

				textLines = (textFile.text.Split ('\n'));
				endAtLine = textLines.Length - 1;

				/*
				 * 		Logic for suspicion level 3 goes here
				 */
			}
			else if (friend.conversationCheck[1] && friend.suspicionLevel == 1)
			{
				textFile = Resources.Load ("GC_SusLvl2") as TextAsset;

				textLines = (textFile.text.Split ('\n'));
				endAtLine = textLines.Length - 1;

				/*
				 * 		Logic for suspicion level 2 goes here
				 */
			}
			else if (friend.conversationCheck[0])
			{
				/*---------------Logic for Suspicion level 1 of GC------------------*/

				//If we've gone through this conversation with raising suspicion we skip all the if statements
				if (!conversationPasses [3]) 
				{
					//	Loads the file
					textFile = Resources.Load ("GC_SusLvl1") as TextAsset;

					//	At line 7 which is a "..." we start the conversation steering slider
					if (currentLine == 7) 
					{
						//	We've passed conversation steering part of the conversation
						conversationPasses [0] = true;

						//	We are now steering a conversation. Make the slider appear and make it move
						conversation.transform.localScale = m_Appear;
						steerConversation = true;
						conversation.GetComponent<Conversation> ().startSlider = true;

						//	Start the timer
						StartCoroutine (SteerConversationTimer (3f));
					}

					//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
					if (currentLine == 8)
					{
						StartCoroutine (DiableInputTimer (1.5f));
					}

					//	This is here to make the dialouge flow smoothly after failure and success states
					if (currentLine == 9) 
					{
						currentLine = 12;
					}
					if (currentLine == 11)
					{
						currentLine = 12;
					}

					//	At line 21 which is a "..." we start the conversation steering slider
					if (currentLine == 21) 
					{
						//	We've passed conversation steering part of the conversation
						conversationPasses [1] = true;

						//	We are now steering a conversation. Make the slider appear and make it move
						conversation.transform.localScale = m_Appear;
						steerConversation = true;
						conversation.GetComponent<Conversation> ().startSlider = true;

						//	Start the timer
						StartCoroutine (SteerConversationTimer (3f));
					}

					//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
					if (currentLine == 22)
					{
						StartCoroutine (DiableInputTimer (1.5f));
					}

					//	This is here to make the dialouge flow smoothly after failure and success states
					if (currentLine == 23) 
					{
						currentLine = 26;
					}
					if (currentLine == 25) 
					{
						currentLine = 26;
					}

					//	At line 29 which is a "..." we start the conversation steering slider
					if (currentLine == 29) 
					{
						//	We've passed conversation steering part of the conversation
						conversationPasses [2] = true;

						//	We are now steering a conversation. Make the slider appear and make it move
						conversation.transform.localScale = m_Appear;
						steerConversation = true;
						conversation.GetComponent<Conversation> ().startSlider = true;

						//	Start the timer
						StartCoroutine (SteerConversationTimer (3f));
					}

					//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
					if (currentLine == 30)
					{
						StartCoroutine (DiableInputTimer (1.5f));
					}

					//	This is here to make the dialouge flow smoothly after failure and success states
					if (currentLine == 31)
					{
						currentLine = 34;
					}
					if (currentLine == 33) 
					{
						currentLine = 34;
					}
						
					//	We have passed through this conversation text file. The last line will now appear
					if (currentLine == 41) 
					{
						conversationPasses [3] = true;
						currentLine = 41;
					}

					//	Hides the pannel
					if (currentLine == 42)
					{
						panel.SetActive (false);
					}
				}
				else
				{
					//	Makes panel appear with the "hold on I'm getting a text line"
					panel.SetActive (true);
				}

				//Sets the appropriate length for each text file
				textLines = (textFile.text.Split ('\n'));
				endAtLine = textLines.Length - 1;
			}
		}
	}


/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame 														*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void Update()
	{
		//	Displays text on current line
		theText.text = textLines [currentLine];

		//	If input is disabled we cannot click forwards
		if (!disableInput) 
		{
			//	Each click when we are not steering a conversation advances the dialouge
			if (Input.GetMouseButtonDown (0) && !steerConversation) 
			{
				currentLine++;
				//	Useed to make sure we don't any Index out of bounds errors
				if (currentLine > textLines.Length - 1) 
				{
					currentLine = textLines.Length - 1;
				}

			}
			//	If we've reached the end of the text file we display the "Last line" we want to see
			if (currentLine == textLines.Length - 1) 
			{
				panel.SetActive (false);
				currentLine = textLines.Length - 2;
			}
		}
	}
}
