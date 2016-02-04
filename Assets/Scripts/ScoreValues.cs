using UnityEngine;
using System.Collections;


public class ScoreValues : MonoBehaviour {

	public int nutrition = -100;
	public int alcohol = -100;
	public int flavor = -100;

	public int itemCost = 3;//in dollars
	public int itemWeight = 1000; //in grams  

	public bool removedYet = false; //first time out of the basket to solve respawning bug
	public bool required = false;

	public string funFacts = "";

	public bool isAtLeastPartlyInBasket = false;
	public bool isAtLeastPartlyAboveBasket = false;

	static AudioClip bumpSnd = null;

	void Start() {
		if(bumpSnd == null) {
			bumpSnd = Resources.Load("bump1") as AudioClip;
		}
	}

	void OnCollisionEnter(Collision bumpFacts) {
		if(bumpSnd == null) {
			Debug.Log("bump sound not found");
			return;
		}
		// Debug.Log(bumpFacts.relativeVelocity.magnitude);
		float hitSpeed = bumpFacts.relativeVelocity.magnitude;
		if(hitSpeed < 1.0f) {
			return;
		}
		float volScale = hitSpeed / 3.0f;

		GameObject tempGO = new GameObject("TempAudio"); // create the temp object
		tempGO.transform.position = Camera.main.transform.position; // set its position
		AudioSource aSource = tempGO.AddComponent<AudioSource>() as AudioSource; // add an audio source
		aSource.clip = bumpSnd; // define the clip
		aSource.volume = Random.Range(0.5f,1.0f)*volScale;
		aSource.pitch = Random.Range(0.5f,2.0f);
		aSource.Play(); // start the sound
		Destroy(tempGO, bumpSnd.length/aSource.pitch); // destroy object after clip duration
	}
}
