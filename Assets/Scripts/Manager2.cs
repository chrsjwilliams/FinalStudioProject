//CAT ROOMMATE by m@ boch - NYU GAMECENTER - Oct 2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Manager2 : MonoBehaviour
{
	//public variables like this one are accessible to other scripts, and often set in the inspector
	//they're great for tunable variables because we can change them while the game is running
	//sometimes it's just easier to set a reference to another object by dragging it into a public variable in the inspector
	public TextMesh QuestionTextMesh; //Text Meshes for Questions & Answers
	public TextMesh Answer1TextMesh;
	public TextMesh Answer2TextMesh;

	public SpriteRenderer CatSpriteRenderer; //Sprite Renderer

	public Sprite CatNormal; //Cat Emoji Sprites
	public Sprite CatHappy;
	public Sprite CatJoy;
	public Sprite CatExcited;
	public Sprite CatLove;
	public Sprite CatTricky;
	public Sprite CatKissy;
	public Sprite CatCoy;
	public Sprite CatMad;
	public Sprite CatSad;
	public Sprite CatAgony;
	public Sprite CatSleepy;

	public List<string> Questions; //Lists for Questions & Answers
	public List<string> Answers1;
	public List<string> Answers2;

	//private variables have to be set in code, like Phaser's global variables
	private int currentQuestion; //Keep track of which questions
	private int goodAnswers; //Count Good Answers
	private int badAnswers; //Count Bad Answers

	bool readyToLoadNextScene; 

	// Use this for initialization
	void Start ()
	{
		readyToLoadNextScene = false;
		//annoyingly, you can't make linebreaks in the unity editor
		//so here we're just goint through all the strings in Questions
		//and wherever we see "BREAK" we're replacing it with \n
		//which is then read as a line break
		int i = 0; 
		foreach (string s in Questions)
		{
			Questions[i] = s.Replace("BREAK", "\r\n");
			i++;
		}

		currentQuestion = 0; //set current question to 0
		QuestionTextMesh.text = Questions[currentQuestion]; //set the starting values for question text mesh
		Answer1TextMesh.text = Answers1[currentQuestion]; //set the starting values for answer1 text mesh
		Answer2TextMesh.text = Answers2[currentQuestion]; //set the starting values for answer2 text mesh
	}
		
	//OnMouseDown is sent so long as an object has a collider on it
	void OnMouseDown()
	{
		Debug.Log("User clicked at: " + Input.mousePosition); //logging the click position to the unity console
		if (currentQuestion >= 11) //Now all questions answered, so it's time to give a result.
		{	if (Input.GetMouseButtonDown (0)) {
				if (readyToLoadNextScene == true){  //assuming you have a global variable with this name
					SceneManager.LoadScene ("Fourth");
				}
				else if (currentQuestion >= 11){
					readyToLoadNextScene = true;
				}
			}
			Answer1TextMesh.text = ""; //set answer to blank
			Answer2TextMesh.text = ""; //set answer to blank
			if (goodAnswers > badAnswers) //if more good answers than bad
			{
				QuestionTextMesh.text = "and you will hate yourself"; //Cat wants to live with you
				CatSpriteRenderer.sprite = CatHappy; //Set cat sprite
			}
			else
			{
				QuestionTextMesh.text = "and you will hate yourself"; //Cat can't live with you
				CatSpriteRenderer.sprite = CatCoy; //Set cat sprite
			}
		}
		else //otherwise, we need to keep updating answers & questions
		{
			//Answer is Left Answer, as the mouse was on the negative side of the x axis when clicked
			if (Input.mousePosition.x < Screen.width/2)
			{
				//all of these are the same, we set a new cat sprite, then add one to goodAnswers or badAnswers accordingly
				if (currentQuestion == 0)
				{
					CatSpriteRenderer.sprite = CatExcited; //set the cat sprite
					badAnswers++; //add one to good answers
				}
				else if (currentQuestion == 1)
				{
					CatSpriteRenderer.sprite = CatJoy;
					badAnswers++;
				}
				else if (currentQuestion == 2)
				{
					CatSpriteRenderer.sprite = CatMad;
					goodAnswers++;
				}
				else if (currentQuestion == 3)
				{
					CatSpriteRenderer.sprite = CatSad;
					goodAnswers++;
				}
				else if (currentQuestion == 4)
				{
					CatSpriteRenderer.sprite = CatSleepy;
					goodAnswers++;
				}
				else if (currentQuestion == 5)
				{
					CatSpriteRenderer.sprite = CatLove;
					goodAnswers++;
				}
				else if (currentQuestion == 6)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 7)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 8)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 9)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 10)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 11)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
			}
			//Answer is Left Answer, as the mouse was on the positive side of the x axis when clicked
			else if (Input.mousePosition.x >= Screen.width/2)
			{
				//all of these are the same, we set a new cat sprite, then add one to goodAnswers or badAnswers accordingly
				if (currentQuestion == 0)
				{
					CatSpriteRenderer.sprite = CatSleepy; //set the cat sprite
					badAnswers++; //add one to good answers
				}
				if (currentQuestion == 1)
				{
					CatSpriteRenderer.sprite = CatNormal;
					badAnswers++;
				}
				else if (currentQuestion == 2)
				{
					CatSpriteRenderer.sprite = CatJoy;
					badAnswers++;
				}
				else if (currentQuestion == 3)
				{
					CatSpriteRenderer.sprite = CatTricky;
					goodAnswers++;
				}
				else if (currentQuestion == 4)
				{
					CatSpriteRenderer.sprite = CatJoy;
					badAnswers++;
				}
				else if (currentQuestion == 5)
				{
					CatSpriteRenderer.sprite = CatAgony;
					badAnswers++;
				}
				else if (currentQuestion == 6)
				{
					CatSpriteRenderer.sprite = CatMad;
					badAnswers++;
				}
				else if (currentQuestion == 7)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 8)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 9)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 10)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
				else if (currentQuestion == 11)
				{
					CatSpriteRenderer.sprite = CatKissy;
					badAnswers++;
				}
			}
			currentQuestion++; //moving on to the next question
			QuestionTextMesh.text = Questions[currentQuestion]; //setting the text mesh to the next question
			Answer1TextMesh.text = Answers1[currentQuestion]; //setting the text mesh to the next answer
			Answer2TextMesh.text = Answers2[currentQuestion]; //setting the text mesh to the next answer
		}
	}
}
