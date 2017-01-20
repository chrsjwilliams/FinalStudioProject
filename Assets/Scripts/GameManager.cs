using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GameManager: Manages the state of the game and progresses player through game		*/
/*					  																	*/
/*																						*/
/*			Functions:																	*/
/*					Awake ()															*/
/*					Start ()															*/
/*					DisableInputTimer (float seconds)									*/
/*					LastTrip (float seconds)											*/
/*					ToggleRestroom ()													*/
/*					LeaveDinner ()														*/
/*					Update ()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class GameManager : MonoBehaviour 
{
	//	Public Variables
	public bool togglRestroomDialouge;			//	Turns on Restroom dialouge
	public bool visitingRestroom;				//	Is player in the restroom
	public int conversationCount;				//	Counts the conversations had by the player with the other characters
	public int restroomVisitCount;				//	Counts how many times the play visits the restroom
	public AudioClip backgrounMusic;			//	Game's background music	
	public AudioSource m_AudioSource;			//	Reference to Game Manager's audio source
	public FoodManager foodManager;				//	Reference to the Food Manager
	public GameObject[] m_Friends;				//	Stores friends in an array
	public GameObject m_RestroomScreen;			//	The restroom scene

	//	Private Variables
	private bool m_DisableInput;				//	Disables player's input
	private PlayerController m_Player;			//	Referrence to the player
	private Vector3 m_Hide;						//	Hides conversation steering slider
	private Vector3 m_RestroomScale;			//	Reference to Restroom scene scale
	private TextboxManager m_TextboxManager;	//	Reference to Textbox Manager


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
		restroomVisitCount = 0;
		visitingRestroom = false;
		togglRestroomDialouge = false;
		conversationCount = 0;
		m_Hide = new Vector3 (0, 0, 0);
		m_RestroomScale = new Vector3 (4.343573f, 4.34357f, 4.34357f);
		m_AudioSource.clip = backgrounMusic;
		m_AudioSource.Play ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	DisableInputTime: Disables input for x seconds										*/
	/*		param: float seconds - how many seconds the function waits for 					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator DisableInputTimer(float seconds)
	{
		m_DisableInput = true;
		yield return new WaitForSeconds (seconds);
		m_DisableInput = false;

	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	LastTrip: Takes player to restroom one last time before game ends					*/
	/*		param: float seconds - how many seconds the function waits for 					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator LastTrip(float seconds)
	{
		visitingRestroom = true;
		yield return new WaitForSeconds (seconds);
		SceneManager.LoadScene ("EndQuote", LoadSceneMode.Single);
		visitingRestroom = false;

	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	ToggleRestroom: Makes Restroom scene appear and disappear							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void ToggleRestroom()
	{
		visitingRestroom = !visitingRestroom;
		if (visitingRestroom)
		{
			restroomVisitCount++;
		}

		if (!visitingRestroom && restroomVisitCount > 3)
		{
			togglRestroomDialouge = true;
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	LeaveDinner: Gives player oppurtunity to quit at anytime							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void LeaveDinner()
	{
		StartCoroutine (LastTrip (5f));
	}
	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame (Will be used for visual fx)							*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if (SceneManager.GetActiveScene().name == "SplashScreen" && Input.GetMouseButtonDown(0))
		{
			SceneManager.LoadScene ("BeginQuote", LoadSceneMode.Single);
			StartCoroutine (DisableInputTimer(3.0f));
		}
		if (SceneManager.GetActiveScene().name == "BeginQuote" && Input.GetMouseButtonDown(0))
		{
			SceneManager.LoadScene ("Dinner", LoadSceneMode.Single);
			StartCoroutine (DisableInputTimer(3.0f));
		}
		if (SceneManager.GetActiveScene().name == "Dinner")
		{
			m_RestroomScreen = GameObject.FindGameObjectWithTag ("Restroom");
			m_TextboxManager = GameObject.FindGameObjectWithTag ("TextManager").GetComponent<TextboxManager>();
			foodManager = GameObject.FindGameObjectWithTag ("Menu").GetComponent<FoodManager> ();
			m_Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
			m_Friends = GameObject.FindGameObjectsWithTag ("Friend");
		}

		if (m_RestroomScreen != null) 
		{
			if (visitingRestroom) 
			{
				m_RestroomScreen.transform.localScale = m_RestroomScale;
			} 
			else 
			{
				m_RestroomScreen.transform.localScale = m_Hide;	
			}
		}

		if (m_Player != null) {

			if (conversationCount > 8 || m_Player.playerSelfImage < 0) {
			
				StartCoroutine (LastTrip (5f));
			}
		}
		if (SceneManager.GetActiveScene().name == "EndQuote" && Input.GetMouseButtonDown(0))
		{
			SceneManager.LoadScene ("SplashScreen", LoadSceneMode.Single);
			StartCoroutine (DisableInputTimer(3.0f));
			conversationCount = 0;
		}
	}
}
