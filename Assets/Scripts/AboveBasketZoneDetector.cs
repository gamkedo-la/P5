using UnityEngine;
using System.Collections;

public class AboveBasketZoneDetector : MonoBehaviour {

	public MousePlacer mousePlacer;

	// Use this for initialization
	void OnTriggerEnter(Collider col){

		if(col.gameObject.CompareTag("PicnicObject")){

			col.GetComponentInParent<ScoreValues>().isAtLeastPartlyAboveBasket = true;
			mousePlacer.UpdateScoring();

		}

	}

	void OnTriggerExit(Collider col){

		if(col.gameObject.CompareTag("PicnicObject")){

			col.GetComponentInParent<ScoreValues>().isAtLeastPartlyAboveBasket = false;
			mousePlacer.UpdateScoring();

		}

	}
}
