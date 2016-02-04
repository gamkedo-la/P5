using UnityEngine;
using System.Collections;

public class RespawnOnExit : MonoBehaviour {

	public GameObject[] generateThese;
	public Transform generateAt;
	public int[] itemLimits;

	void Start(){

		itemLimits = new int[generateThese.Length];

		for (int i = 0; i < itemLimits.Length; i++){

			itemLimits[i] = 2;

		}
			
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
		int safetyBreak = generateThese.Length;

		while (itemLimits[randObj]<=0){

			randObj++;
			randObj = randObj % generateThese.Length;
			safetyBreak--;

			if (safetyBreak < 0){
				return; 
			}
		}

		itemLimits[randObj]--;
		GameObject.Instantiate(generateThese[randObj], generateAt.position, generateAt.rotation);

	}
}
