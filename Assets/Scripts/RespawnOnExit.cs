using UnityEngine;
using System.Collections;

public class RespawnOnExit : MonoBehaviour {

	public GameObject[] generateThese;
	public Transform generateAt;


	void Start(){

		RandomSpawn();

	}

	void OnTriggerExit(Collider col){
		
		ScoreValues scoreValues = col.transform.GetComponentInParent<ScoreValues>();

		if (scoreValues != null && scoreValues.removedYet == false){

			RandomSpawn();
			scoreValues.removedYet = true;

		}


	}

	void RandomSpawn(){

		int randObj = Random.Range(0, generateThese.Length);
		GameObject.Instantiate(generateThese[randObj], generateAt.position, generateAt.rotation);

	}
}
