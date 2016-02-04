using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	private AudioSource mySnd;

	void Start() {
		mySnd = GetComponent<AudioSource>();
	}

	// Use this for initialization
	public void BeginLevel1(){

		MousePlacer.levelNow = 0;
		Application.LoadLevel("Main");


	}

	public void MouseOverSound() {
		if(mySnd.isPlaying == false) {
			mySnd.Play();
		}
	}
	

}
