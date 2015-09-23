using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OFB_ScrollUser : MonoBehaviour {

	[SerializeField]
	private Slider _slider;

	public void Slider_Change() {		
		PlayerPrefs.SetInt ("user", (int)_slider.value);
	}

	public Slider Slider {
		set {
			_slider = value;
		}
	}

	void Start() {		
		PlayerPrefs.SetInt ("user", 0);
	}
}
