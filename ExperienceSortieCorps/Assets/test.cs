using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class test : MonoBehaviour {

	public RenderTexture tex;
	public GameObject canvas;

	// Use this for initialization
	void Start () {
		RenderTexture t = new RenderTexture(256,256,24);
		gameObject.GetComponent<Camera> ().targetTexture = t;
		canvas.GetComponent<RawImage> ().texture = t;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
