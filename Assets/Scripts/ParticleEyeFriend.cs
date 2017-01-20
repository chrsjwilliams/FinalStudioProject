using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	ParticleEyeFriend: pushes particles infront of sprites and activates them			*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class ParticleEyeFriend : MonoBehaviour {

	// Public Variables
	public PlayerController player;			//	Reference to the player
	public FriendController friend;			//	Referecne to the friend
	public GameManager gm;					//	Reference to Game Manager
	public int renderOrder = 0;				//	Render Order
	public float startLife;					//	How volatile the particle effect will be
	int lastRenderOrder = 0;				//	Pushes particles in front of sprites

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		startLife = 10;
		friend = transform.parent.GetComponent<FriendController> ();

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		GetComponent<Renderer>().material.renderQueue = renderOrder;
		lastRenderOrder = renderOrder;
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Particle";
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();

	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame (Will be used for visual fx)							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update ()
	{
		if (renderOrder != lastRenderOrder)
		{
			GetComponent<Renderer>().material.renderQueue = renderOrder;
			lastRenderOrder = renderOrder;
		}

		if (friend.suspicionLevel == 2)
		{
			GetComponent<ParticleSystem> ().startLifetime = 1;
			GetComponent<ParticleSystem> ().Play ();
		}
		else if (friend.suspicionLevel == 3)
		{
			GetComponent<ParticleSystem> ().startLifetime = 5;
			GetComponent<ParticleSystem> ().Play ();
		}
		else if (friend.suspicionLevel == 4)
		{
			GetComponent<ParticleSystem> ().startLifetime = 10;
			GetComponent<ParticleSystem> ().Play ();
		}
	}
}
