using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Food : MonoBehaviour {

	public bool playingWithFood;
	public int timesPlayedWithFood;
	public TextboxManager textboxManager;
	public GameManager gm;
	public PlayerController player;
	public Animator m_Animatior;
	private Image image;

	// Use this for initialization
	void Start () 
	{
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		playingWithFood = false;
		m_Animatior = GetComponent<Animator> ();
		timesPlayedWithFood = 0;
		textboxManager = GameObject.FindGameObjectWithTag ("TextManager").GetComponent<TextboxManager> ();
		image = GetComponent<Image> ();
	}


	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	DisableInputTime: Disables input for x seconds										*/
	/*		param: float seconds - how many seconds the function waits for 					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator PlayingWithFood(float seconds)
	{
		playingWithFood = true;
		m_Animatior.SetBool ("PlayingWithFood", playingWithFood);
		yield return new WaitForSeconds (seconds);
		playingWithFood = false;
		m_Animatior.SetBool ("PlayingWithFood", playingWithFood);
		timesPlayedWithFood++;

	}

	public void PlayWithFood()
	{
		StartCoroutine (PlayingWithFood (0.5f));

		player.LowerSelfImage ();

		if (timesPlayedWithFood > 3)
		{
			textboxManager.NotEating ();
			gm.m_Friends [Random.Range(0,3) % 3].GetComponent<FriendController> ().RaiseSuspicion ();
		}
		else
		{
			gm.m_Friends [Random.Range (0, 3) % 3].GetComponent<FriendController> ().LowerSuspicion ();
		}

		Color newAlpha = new Color (image.color.r, image.color.g, image.color.b, image.color.a * 0.85f);

		image.color = newAlpha;
	}


	// Update is called once per frame
	void Update () 
	{
		
	}
}
