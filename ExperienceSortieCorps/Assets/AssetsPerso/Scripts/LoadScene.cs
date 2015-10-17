using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
public class LoadScene : MonoBehaviour {
	
	public ToggleGroup toggleGroupSrc;
	public ToggleGroup toggleGroupDst;
	string model;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Load(){
		foreach (Toggle t in toggleGroupSrc.ActiveToggles()) {
			if (t.isOn) {
				model = "Femme/" + t.GetComponent<ToggleModel> ().model + ";";
				break;
			}
		}
		foreach (Toggle t in toggleGroupDst.ActiveToggles()) {
			if (t.isOn) {
				model += "Femme/" + t.GetComponent<ToggleModel> ().model;
				break;
			}
		}
		PlayerPrefs.SetString (Utils.PREFS_MODEL,model);
		PlayerPrefs.SetInt("gender",1);
		PlayerPrefs.SetInt (Utils.PREFS_SESSION, 1);
		PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 0);
		PlayerPrefs.SetInt (Utils.PREFS_SUJET, PlayerPrefs.GetInt(Utils.PREFS_SUJET,-1) + 1);
		//Application.LoadLevel (1);
		Process foo = new Process();
		foo.StartInfo.FileName = "evalApp_DirectToRift.exe";
		foo.Start ();
		Application.Quit ();
	}
}
