using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * 
 * 		TODO:	Make food script. PLay with food function and eat food function	
 * 				Paritcle effect when certain words at sus lvl 3 and 4 appear
 * 				Bathroom via phone button (put the few lines of code to make it work)
 * 				Implement some visual fx
 * 				After dessert one last stop at the restroom
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
	public bool conversationIsOver;						//	Allows player to talk to other friends when the current conversation is done
	public bool disableInput;							//	Bool to disable ALL input after click event
	public bool steerConversation;						//	Disables progressing through conversation text when in click event
	public float comfortLevelReference;					//	Reference to player's comfort level
	public bool[] conversationPasses;					//	Reference to conversationCheck in FriendController script
	public GameObject conversation;						//	Reference to the Conversation Script
	public GameObject panel;							//	Reference to Panel
	public UnityEngine.UI.Text theText;					//	Reference to text in text box
	public TextAsset textFile;							//	The file where the dialogue comes from
	public string talkingTo;							//	Who the player character is currently talking to
	public string currentTextLine;						//	Current line in a string
	private string currentWord = "";					//	Current word on currentTextLine
	public string[] textLines;							//	Lines of text in the textFile
	public float letterPause = 0.075f;					//	Pause to draw each letter
	public int currentLine;								//	Line of text being displayed
	public int endAtLine;								//	Last line of text file
	public CanvasGroup textpanel;							//	Reference to canvas group to hide textbox when not in use
	public PlayerController player;						//	reference to the player

	private Vector3 m_Hide;								//	Hides conversation steering slider
	private Vector3 m_Appear;							//	Makes conversation steering slider appear
	private FriendController m_FriendReference;			//	Reference to the friend the player is talking to
	private GameManager m_GameManager;								//	Reference to the Game Manager

	// Use this for initialization
	void Start ()
	{
		//	We can take in input
		disableInput = false;

		//	No conversations have started
		conversationIsOver = true;

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
		textpanel = panel.GetComponent<CanvasGroup> ();
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

		// Currently talking to no one
		talkingTo = "";

		//	Makes text panel visible
		textpanel.alpha = 0f;
		textpanel.blocksRaycasts = false;

		m_GameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager>();
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	DisableInputTime: Disables input for x seconds										*/
/*		param: float seconds - how many seconds the function waits for 					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	IEnumerator DiableInputTimer(float seconds)
	{
		if (m_FriendReference != null) 
		{
			m_FriendReference.GetComponent<Button> ().enabled = false;
		}
		disableInput = true;
		yield return new WaitForSeconds (seconds);
		if (m_FriendReference != null) 
		{
			m_FriendReference.GetComponent<Button> ().enabled = true;
		}
		disableInput = false;

	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	EndConversation: hides text panel at the end of the conversation					*/
/*		param: float seconds - how many seconds the function waits for 					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	IEnumerator EndConversation(float seconds)
	{
		yield return new WaitForSeconds (seconds);
		conversationIsOver = true;
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

		// Currently talking to no one
		talkingTo = "";

		//	Makes text panel visible
		player.isTalking = false;
		textpanel.alpha = 0f;
		textpanel.blocksRaycasts = false;
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	TypeText: Types texts word by word													*/
/*		param: string compareWord - the current line of text							*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	private IEnumerator TypeText (string compareWord)
	{
		//	Splits the line into words at the ' ' character
		string[] displayThis = compareWord.Split (' ');

		//	Disables Input to give players time to read each line and prevent them from just skipping through
		if (compareWord.Equals("…"))
		{
			StartCoroutine (DiableInputTimer(0.4f));
		}
		else
		{
			StartCoroutine (DiableInputTimer(1.75f));
		}

		for(int i = 0; i < displayThis.Length; i++)
		{
			//	If words are the same leave the IEnumerator
			if (currentTextLine != compareWord)
				break;
			
			//	Adds current word to the text line
			currentWord = " " + displayThis [i];

			//	Displays text on screen
			theText.text = theText.text + currentWord;

			//	Pause between each word
			yield return new WaitForSeconds (letterPause);
			//yield return new WaitForSeconds(letterPause * Random.Range(0.5f, 2f));
		}  
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	SteerConversationTimer: Player steers conversation for x seconds					*/
/*		param: float seconds - how many seconds the function waits for 					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	IEnumerator SteerConversationTimer(float seconds , int thisLine)
	{
		
		//	Makes the player keep click until the number of seconds is done
		yield return new WaitForSeconds (seconds);

		//	Checks if conversation steerer reached 0 [This can change easily]
		if (conversation.GetComponent<Conversation>().conversationSteerer.value == 0)
		{
			//	If the conversatin takes a bad turn a friend's suspicion is raised
			m_FriendReference.RaiseSuspicion ();

			player.LoseComposure ();

			//	We are done with the conversation steering slider so we hide it
			conversation.transform.localScale = m_Hide;

			//	We no longer want the slider to move
			conversation.GetComponent<Conversation>().startSlider = false;

			//	We are no longer attempting to steer a conversation
			steerConversation = false;

			//	Sets a new starting comfort level in the case of a failure
			conversation.GetComponent<Conversation> ().SetComfortlevel (0.5f);

			//	These are put in place to make the movement from the failure state appear seemless
			currentLine = thisLine + 3;
			
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

		Resources.UnloadAsset (textFile);
		friend.talkingToPlayer = true;
		player.isTalking = true;

		textpanel.alpha = 1f;
		textpanel.blocksRaycasts = true;

		//	Sets our friend reference incase we need it outside this function
		m_FriendReference = friend;

		//	All conversation stuff for GianCarlo goes in here
		if (friend.friendName.Equals ("GC") && (conversationIsOver || talkingTo.Equals("GC"))) // Make a talking to 
		{
			GianCarloConversation (ref friend);
		}
		if (friend.friendName.Equals ("Candace") && (conversationIsOver || talkingTo.Equals("Candace")))
		{
			CandaceConversation (ref friend);
		}
		if (friend.friendName.Equals ("Mike") && (conversationIsOver || talkingTo.Equals("Mike")))
		{
			MikeConversation (ref friend);
		}

		//	We started a conversation
		conversationIsOver = false;

		//	Emptys the text panel after each block of text
		theText.text = "";

		//	Displays text on the panel
		currentTextLine = textLines [currentLine];
		//	Seprates text line word by word
		StartCoroutine (TypeText (currentTextLine));
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	CandaceConversation: Navigates through conversations and handles the				*/
/*								conversation's states									*/
/*		param: FriendController friend - Lets us know which friend and what 			*/
/*										 suspcion level they have						*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	void GianCarloConversation(ref FriendController friend)
	{
		talkingTo = friend.friendName;
		//	We check GC's suspciion level and load the appropriate file and split each line at the neter key "\n"
		if (friend.conversationCheck[3] && friend.suspicionLevel == 3) 
		{
			textFile = Resources.Load ("GC_SusLvl4") as TextAsset;

			if (!conversationPasses [3]) 
			{
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 9) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 10)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 12) 
				{
					currentLine = 14;
				}
				if (currentLine == 15)
				{
					currentLine = 14;
				}

				//	At line 21 which is a "..." we start the conversation steering slider
				if (currentLine == 20) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [1] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 21)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 22) 
				{
					currentLine = 25;
				}
				if (currentLine == 24) 
				{
					currentLine = 25;
				}

				//	At line 29 which is a "..." we start the conversation steering slider
				if (currentLine == 28) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 29)
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 30)
				{
					currentLine = 33;
				}
				if (currentLine == 32) 
				{
					currentLine = 33;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
		}
		else if (friend.conversationCheck[2] && friend.suspicionLevel == 2)
		{
			textFile = Resources.Load ("GC_SusLvl3") as TextAsset;

			if (!conversationPasses [3]) 
			{
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 9) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 10)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 12) 
				{
					currentLine = 15;
				}
				if (currentLine == 14)
				{
					currentLine = 15;
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 22)
				{
					StartCoroutine (DiableInputTimer (5f));
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
				if (currentLine == 30) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 31)
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 32)
				{
					currentLine = 35;
				}
				if (currentLine == 36) 
				{
					currentLine = 35;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}

			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
		}
		else if (friend.conversationCheck[1] && friend.suspicionLevel == 1)
		{
			textFile = Resources.Load ("GC_SusLvl2") as TextAsset;
			if (!conversationPasses [3]) 
			{
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 8) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;
				
					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;
				
					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}
				
				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 9)
				{
					StartCoroutine (DiableInputTimer (5f));
				}
				
				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 10) 
				{
					currentLine = 13;
				}
				if (currentLine == 12)
				{
					currentLine = 13;
				}
				
				//	At line 21 which is a "..." we start the conversation steering slider
				if (currentLine == 19) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [1] = true;
				
					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;
				
					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}
				
				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 20)
				{
					StartCoroutine (DiableInputTimer (5f));
				}
				
				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 21) 
				{
					currentLine = 24;
				}
				if (currentLine == 23) 
				{
					currentLine = 24;
				}
				
				//	At line 29 which is a "..." we start the conversation steering slider
				if (currentLine == 27) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;
				
					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}
				
				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 28)
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}
				
				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 29)
				{
					currentLine = 32;
				}
				if (currentLine == 31) 
				{
					currentLine = 32;
				}
				
				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}
			
				textLines = (textFile.text.Split ('\n'));
				endAtLine = textLines.Length - 1;
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 8)
				{
					StartCoroutine (DiableInputTimer (5f));
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 22)
				{
					StartCoroutine (DiableInputTimer (5f));
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
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
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}

			//Sets the appropriate length for each text file
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
		}
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	CandaceConversation: Navigates through conversations and handles the				*/
/*								conversation's states									*/
/*		param: ref FriendController friend - Lets us know which friend and what 		*/
/*										 suspcion level they have						*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	void CandaceConversation(ref FriendController friend)
	{
		talkingTo = friend.friendName;
		//	We check GC's suspciion level and load the appropriate file and split each line at the neter key "\n"
		if (friend.conversationCheck [3] && friend.suspicionLevel == 3) 
		{
			textFile = Resources.Load ("CN_SusLvl4") as TextAsset;

			if (!conversationPasses [3]) 
			{
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 9) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 10)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 11) 
				{
					currentLine = 14;
				}
				if (currentLine == 13)
				{
					currentLine = 14;
				}

				//	At line 21 which is a "..." we start the conversation steering slider
				if (currentLine == 20) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [1] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 21)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 22) 
				{
					currentLine = 23;
				}
				if (currentLine == 22) 
				{
					currentLine = 23;
				}

				//	At line 29 which is a "..." we start the conversation steering slider
				if (currentLine == 28) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 29)
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 30)
				{
					currentLine = 33;
				}
				if (currentLine == 32) 
				{
					currentLine = 33;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
				
		} 
		else if (friend.conversationCheck [2] && friend.suspicionLevel == 2) 
		{
			textFile = Resources.Load ("CN_SusLvl3") as TextAsset;

			if (!conversationPasses [3]) 
			{
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 9) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 10)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 11) 
				{
					currentLine = 14;
				}
				if (currentLine == 13)
				{
					currentLine = 14;
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 22)
				{
					StartCoroutine (DiableInputTimer (5f));
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
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
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;

		} 
		else if (friend.conversationCheck [1] && friend.suspicionLevel == 1) 
		{
			textFile = Resources.Load ("CN_SusLvl2") as TextAsset;

			if (!conversationPasses [3]) 
			{
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 11) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 12)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 13) 
				{
					currentLine = 16;
				}
				if (currentLine == 15)
				{
					currentLine = 16;
				}

				//	At line 21 which is a "..." we start the conversation steering slider
				if (currentLine == 23) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [1] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 24)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 25) 
				{
					currentLine = 28;
				}
				if (currentLine == 27) 
				{
					currentLine = 28;
				}

				//	At line 29 which is a "..." we start the conversation steering slider
				if (currentLine == 31) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 32)
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 33)
				{
					currentLine = 36;
				}
				if (currentLine == 35) 
				{
					currentLine = 36;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;

				
		} 
		else if (friend.conversationCheck [0]) 
		{

			//If we've gone through this conversation with raising suspicion we skip all the if statements
			if (!conversationPasses [3]) {
				//	Loads the file
				textFile = Resources.Load ("CN_SusLvl1") as TextAsset;
			
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 9) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 10) 
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 11) 
				{
					currentLine = 14;
				}
				if (currentLine == 13) 
				{
					currentLine = 14;
				}

				//	At line 21 which is a "..." we start the conversation steering slider
				if (currentLine == 23) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [1] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 24) 
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 25) 
				{
					currentLine = 28;
				}
				if (currentLine == 27) 
				{
					currentLine = 28;
				}

				//	At line 29 which is a "..." we start the conversation steering slider
				if (currentLine == 31) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;
				
					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 32) 
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 33) 
				{
					currentLine = 36;
				}
				if (currentLine == 35) 
				{
					currentLine = 36;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			} 
			else 
			{
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
			}

			//Sets the appropriate length for each text file
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
		}
	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	MikeConversation: Navigates through conversations and handles the					*/
/*								conversation's states									*/
/*		param: ref FriendController friend - Lets us know which friend and what 		*/
/*										 suspcion level they have						*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	void MikeConversation(ref FriendController friend)
	{
		talkingTo = friend.friendName;

		//	We check GC's suspciion level and load the appropriate file and split each line at the neter key "\n"
		if (friend.conversationCheck [3] && friend.suspicionLevel == 3) 
		{
			textFile = Resources.Load ("MK_SusLvl4") as TextAsset;

			if (!conversationPasses [3]) 
			{
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 9) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 10)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 11) 
				{
					currentLine = 14;
				}
				if (currentLine == 13)
				{
					currentLine = 14;
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 22)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 25) 
				{
					currentLine = 28;
				}
				if (currentLine == 27) 
				{
					currentLine = 28;
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
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
					currentLine = 33;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
		} 
		else if (friend.conversationCheck [2] && friend.suspicionLevel == 2)
		{
			textFile = Resources.Load ("MK_SusLvl3") as TextAsset;

			if (!conversationPasses [3]) 
			{
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 8)
				{
					StartCoroutine (DiableInputTimer (5f));
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
				if (currentLine == 19) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [1] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 20)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 21) 
				{
					currentLine = 24;
				}
				if (currentLine == 23) 
				{
					currentLine = 24;
				}

				//	At line 29 which is a "..." we start the conversation steering slider
				if (currentLine == 27) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 28)
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 29)
				{
					currentLine = 32;
				}
				if (currentLine == 31) 
				{
					currentLine = 32;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
		} 
		else if (friend.conversationCheck [1] && friend.suspicionLevel == 1) 
		{
			textFile = Resources.Load ("MK_SusLvl2") as TextAsset;

			if (!conversationPasses [3]) 
			{
				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 9) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 10)
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 11) 
				{
					currentLine = 14;
				}
				if (currentLine == 13)
				{
					currentLine = 14;
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
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 22)
				{
					StartCoroutine (DiableInputTimer (5f));
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
				if (currentLine == 28) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 29)
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 30)
				{
					currentLine = 33;
				}
				if (currentLine == 32) 
				{
					currentLine = 33;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41) 
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation(1.75f));
				}
			}
			else
			{
				friend.conversationCheck [0] = true;
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
				StartCoroutine (EndConversation(1.75f));
			}
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
		} 
		else if (friend.conversationCheck [0])
		{
			/*---------------Logic for Suspicion level 1 of GC------------------*/

			//If we've gone through this conversation with raising suspicion we skip all the if statements
			if (!conversationPasses [3]) 
			{
				//	Loads the file
				textFile = Resources.Load ("MK_SusLvl1") as TextAsset;

				//	At line 7 which is a "..." we start the conversation steering slider
				if (currentLine == 9) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [0] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 10) 
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 11) 
				{
					currentLine = 14;
				}
				if (currentLine == 13) 
				{
					currentLine = 14;
				}

				//	At line 21 which is a "..." we start the conversation steering slider
				if (currentLine == 23) 
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [1] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 24) 
				{
					StartCoroutine (DiableInputTimer (5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 25) 
				{
					currentLine = 28;
				}
				if (currentLine == 27) 
				{
					currentLine = 28;
				}

				//	At line 29 which is a "..." we start the conversation steering slider
				if (currentLine == 31)
				{
					//	We've passed conversation steering part of the conversation
					conversationPasses [2] = true;

					//	We are now steering a conversation. Make the slider appear and make it move
					conversation.transform.localScale = m_Appear;
					steerConversation = true;
					conversation.GetComponent<Conversation> ().startSlider = true;

					//	Start the timer
					StartCoroutine (SteerConversationTimer (3f, currentLine));
				}

				//	Disables input for 1.5 seconds to prevent player from clicking through dialogue
				if (currentLine == 32) 
				{
					StartCoroutine (DiableInputTimer (1.5f));
				}

				//	This is here to make the dialouge flow smoothly after failure and success states
				if (currentLine == 33) 
				{
					currentLine = 36;
				}
				if (currentLine == 35) 
				{
					currentLine = 36;
				}

				//	We have passed through this conversation text file. The last line will now appear
				if (currentLine == 41)
				{
					conversationPasses [3] = true;
					m_GameManager.conversationCount++;
					StartCoroutine (EndConversation (1.75f));
				}
			}
			else 
			{
				//	Makes panel appear with the "hold on I'm getting a text line"
				textpanel.alpha = 1f;
				textpanel.blocksRaycasts = true;
			}

			//Sets the appropriate length for each text file
			textLines = (textFile.text.Split ('\n'));
			endAtLine = textLines.Length - 1;
		}
	}

	public void RestroomDialouge()
	{
		//	We started a conversation
		conversationIsOver = false;

		textpanel.alpha = 1f;
		textpanel.blocksRaycasts = true;

		textFile = Resources.Load ("RestroomDialouge") as TextAsset;

		textLines = (textFile.text.Split ('\n'));
		endAtLine = textLines.Length - 1;
		//	Emptys the text panel after each block of text
		theText.text = "";

		//	Displays text on the panel
		currentTextLine = textLines [currentLine];
		//	Seprates text line word by word
		StartCoroutine (TypeText (currentTextLine));

		StartCoroutine (EndConversation(3f));

	}


	public void NotEating()
	{
		//	We started a conversation
		conversationIsOver = false;

		textpanel.alpha = 1f;
		textpanel.blocksRaycasts = true;

		textFile = Resources.Load ("PlayingWithFood") as TextAsset;

		textLines = (textFile.text.Split ('\n'));
		endAtLine = textLines.Length - 1;
		//	Emptys the text panel after each block of text
		theText.text = "";

		//	Displays text on the panel
		currentTextLine = textLines [currentLine];
		//	Seprates text line word by word
		StartCoroutine (TypeText (currentTextLine));

		StartCoroutine (EndConversation(3f));

	}

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame 														*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void Update()
	{
		//	If input is disabled we cannot click forwards
		if (!disableInput) 
		{
			//	Each click when we are not steering a conversation advances the dialouge
			if (Input.GetMouseButtonDown (0) && !steerConversation) 
			{
				currentLine++;
				if (m_FriendReference != null) 
				{
					LoadAndRunConversationWith (m_FriendReference);
				}

				//	Useed to make sure we don't get any Index out of bounds errors
				if (currentLine > textLines.Length - 1) 
				{
					currentLine = textLines.Length - 1;
				}

			}
			//	If we've reached the end of the text file we display the "Last line" we want to see
			if (currentLine == textLines.Length - 1) 
			{
				textpanel.alpha = 0f;
				textpanel.blocksRaycasts = false;
				currentLine = textLines.Length - 2;
			}

			if (textFile.name == "blank")
			{
				currentLine = 0;
			}

		}
	}
}
