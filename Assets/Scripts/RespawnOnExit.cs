using UnityEngine;
using System.Collections;

public class RespawnOnExit : MonoBehaviour {

	public GameObject[] generateThese;
	public Transform generateAt;

	//private int debt = 0;

	void OnTriggerEnter(Collider col){

	//	debt++;

	}

	void OnTriggerExit(Collider col){

		//debt--;

		//if (debt < 0){
		int randObj = Random.Range(0, generateThese.Length);
		GameObject.Instantiate(generateThese[randObj], generateAt.position, generateAt.rotation);

		//}


	}
}
