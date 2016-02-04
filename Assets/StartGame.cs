using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	public void BeginLevel1(){

		MousePlacer.levelNow = 0;
		Application.LoadLevel("Main");


	}
	

}
