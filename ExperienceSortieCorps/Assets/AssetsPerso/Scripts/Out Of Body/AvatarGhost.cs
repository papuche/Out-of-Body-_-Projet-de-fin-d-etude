using UnityEngine;
using System.Collections;

public class AvatarGhost : MonoBehaviour {
	bool active = false;
	

	void Update () {
		//if (Input.GetKeyDown (KeyCode.G)) {
		if(!PlayerPrefs.GetString(Utils.PREFS_GHOST).Equals("")) {
			PlayerPrefs.DeleteKey(Utils.PREFS_GHOST);
			active = !active;
			gameObject.transform.FindChild("shirtGhost").gameObject.SetActive(active);
			gameObject.transform.FindChild("jeanGhost").gameObject.SetActive(active);

		}
	}
}
