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
	[SerializeField]
	private Camera _camera;

	// Use this for initialization
	void Start () {
		_camera.gameObject.SetActive (false);
		Debug.Log (_camera);
		int launchModel = PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_MODEL);
		if (launchModel == 0) {
			_selectModel.SetActive (true);
		} else {
			_initModel.SetActive (true);
			if (PlayerPrefs.GetInt(Utils.PREFS_BATON) == 1)
				_baton.SetActive (true);
			if (PlayerPrefs.GetInt(Utils.PREFS_MORPHING) == 1)
				_launchMorphing.SetActive (true);
		}
		_camera.gameObject.SetActive (true);
	}

	public GameObject initModel {
		set {
			_initModel = value;
		}
	}

	public GameObject selectModel {
		set {
			_selectModel = value;
		}
	}

	public GameObject baton {
		set {
			_baton = value;
		}
	}

	public GameObject launchMorphing {
		set {
			_launchMorphing = value;
		}
	}

	public Camera camera {
		set {
			_camera = value;
		}
	}
}