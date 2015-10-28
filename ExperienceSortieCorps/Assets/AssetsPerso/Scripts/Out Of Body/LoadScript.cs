using UnityEngine;
using System.Collections;

public class LoadScript : MonoBehaviour {
	[SerializeField]
	private GameObject _initModel;
	[SerializeField]
	private GameObject _selectModel;
	[SerializeField]
	private GameObject _baton;
	[SerializeField]
	private GameObject _launchMorphing;

	public GameObject camera;

	// Use this for initialization
	void Start () {
		camera.SetActive (false);
		int launchModel = PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_MODEL);
		if (launchModel == 0) {
			_selectModel.SetActive (true);
			//_baton.SetActive (true);
		} else {
			_initModel.SetActive (true);
			/*if (PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_BATON) == 1)
				_baton.SetActive (true);
			else if (PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_MORPHING) == 1)
				_launchMorphing.SetActive (true);
		}*/
			string parameter = PlayerPrefs.GetString (Utils.PREFS_OUTOFBODY);
			if (parameter.Equals (Utils.MORPHING_PARAMETER)) {
				_launchMorphing.SetActive (true);
			} else if (parameter.Equals (Utils.STICK_PARAMETER)) {
				_baton.SetActive (true);
			} else if (parameter.Equals (Utils.ALL_PARAMETERS)) {
				_launchMorphing.SetActive (true);
				_baton.SetActive (true);
			}
		}
		camera.SetActive (true);
	}

	public GameObject initModel {
		get {
			return _initModel;
		}

		set {
			_initModel = value;
		}
	}

	public GameObject selectModel {
		get {
			return _selectModel;
		}
		
		set {
			_selectModel = value;
		}
	}

	public GameObject baton {
		get {
			return _baton;
		}
		set {
			_baton = value;
		}
	}

	public GameObject launchMorphing {
		get {
			return _launchMorphing;
		}
		set {
			_launchMorphing = value;
		}
	}
}
