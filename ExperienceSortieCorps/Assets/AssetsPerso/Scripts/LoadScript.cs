using UnityEngine;
using System.Collections;

public class LoadScript : MonoBehaviour {
	[SerializeField]
	private GameObject _initModel;
	[SerializeField]
	private GameObject _canvas;
	[SerializeField]
	private GameObject _baton;
	[SerializeField]
	private GameObject _launchMorphing;

	// Use this for initialization
	void Start () {
		int launchModel = PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_MODEL);
		if (launchModel == 0)
			_canvas.SetActive (true);
		else {
			_initModel.SetActive (true);
			if (PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_BATON) == 1)
				_baton.SetActive (true);
			else if (PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_MORPHING) == 1)
				_launchMorphing.SetActive (true);
		}
	}

	public GameObject initModel {
		get {
			return _initModel;
		}

		set {
			_initModel = value;
		}
	}

	public GameObject canvas {
		get {
			return _canvas;
		}
		
		set {
			_canvas = value;
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
