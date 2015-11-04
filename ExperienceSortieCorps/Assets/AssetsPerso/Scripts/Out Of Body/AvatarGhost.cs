using UnityEngine;
using System.Collections;

public class AvatarGhost : MonoBehaviour {
	/*void Start(){
		PlayerPrefs.DeleteKey(Utils.PREFS_GHOST);
	}

	void Update () {
		//if (Input.GetKeyDown (KeyCode.G)) {
		if(!PlayerPrefs.GetString(Utils.PREFS_GHOST).Equals("")) {
			PlayerPrefs.DeleteKey(Utils.PREFS_GHOST);
			active = !active;
			gameObject.transform.FindChild("shirtGhost").gameObject.SetActive(active);
			gameObject.transform.FindChild("jeanGhost").gameObject.SetActive(active);

		}
	}*/
	
	void Start(){
		if(PlayerPrefs.GetString(Utils.PREFS_GHOST).Equals(Utils.SOCKET_GHOST)) {
			PlayerPrefs.DeleteKey(Utils.PREFS_GHOST);
			gameObject.transform.FindChild("shirtGhost").gameObject.SetActive(true);
			gameObject.transform.FindChild("jeanGhost").gameObject.SetActive(true);
			
		}
	}
}
