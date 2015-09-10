using UnityEngine;
using System.Collections;

public class CamOOB : MonoBehaviour {
	public float speed = 0.00002f;
	public LayerMask customMask;
	LayerMask defaultMask;
	Camera cam;
	Vector3 dst;
	bool init;
	// Use this for initialization


	void Start () {
		dst = new Vector3 (2.5f, transform.position.y, transform.position.z);
		cam = GetComponentInChildren<Camera> ();
		defaultMask = cam.cullingMask;
		init = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			init = true;
			//gameObject.transform.Translate (new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, -5.3f));
		}
		if (init) {
			cam.cullingMask = customMask;
			transform.position = (Vector3.Lerp (transform.position, dst, speed));
			Debug.Log(transform.position);
			if (transform.position.x > 0.0f) {
				//Debug.Log(transform.position.x);
				cam.cullingMask = defaultMask;
			}
		}
	}
}
