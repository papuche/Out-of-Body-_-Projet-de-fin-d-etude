using UnityEngine;
using System.Collections;

public class AvatarGhost : MonoBehaviour {
	bool active;
	// Use this for initialization
	void Start () {
		active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G)) {
			if(active){
				gameObject.transform.FindChild("shirtGhost").gameObject.SetActive(false);
				gameObject.transform.FindChild("jeanGhost").gameObject.SetActive(false);
				active = false;
			}else{
				gameObject.transform.FindChild("shirtGhost").gameObject.SetActive(true);
				gameObject.transform.FindChild("jeanGhost").gameObject.SetActive(true);
				active = true;
			}
		}
	}
}
