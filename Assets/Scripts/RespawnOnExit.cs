using UnityEngine;
using System.Collections;

public class RespawnOnExit : MonoBehaviour {

	public GameObject generateThis;
	public Transform generateAt;

	//private int debt = 0;

	void OnTriggerEnter(Collider col){

	//	debt++;

	}

	void OnTriggerExit(Collider col){

		//debt--;

		//if (debt < 0){
			
			GameObject.Instantiate(generateThis, generateAt.position, generateAt.rotation);

		//}


	}
}
