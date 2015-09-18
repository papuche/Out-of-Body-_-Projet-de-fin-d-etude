using UnityEngine;
using System.Collections;

public class StickAnimation : MonoBehaviour {

	// Use this for initialization
	bool enabled;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.Z)) {
			enabled = !enabled;
			gameObject.GetComponentInChildren<MeshRenderer>().enabled = enabled;
			gameObject.GetComponent<Animator>().enabled = enabled;
		}
	}
}
