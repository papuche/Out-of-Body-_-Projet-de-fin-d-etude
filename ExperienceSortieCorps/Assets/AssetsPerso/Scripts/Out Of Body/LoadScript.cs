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
	private GameObject _camera;

	// Use this for initialization
	void Start () {
		_camera.SetActive (false);
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
		_camera.SetActive (true);
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

	public GameObject camera {
		get {
			return _camera;
		}
		set {
			_camera = value;
		}
	}
}
