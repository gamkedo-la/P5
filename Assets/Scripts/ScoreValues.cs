using UnityEngine;
using System.Collections;


public class ScoreValues : MonoBehaviour {

	public int nutrition = -100;
	public int alcohol = -100;
	public int flavor = -100;

	public int itemCost = 3;//in dollars
	public int itemWeight = 1000; //in grams  

	public bool removedYet = false; //first time out of the basket to solve respawning bug
	public bool required = false;

	public string funFacts = "";

	public bool isAtLeastPartlyInBasket = false;
	public bool isAtLeastPartlyAboveBasket = false;
}
