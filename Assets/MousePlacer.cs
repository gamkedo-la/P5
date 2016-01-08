using UnityEngine;
using System.Collections;

public class MousePlacer : MonoBehaviour {

	public GameObject placingObject;

	private GameObject activeObject;

	private int alternateShuffle = 0;
	public Vector3[] sideAdjustTranslation;

	// Use this for initialization
	void Start () {

		activeObject = GameObject.Instantiate(placingObject);
	
	}
	
	// Update is called once per frame
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rhInfo;

		if (Physics.Raycast(ray, out rhInfo)){ //out causes pass by reference

			Vector3 snapped = rhInfo.point;

			snapped.x = Mathf.RoundToInt(snapped.x);
			snapped.z = Mathf.RoundToInt(snapped.z);

			Debug.Log(snapped.x + " " + snapped.z);

			activeObject.transform.position = snapped;

			activeObject.transform.position += sideAdjustTranslation[alternateShuffle];

			if (Input.GetButtonDown("Fire1")){

				activeObject = (GameObject) GameObject.Instantiate(placingObject,
					activeObject.transform.position,
					activeObject.transform.rotation); //put down object

			}
				
			if (Input.GetButtonDown("Fire2")){

				activeObject.transform.Rotate(Vector3.up, -90.0f); //rotate object

				alternateShuffle++;
				if(alternateShuffle >= 4) {
					alternateShuffle -= 4;
				}
			}

		}
	
	}
}
