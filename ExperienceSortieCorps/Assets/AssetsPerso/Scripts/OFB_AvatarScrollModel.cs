using UnityEngine;
using System.Collections;

public class OFB_AvatarScrollModel : MonoBehaviour {

	public void Decrement () {
		int avatarIndex = 0;
		if (PlayerPrefs.GetInt ("avatarIndex") != null) { 
			avatarIndex = PlayerPrefs.GetInt ("avatarIndex");
		}
		avatarIndex --;
		if (avatarIndex < 0)
			avatarIndex = Resources.LoadAll<GameObject> ("Models/Homme/").Length -1;
		PlayerPrefs.SetInt ("avatarIndex", avatarIndex);
		}

	
	public void Increment () {
		int avatarIndex = 0;
		if (PlayerPrefs.GetInt ("avatarIndex") != null) { 
			avatarIndex = PlayerPrefs.GetInt ("avatarIndex");
		}
		avatarIndex ++;
		if (avatarIndex >= Resources.LoadAll<GameObject> ("Models/Homme/").Length -1 )
			avatarIndex = 0;
		PlayerPrefs.SetInt ("avatarIndex", avatarIndex);
	}
}
