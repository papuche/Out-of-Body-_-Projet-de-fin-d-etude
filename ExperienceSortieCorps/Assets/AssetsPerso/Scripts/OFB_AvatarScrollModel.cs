using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OFB_AvatarScrollModel : MonoBehaviour {
	[SerializeField]
	private Slider _sliderGender;

	public void Decrement () {
		int avatarIndex = 0;
		if (PlayerPrefs.GetInt ("avatarIndex") != null) { 
			avatarIndex = PlayerPrefs.GetInt ("avatarIndex");
		}
		avatarIndex --;
		if (avatarIndex < 0)
			avatarIndex = Resources.LoadAll<GameObject> (SelectModel._modelsDirectory[(int)_sliderGender.value]).Length -1;
		PlayerPrefs.SetInt ("avatarIndex", avatarIndex);
		}

	
	public void Increment () {
		int avatarIndex = 0;
		if (PlayerPrefs.GetInt ("avatarIndex") != null) { 
			avatarIndex = PlayerPrefs.GetInt ("avatarIndex");
		}
		avatarIndex ++;
		if (avatarIndex >= Resources.LoadAll<GameObject> (SelectModel._modelsDirectory[(int)_sliderGender.value]).Length -1 )
			avatarIndex = 0;
		PlayerPrefs.SetInt ("avatarIndex", avatarIndex);
	}

	public Slider SliderGender {
		get {
			return _sliderGender;
		}
		set {
			_sliderGender = value;
		}
	}
}
