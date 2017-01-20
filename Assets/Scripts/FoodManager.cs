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
/*					Ordered (GameObject menuItem)										*/
/*					FinihsedOrdering ()													*/
/*					SetFoodImages (string food)											*/
/*					SetDrinkImages (string drink)										*/
/*					Update ()															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class FoodManager : MonoBehaviour 
{
	//	Public Variables
	public bool finishedOrdering;			//	Comunicates to game player is done ordering
	public string[] dinnerOrder;			//	Stores the names of the items the player has ordered
	public GameObject item;					//	The current item
	public GameObject[] drinkItems;			//	Array of all dirnk items
	public GameObject[] appItems;			//	Array of all appetizer items
	public GameObject[] entreeItems;		//	Array of all entree items
	public GameObject[] dessertItems;		//	Array of all dessert items
	public Image menu;						//	Image of the menu
	public Image yourOrder;					//	The image of the food items you ordered
	public Image yourDrink;					//	The image of the drink item you ordered
	public PlayerController player;			//	Reference to player controller
	public Sprite[] menuItems;				//	Array of sprites for menu items

	//	Private Variables
	private Color m_Color;					//	Color of highlight when cursor is over food
	private Color temp;						//	Temporary variable to swap color
	private GameManager m_GM;				//	Reference to GameManager
	private Vector3 m_Hide;					//	Hides menu

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

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Ordered: Logic for menu and storing player's order									*/
	/*		param: GameObject menuItem -													*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
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
			if(item.name == "Mozzarella Stick")
			{
				player.playerSelfImage = 4;
			}
			else if (item.name == "Nacho")
			{
				player.playerSelfImage = 3;
			}

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

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FinishedOrdering: Hides menu when finished ordering									*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void FinishedOrdering()
	{
		transform.localScale = m_Hide;
		finishedOrdering = true;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetFoodImage: Displays sprite with your meal										*/
	/*		param: string food - the name of what you ordered								*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
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

		}
		if (food.Equals("Nacho"))
		{
			GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().sprite = menuItems [5];
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

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetDrinkImage: Displays sprite with your drink										*/
	/*		param: string food - the name of what you ordered								*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
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
				Color newAlpha = new Color (GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().color.r, GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().color.g, GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().color.b, 1);
				GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().color = newAlpha;
				SetFoodImage (dinnerOrder [3]);
			}
			else if (m_GM.conversationCount > 2)
			{
				Color newAlpha = new Color (GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().color.r, GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().color.g, GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().color.b, 1);
				GameObject.FindGameObjectWithTag ("YourMeal").GetComponent<Image> ().color = newAlpha;
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
