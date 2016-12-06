using UnityEngine;
using System.Collections;

public class RestroomHandler : MonoBehaviour {

	public GameManager gm;
	public PlayerController player;
	public TextboxManager textboxManager;
	// Use this for initialization
	void Start () 
	{
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		textboxManager = GameObject.FindGameObjectWithTag ("TextManager").GetComponent<TextboxManager> ();
	}

	public void ToggleRestroom()
	{
		if (!player.isTalking) 
		{

			gm.ToggleRestroom ();
			player.GainComposure ();
			if (gm.restroomVisitCount > 3 && !gm.m_VisitingRestroom) 
			{
				textboxManager.RestroomDialouge ();
				gm.m_Friends [Random.Range(0, 3) % 3].GetComponent<FriendController> ().RaiseSuspicion ();
			}
		}
	}


	void Update () 
	{
		

	}
}
