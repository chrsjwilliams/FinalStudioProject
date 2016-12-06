using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GameManager: Manages the state of the game and progresses player through game		*/
/*					  																	*/
/*																						*/
/*			Functions:																	*/
/*					Start()																*/
/*					DisableInputTimer()													*/
/*					Update()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class GameManager : MonoBehaviour 
{
	public int conversationCount;
	public int restroomVisitCount;
	public bool togglRestroomDialouge;
	private bool m_DisableInput;
	public bool m_VisitingRestroom;
	private TextboxManager m_TextboxManager;
	public GameObject m_RestroomScreen;
	[SerializeField]
	private PlayerController m_Player;
	public FoodManager foodManager;
	public GameObject[] m_Friends;
	private Vector3 m_Hide;								//	Hides conversation steering slider
	private Vector3 m_RestroomScale;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		restroomVisitCount = 0;
		m_VisitingRestroom = false;
		togglRestroomDialouge = false;
		conversationCount = 0;
		m_Hide = new Vector3 (0, 0, 0);
		m_RestroomScale = new Vector3 (4.343573f, 4.34357f, 4.34357f);
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

	public void ToggleRestroom()
	{
		m_VisitingRestroom = !m_VisitingRestroom;
		if (m_VisitingRestroom)
		{
			restroomVisitCount++;
		}

		if (!m_VisitingRestroom && restroomVisitCount > 3)
		{
			togglRestroomDialouge = true;
		}
	}

	public void LeaveDinner()
	{
		Application.LoadLevel ("EndQuote");
	}
	
/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame (Will be used for visual fx)							*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if (Application.loadedLevelName == "SplashScreen" && Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel ("BeginQuote");
			StartCoroutine (DisableInputTimer(3.0f));
		}
		if (Application.loadedLevelName == "BeginQuote" && Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel ("Dinner");
			StartCoroutine (DisableInputTimer(3.0f));
		}
		if (Application.loadedLevelName == "Dinner")
		{
			m_RestroomScreen = GameObject.FindGameObjectWithTag ("Restroom");
			m_TextboxManager = GameObject.FindGameObjectWithTag ("TextManager").GetComponent<TextboxManager>();
			foodManager = GameObject.FindGameObjectWithTag ("Menu").GetComponent<FoodManager> ();
			m_Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
			m_Friends = GameObject.FindGameObjectsWithTag ("Friend");


		}

		if (m_RestroomScreen != null) 
		{
			if (m_VisitingRestroom) 
			{
				m_RestroomScreen.transform.localScale = m_RestroomScale;
			} 
			else 
			{
				m_RestroomScreen.transform.localScale = m_Hide;	
			}
		}

		if (conversationCount > 8)
		{
			Application.LoadLevel ("EndQuote");
		}
		else if (conversationCount > 5)
		{
			//	show what player ordered
			//	Bring out entrees
		}
		else if (conversationCount > 2)
		{
			//	show what player ordered
			//	BRing out desserts
		}
		else if (conversationCount > 0)
		{
			
		}

		if (Application.loadedLevelName == "EndQuote" && Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel ("SplashScreen");
			StartCoroutine (DisableInputTimer(3.0f));
			conversationCount = 0;
		}
	}
}
