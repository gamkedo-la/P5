using UnityEngine;
using System.Collections;

public class CandleFlicker : MonoBehaviour {
	public float phaseOffset = 0.0f;
	public float phaseMult = 1.0f;
	private Light myLight;
	// Use this for initialization
	void Start () {
		myLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		myLight.intensity = Random.Range(3.4f, 3.7f) + Mathf.Cos(Time.time*phaseMult+phaseOffset);
		myLight.range = Random.Range(9.5f, 10.5f) + Mathf.Cos(Time.time*phaseMult+phaseOffset);
	}
}
