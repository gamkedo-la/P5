﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class TunedLevel	{

	public int nutritionGoal = 10;
	public int alcoholGoal = 10;
	public int flavorGoal = 10;
	public int costLimit = 20;
	public int weightLimit = 4500;

}

public class MousePlacer : MonoBehaviour {

//	public GameObject placingObject;
	public AudioClip[] voices;
	private int voiceNum = 0;
	private AudioSource mySound;
	public AudioClip lift;
	public AudioClip release;

	public Shader diffuseShader;
	public Shader selectionShader;
	public Shader invalidShader;

	private GameObject activeObject;
	private bool hasBeenRotated;
	int clickMask;

	private List<ScoreValues> atLeastPartlyInBasket;

	private int nutritionScore = 0;
	private int alcoholScore = 0;
	private int flavorScore = 0;
	private int costNow = 0;
	private int weightNow = 0;

	public TunedLevel[] levels;
	static public int levelNow = 0; //keeps level between scene changes

	public Text scoreDisplay;

	public Text heldItemName;
	public Text heldItemCost;
	public Text heldItemWeight;
	public Text heldItemFacts;

	public Text nutrDisplayTotal;
	public Text flavDisplayTotal;
	public Text alcoDisplayTotal;

	public Text nutrDisplayItem;
	public RectTransform nutrDisplayBar;
	public Text flavDisplayItem;
	public RectTransform flavDisplayBar;
	public Text alcoDisplayItem;
	public RectTransform alcoDisplayBar;

	public GameObject itemInfo;

	public Text costWeight;

//	private int alternateShuffle = 0;
//	public Vector3[] sideAdjustTranslation;

	// Use this for initialization
	void Start () {
		mySound = GetComponent<AudioSource>();
		voiceNum = UnityEngine.Random.Range(0, voices.Length);
	//	activeObject = GameObject.Instantiate(placingObject);
		clickMask = ~LayerMask.GetMask("Ignore Raycast"); //flip bitmask
		atLeastPartlyInBasket = new List<ScoreValues>();
		itemInfo.SetActive(false);
		UpdateScoring();
	}

	public void EnteredBasket(ScoreValues obj){

		atLeastPartlyInBasket.Add(obj);
	}

	public void ExitBasket(ScoreValues obj){

		atLeastPartlyInBasket.Remove(obj);

		Renderer rend = obj.GetComponentInChildren<Renderer>();
		if(rend.material.shader != selectionShader) {
			rend.material.shader = invalidShader;
		}

	}

