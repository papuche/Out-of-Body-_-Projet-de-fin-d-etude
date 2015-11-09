using UnityEngine;
using System.Collections;

public class AvatarGhost : MonoBehaviour {

	void Start(){
		if (PlayerPrefs.GetInt(Utils.PREFS_GHOST) == 1) {
			gameObject.transform.FindChild("shirtGhost").gameObject.SetActive(true);
			gameObject.transform.FindChild("jeanGhost").gameObject.SetActive(true);
		}
	}
}
