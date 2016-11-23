using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * 
 * 
 * 
 * 										TO BE DELETED
 * 
 * 
 * 					REASON WHY IT'S STILL HERE: Script may be useful in displaying one word at a time like an rpg

public class TextBubble : MonoBehaviour 
{
	//	Public Variables
	public bool isVisible;

	//	Private Variables
	public Image m_TextBubble;
	public UnityEngine.UI.Text m_Text;

	float letterPause = 0.2f;
	public string word = "I’m so happy you finally got a chance to come out with us! We’ve all really missed you."; // change this one in the inspector
	private string currentWord = "";


	void Start ()
	{
		isVisible = false;
		m_TextBubble = GetComponentInParent<Image> ();
		m_Text = gameObject.GetComponent<UnityEngine.UI.Text> ();

	}



	void AddText(string newText)
	{
		word = newText;
		TypeText(word);
	}

	private IEnumerator TypeText (string compareWord)
	{
		char[] displayThis = compareWord.ToCharArray();
		for(int i = 0; i < displayThis.Length; i++) 
		{
			if (word != compareWord)
				break;
			currentWord += displayThis [i];

			m_Text.text = m_Text.text + currentWord;
			yield return new WaitForSeconds (letterPause);

			//for added fun, use this instead :D ...
			//yield WaitForSeconds(letterPause * Random.Range(0.5, 2));
		}  
	}
	

	void Update () 
	{

		m_Text.text = currentWord;
		if(isVisible)
		{
			m_TextBubble.enabled = true;
			m_Text.enabled = true;

		}
		else
		{
			m_TextBubble.enabled = false;
			m_Text.enabled = false;
		}
	}
}
*/