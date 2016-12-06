using UnityEngine;
using System.Collections;

public class ParticleSystemOrder : MonoBehaviour {

	// Use this for initialization
	public PlayerController player;
	public GameManager gm;
	public int renderOrder = 0;
	public float startLife;
	int lastRenderOrder = 0;

	void Start () 
	{
		startLife = 10;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		GetComponent<Renderer>().material.renderQueue = renderOrder;
		lastRenderOrder = renderOrder;
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Particle";
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		//GetComponent<ParticleSystem> ().Stop ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (renderOrder != lastRenderOrder)
		{
			GetComponent<Renderer>().material.renderQueue = renderOrder;
			lastRenderOrder = renderOrder;
		}



		if (!gm.m_VisitingRestroom & tag != "FriendEye")
		{
			GetComponent<ParticleSystem> ().Stop ();
		}
		else if (transform.parent != null)
		{
			if (GetComponentInParent<FriendController>().suspicionLevel < 2)
			{
				Debug.Log ("Here");
				GetComponent<ParticleSystem> ().Stop ();
			}
		}
		else
		{
			GetComponent<ParticleSystem> ().Play ();

		}
	}
}
