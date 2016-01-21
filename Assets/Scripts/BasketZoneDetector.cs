using UnityEngine;
using System.Collections;

public class BasketZoneDetector : MonoBehaviour {

	public MousePlacer mousePlacer;

	// Use this for initialization
	void OnTriggerEnter(Collider col){

		if(col.gameObject.CompareTag("PicnicObject")){

			col.GetComponentInParent<ScoreValues>().isAtLeastPartlyInBasket = true;
			mousePlacer.EnteredBasket(col.GetComponentInParent<ScoreValues>());
			mousePlacer.UpdateScoring();
		}

	}

	void OnTriggerExit(Collider col){

		if(col.gameObject.CompareTag("PicnicObject")){

			col.GetComponentInParent<ScoreValues>().isAtLeastPartlyInBasket = false;
			mousePlacer.ExitBasket(col.GetComponentInParent<ScoreValues>());
			mousePlacer.UpdateScoring();

		}

	}
}
