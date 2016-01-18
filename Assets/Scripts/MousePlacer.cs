using UnityEngine;
using System.Collections;

public class MousePlacer : MonoBehaviour {

//	public GameObject placingObject;

	private GameObject activeObject;
	int clickMask;

//	private int alternateShuffle = 0;
//	public Vector3[] sideAdjustTranslation;

	// Use this for initialization
	void Start () {

	//	activeObject = GameObject.Instantiate(placingObject);
		clickMask = ~LayerMask.GetMask("Ignore Raycast"); //flip bitmask 
	
	}
	
	// Update is called once per frame
	void Update () {

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

					activeObject.transform.Rotate(90,0,0);

				}
				if (Input.GetKeyDown(KeyCode.S)){

					activeObject.transform.Rotate(0,90,0);

				}
				if (Input.GetKeyDown(KeyCode.D)){

					activeObject.transform.Rotate(0,0,90);

				}

				//until you release the mouse button
				if (Input.GetButtonUp("Fire1")){

					activeObject.layer = LayerMask.NameToLayer("Default");
					activeObject.GetComponentInParent<Rigidbody>().useGravity = true;
					activeObject.GetComponentInParent<Rigidbody>().isKinematic = false;
					activeObject.GetComponent<Collider>().isTrigger = false;
					activeObject = null;

				}
					

			}
			
				




		
			//	carryPoint += Vector3.up * 1.5f;

		
			}
				
//
//			activeObject.transform.position += sideAdjustTranslation[alternateShuffle];

//			if (Input.GetButtonDown("Fire1")){
//
//				activeObject.GetComponent<Rigidbody>().isKinematic = false;
//				activeObject.GetComponentInChildren<Collider>().isTrigger = false;
//
//				activeObject = (GameObject) GameObject.Instantiate(placingObject,
//					activeObject.transform.position,
//					activeObject.transform.rotation); //put down object
//
//			}
//				
//			if (Input.GetButtonDown("Fire2")){
//
//				activeObject.transform.Rotate(Vector3.up, -90.0f); //rotate object
//
////				alternateShuffle++;
////				if(alternateShuffle >= 4) {
////					alternateShuffle -= 4;
////				}
//			}

		}
	
	}
	