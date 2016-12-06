using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	FoodManager: Manages the state of the food and  beverages throughout the game		*/
/*					  																	*/
/*																						*/
/*			Functions:																	*/
/*					Start()																*/
/*					DisableInputTimer()													*/
/*					Update()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class FoodManager : MonoBehaviour 
{
	public bool finishedOrdering;
	public string[] dinnerOrder;
	public GameObject item;
	public GameObject[] drinkItems;
	public GameObject[] appItems;
	public GameObject[] entreeItems;
	public GameObject[] dessertItems;
	public Image menu;
	public Image yourOrder;
	public Image yourDrink;
	public PlayerController player;

	public Sprite[] menuItems;

	private Color m_Color;
	private Color temp;
	private GameManager m_GM;
	private Vector3 m_Hide;								//	Hides menu

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		m_GM = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager>();
		finishedOrdering = false;
		dinnerOrder = new string[4];

		item = GameObject.Find ("Temp");

		drinkItems = GameObject.FindGameObjectsWithTag("Beverages");
		appItems = GameObject.FindGameObjectsWithTag ("Appetizers");
		entreeItems = GameObject.FindGameObjectsWithTag ("Entree");
		dessertItems = GameObject.FindGameObjectsWithTag ("Desserts");

		temp = new Color (1.0f, 1.0f, 1.0f);
		m_Color = new Color (0.17f, 0.54f, 0.13f);
		m_Hide = new Vector3 (0, 0, 0);

		yourOrder = GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ();
		yourDrink = GameObject.FindGameObjectWithTag ("Water").GetComponent<Image> ();
	}

	public void Ordered(GameObject menuItem)
	{
		item = menuItem;

		if (item.GetComponent<Toggle>().isOn)
		{
			Color temp = new Color (1.0f, 1.0f, 1.0f);
			item.GetComponentInChildren<Text> ().color = temp;
			temp = m_Color;
			item.GetComponentInChildren<Text> ().color = temp;
		}

		if (item.tag == "Beverages")
		{
			dinnerOrder [0] = item.name;
			foreach (GameObject otherItem in drinkItems)
			{
				if (!otherItem.Equals(item))
				{
					//	change to toggle. set toggle text to different color
					otherItem.GetComponent<Toggle> ().enabled = false;
				}
			}
		}
		if (item.tag == "Appetizers")
		{
			dinnerOrder [1] = item.name;
			foreach (GameObject otherItem in appItems)
			{
				if (!otherItem.Equals(item))
				{
					otherItem.GetComponent<Toggle> ().enabled = false;
				}
			}
		}
		if (item.tag == "Entree")
		{
			dinnerOrder [2] = item.name;
			foreach (GameObject otherItem in entreeItems)
			{
				if (!otherItem.Equals(item))
				{
					otherItem.GetComponent<Toggle> ().enabled = false;
				}
			}
		}
		if (item.tag == "Desserts")
		{
			dinnerOrder [3] = item.name;
			foreach (GameObject otherItem in dessertItems)
			{
				if (!otherItem.Equals(item))
				{
					otherItem.GetComponent<Toggle> ().enabled = false;
				}
			}
		}
	}

	public void FinishedOrdering()
	{
		transform.localScale = m_Hide;
		finishedOrdering = true;
	}

	public void SetFoodImage(string food)
	{
		//	Needs to relate to self image somehow I want specific numbers
		if (food.Equals("Spring Roll"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [3];
		}
		if (food.Equals("Mozzarella Stick"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [4];
			player.playerSelfImage = 8;
		}
		if (food.Equals("Nacho"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [5];
			player.playerSelfImage = 6;
		}
		if (food.Equals("Salad"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [6];
		}
		if (food.Equals("Salmon"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [7];
		}
		if (food.Equals("Steak"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [8];
		}
		if (food.Equals("Flan"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [9];
		}
		if (food.Equals("Cake"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [10];
		}
		if (food.Equals("Fruit Tart"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [11];
		}
	}

	public void SetDrinkImage(string drink)
	{
		if (drink.Equals("Soda"))
		{
			GameObject.FindGameObjectWithTag ("Water").GetComponent<Image> ().sprite = menuItems [0];
		}
		if (drink.Equals("Fruit Jucie"))
		{
			GameObject.FindGameObjectWithTag ("Water").GetComponent<Image> ().sprite = menuItems [1];
		}
		if (drink.Equals("Green Tea"))
		{
			GameObject.FindGameObjectWithTag ("Water").GetComponent<Image> ().sprite = menuItems [2];
		}
	}
	
/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Update: Called once per frame (Will be used for visual fx)							*/
/*																						*/
///*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if (item.GetComponent<Toggle>() != null)
		{
			if (!item.GetComponent<Toggle>().isOn)
			{
				item.GetComponentInChildren<Text> ().color = temp;
				temp = m_Color;
				item.GetComponentInChildren<Text> ().color = temp;
			}
		}

		if(finishedOrdering)
		{
			if (m_GM.conversationCount > 5)
			{
				SetFoodImage (dinnerOrder [3]);
			}
			else if (m_GM.conversationCount > 2)
			{
				SetFoodImage (dinnerOrder [2]);
			}
			else if (m_GM.conversationCount > -1)
			{
				SetFoodImage (dinnerOrder [1]);
				SetDrinkImage (dinnerOrder [0]);
			}
		}
	}
}
