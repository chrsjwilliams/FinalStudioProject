using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	ParticleSystemOrder: pushes particles infront of sprites and activates them	on		*/
/*						 the player.													*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class ParticleSystemOrder : MonoBehaviour {

	//	Public Variables
	public PlayerController player;			//	Refrence to player
	public GameManager gm;					//	Reference to GameManager
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
	void Update () {
	
		if (renderOrder != lastRenderOrder)
		{
			GetComponent<Renderer>().material.renderQueue = renderOrder;
			lastRenderOrder = renderOrder;
		}



		if (!gm.visitingRestroom)
		{
			GetComponent<ParticleSystem> ().Stop ();

		}
		else if (transform.parent != null)
		{
			if (GetComponentInParent<FriendController>().suspicionLevel < 1)
			{
				GetComponent<ParticleSystem> ().Stop ();
			}
		}
		else
		{
			if (tag == "Self")
			{
				if (player.playerSelfImage > 6) 
				{
					GetComponent<ParticleSystem> ().startLifetime = 1;
				}
				else if (player.playerSelfImage > 4) 
				{
					GetComponent<ParticleSystem> ().startLifetime = 3;
				}
				else if (player.playerSelfImage > 2) 
				{
					GetComponent<ParticleSystem> ().startLifetime = 5;
				}
				else
				{
					GetComponent<ParticleSystem> ().startLifetime = 10;
				}
			}

			if (tag == "FriendEye")
			{
				if (GetComponentInParent<FriendController>().suspicionLevel == 2)
				{
					GetComponent<ParticleSystem> ().startLifetime = 1;
				}
				else if (GetComponentInParent<FriendController>().suspicionLevel == 3)
				{
					GetComponent<ParticleSystem> ().startLifetime = 3;
				}
				else if (GetComponentInParent<FriendController>().suspicionLevel == 4)
				{
					GetComponent<ParticleSystem> ().startLifetime = 10;
				}

			}

			GetComponent<ParticleSystem> ().Play ();

		}
	}
}
