using UnityEngine;
using System.Collections;

public class FoodItem : MonoBehaviour 
{

	public FoodManager menu;
	public string food;

	void Start()
	{
		menu = GameObject.FindGameObjectWithTag ("Menu").GetComponent<FoodManager>();
		food = this.name;
	}

	void OrderThis()
	{
		menu.SendMessage ("Ordered", this.gameObject);
	}
}
