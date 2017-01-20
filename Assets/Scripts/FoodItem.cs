using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	FoodItem: Controls player's interaction with menu									*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class FoodItem : MonoBehaviour 
{
	//	Public Variables
	public FoodManager menu;		//	Reference to menu
	public string food;				//	Stores the name of the food item

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start()
	{
		menu = GameObject.FindGameObjectWithTag ("Menu").GetComponent<FoodManager>();
		food = this.name;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	OrderThis: Stores what the player has ordered										*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void OrderThis()
	{
		menu.SendMessage ("Ordered", this.gameObject);
	}
}