	public void UpdateScoring() {

		nutritionScore = 0;
		alcoholScore = 0;
		flavorScore = 0;

		costNow = 0;
		weightNow = 0;

		foreach(ScoreValues oneObj in atLeastPartlyInBasket){

			Renderer rend = oneObj.GetComponentInChildren<Renderer>();


			if(rend.material.shader == selectionShader) { // don't count object held in hand
				continue;
			}

			if (oneObj.isAtLeastPartlyAboveBasket == false){

				nutritionScore += oneObj.nutrition;
				alcoholScore += oneObj.alcohol;
				flavorScore += oneObj.flavor;

				costNow += oneObj.itemCost;
				weightNow += oneObj.itemWeight;
				if(rend.material.shader != diffuseShader) {
					rend.material.shader = diffuseShader;
				}
			} else {
				if(rend.material.shader != invalidShader) {
					rend.material.shader = invalidShader;
				}
			}

		}

		TunedLevel lvl = levels[levelNow];

		if (nutritionScore >= lvl.nutritionGoal && alcoholScore >= lvl.alcoholGoal && flavorScore >= lvl.flavorGoal
			&& costNow <= lvl.costLimit && weightNow <= lvl.weightLimit){

			Debug.Log("YOU WIN!");
			levelNow++;

			if(levelNow >= levels.Length){

				levelNow = levels.Length - 1;
				Debug.Log("No mas levels.");

			}

			Application.LoadLevel(Application.loadedLevel); //restart

		}
			
		nutrDisplayTotal.text = nutritionScore + "/ " + lvl.nutritionGoal;
		flavDisplayTotal.text = flavorScore + "/ " + lvl.flavorGoal;
		alcoDisplayTotal.text = alcoholScore + "/ " + lvl.alcoholGoal;

		Vector3 newScale = Vector3.one;
		newScale.x = ((float)nutritionScore) / ((float)lvl.nutritionGoal);
		newScale.x = Mathf.Clamp01(newScale.x);
		nutrDisplayBar.localScale = newScale;

		newScale = Vector3.one;
		newScale.x = ((float)flavorScore) / ((float)lvl.flavorGoal);
		newScale.x = Mathf.Clamp01(newScale.x);
		flavDisplayBar.localScale = newScale;

		newScale = Vector3.one;
		newScale.x = ((float)alcoholScore) / ((float)lvl.alcoholGoal);
		newScale.x = Mathf.Clamp01(newScale.x);
		alcoDisplayBar.localScale = newScale;

		costWeight.text = "Cost: $" + costNow + ".00/$" + lvl.costLimit + ".00"
		+ "\nWeight: " + weightNow + "g/" + lvl.weightLimit + "g"
		+ "\n=============="; // ugly hack to reduce the font changing sizes due to best fit when weights are low
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log(atLeastPartlyInBasket.Count);

		Vector3 mousePos = Input.mousePosition;
	//	Debug.Log(mousePos);
	//	activeObject.transform.position = mousePos;


		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rhInfo;

		if (Physics.Raycast(ray, out rhInfo, 5000f, clickMask)){ //out causes pass by reference


			//if not carrying an object
			if (activeObject == null){

				//pick up the object assuming that you've clicked on one, while holding the mouse down
				if(rhInfo.collider.gameObject.CompareTag("PicnicObject")) {
			
					GameObject mouseOverObject = rhInfo.collider.gameObject;
					Renderer rend = mouseOverObject.GetComponent<Renderer>();
					if(Input.GetButtonDown("Fire1")) {
						if(mySound.isPlaying == false) {
							mySound.clip = lift;
							mySound.Play();
						}

						hasBeenRotated = false;
						activeObject = mouseOverObject;
						rend.material.shader = selectionShader;
						activeObject.layer = LayerMask.NameToLayer("Ignore Raycast");
						activeObject.GetComponentInParent<Rigidbody>().useGravity = false;
						activeObject.GetComponentInParent<Rigidbody>().isKinematic = true;
						activeObject.GetComponent<Collider>().isTrigger = true;
						activeObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
						activeObject.GetComponentInParent<Rigidbody>().angularVelocity = Vector3.zero;
					}

					ScoreValues svScript = mouseOverObject.GetComponentInParent<ScoreValues>();
					nutrDisplayItem.text = "Nutrition: " + svScript.nutrition;
					flavDisplayItem.text = "Flavor: " + svScript.flavor;
					alcoDisplayItem.text = "Maturity: " + svScript.alcohol;

					String nameNoClone = svScript.name;
					nameNoClone = nameNoClone.Replace("(Clone)", "");
					nameNoClone = nameNoClone.Trim();
					svScript.name = nameNoClone;
					heldItemName.text = nameNoClone;
					heldItemFacts.text = svScript.funFacts;
					heldItemCost.text = "$" + svScript.itemCost + ".00";
					heldItemWeight.text = svScript.itemWeight + "g";
					itemInfo.SetActive(true);
				} else {
					itemInfo.SetActive(false);
				}

			}
			//if carrying an object
			else {

				//it follows the mouse pointer
				Vector3 carryPoint = rhInfo.point;

				carryPoint += Vector3.up * 1.5f;
				activeObject.transform.position = carryPoint;

				if (Input.GetKeyDown(KeyCode.A)){

					cleanUpRotationIfFirst();
					activeObject.transform.Rotate(90,0,0);

				}
				if (Input.GetKeyDown(KeyCode.S)){

					cleanUpRotationIfFirst();
					activeObject.transform.Rotate(0,90,0);

				}
				if (Input.GetKeyDown(KeyCode.D)){

					cleanUpRotationIfFirst();
					activeObject.transform.Rotate(0,0,90);

				}

				//until you release the mouse button
				if (Input.GetButtonUp("Fire1")){

					if(mySound.isPlaying == false && UnityEngine.Random.Range(0,100)<19) {
						mySound.clip = voices[voiceNum];
						mySound.Play();
						voiceNum++;
						voiceNum = voiceNum % voices.Length;
					}

					if(mySound.isPlaying == false) {
						mySound.clip = release;
						mySound.Play();
					}

					Renderer rend = activeObject.GetComponent<Renderer>();
					rend.material.shader = invalidShader;//diffuseShader;

					activeObject.layer = LayerMask.NameToLayer("Default");
					activeObject.GetComponentInParent<Rigidbody>().useGravity = true;
					activeObject.GetComponentInParent<Rigidbody>().isKinematic = false;
					activeObject.GetComponent<Collider>().isTrigger = false;
					activeObject = null;

					nutrDisplayItem.text = "Nutrition: ?";
					flavDisplayItem.text = "Flavor: ?";
					alcoDisplayItem.text = "Alcohol: ?";

					heldItemName.text = "Item Name";
					heldItemFacts.text = "Prepare yourself for amazing facts";
					heldItemCost.text = "Cost";
					heldItemWeight.text = "Weight";
					itemInfo.SetActive(false);
				}//fire1
					

			}//else
		
		}//raycast
				
	}//update()

	void cleanUpRotationIfFirst(){

		if (hasBeenRotated==false){
			hasBeenRotated=true;
			activeObject.transform.rotation = Quaternion.identity;
		}

	}
	
}//class
	