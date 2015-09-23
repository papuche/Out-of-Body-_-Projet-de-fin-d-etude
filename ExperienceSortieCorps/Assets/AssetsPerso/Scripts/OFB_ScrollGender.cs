using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OFB_ScrollGender : MonoBehaviour {
	
	[SerializeField]
	private Slider _slider;
	
	public void Slider_Change() {		
		PlayerPrefs.SetInt ("gender", (int)_slider.value);
	}
	
	public Slider Slider {
		set {
			_slider = value;
		}
	}

    void Start() {		
		PlayerPrefs.SetInt ("gender", 0);
	}
}
