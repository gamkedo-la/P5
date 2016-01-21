using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class MousePlacer : MonoBehaviour {

//	public GameObject placingObject;

	private GameObject activeObject;
	private bool hasBeenRotated;
	int clickMask;

	private List<ScoreValues> atLeastPartlyInBasket;

	private int nutritionScore = 0;
	private int alcoholScore = 0;
	private int flavorScore = 0;

	public Text scoreDisplay;

//	private int alternateShuffle = 0;
//	public Vector3[] sideAdjustTranslation;

	// Use this for initialization
	void Start () {

	//	activeObject = GameObject.Instantiate(placingObject);
		clickMask = ~LayerMask.GetMask("Ignore Raycast"); //flip bitmask
		atLeastPartlyInBasket = new List<ScoreValues>();
	
	}

	public void EnteredBasket(ScoreValues obj){

		atLeastPartlyInBasket.Add(obj);

	}

	public void ExitBasket(ScoreValues obj){

		atLeastPartlyInBasket.Remove(obj);

	}

	public void UpdateScoring() {

		nutritionScore = 0;
		alcoholScore = 0;
		flavorScore = 0;

		foreach(ScoreValues oneObj in atLeastPartlyInBasket){

			if (oneObj.isAtLeastPartlyAboveBasket == false){

				nutritionScore += oneObj.nutrition;
				alcoholScore += oneObj.alcohol;
				flavorScore += oneObj.flavor;

			}

		}

		scoreDisplay.text = "Total Stats:" 
			+ "\nNutrition: " + nutritionScore 
			+ "\nAlcohol: " + alcoholScore 
			+ "\nFlavor: " + flavorScore;

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
				if (rhInfo.collider.gameObject.CompareTag("PicnicObject") && Input.GetButtonDown("Fire1")){
			
					Debug.Log(rhInfo.collider.name);

					hasBeenRotated = false;
					activeObject = rhInfo.collider.gameObject;
					activeObject.layer = LayerMask.NameToLayer("Ignore Raycast");
					activeObject.GetComponentInParent<Rigidbody>().useGravity = false;
					activeObject.GetComponentInParent<Rigidbody>().isKinematic = true;
					activeObject.GetComponent<Collider>().isTrigger = true;

					activeObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
					activeObject.GetComponentInParent<Rigidbody>().angularVelocity = Vector3.zero;

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

					activeObject.layer = LayerMask.NameToLayer("Default");
					activeObject.GetComponentInParent<Rigidbody>().useGravity = true;
					activeObject.GetComponentInParent<Rigidbody>().isKinematic = false;
					activeObject.GetComponent<Collider>().isTrigger = false;
					activeObject = null;

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
	