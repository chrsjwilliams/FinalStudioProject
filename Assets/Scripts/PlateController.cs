using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlateController : MonoBehaviour {


	public AudioClip chew;
	Image foodImage;
	PlayerController player;

	private AudioSource m_AudioSource;					//	AudioSource for player

	void Awake ()
	{
		m_AudioSource = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () 
	{
		foodImage = GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}

	public void EatFood()
	{
		Color newAlpha = new Color (foodImage.color.r, foodImage.color.g, foodImage.color.b, foodImage.color.a * 0.85f);
		m_AudioSource.PlayOneShot (chew, 0.5f);
		foodImage.color = newAlpha;
		player.LowerSelfImage ();
	}

	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
