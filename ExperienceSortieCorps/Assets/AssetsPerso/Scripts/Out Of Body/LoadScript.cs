using UnityEngine;
using System.Collections;

public class LoadScript : MonoBehaviour {

	[SerializeField]
	private GameObject _baton;
	[SerializeField]
	private Camera _camera;
	[SerializeField]
	private GameObject _scene;
	[SerializeField]
	private GameObject _posAvatar;
	[SerializeField]
	private Material _jeanGhost;
	[SerializeField]
	private Material _shirtGhost; 

	// Use this for initialization
	void Start () {
		_camera.gameObject.SetActive (false);
		int launchModel = PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_MODEL);
		if (launchModel == 0) {
			//_selectModel.SetActive (true);
			_scene.AddComponent<SelectModel> ().posAvatar = _posAvatar;
		} else {
			_scene.AddComponent<InitModel> ().posAvatar = _posAvatar;
			//_initModel.SetActive (true);
			if (PlayerPrefs.GetInt(Utils.PREFS_BATON) == 1)
				_baton.SetActive (true);
			if (PlayerPrefs.GetInt(Utils.PREFS_MORPHING) == 1) {
				//_launchMorphing.SetActive (true);
				InitMorphing initMorphing = _scene.AddComponent<InitMorphing>();
				initMorphing.jeanGhost = _jeanGhost;
				initMorphing.shirtGhost = _shirtGhost;
			}
		}
		_camera.gameObject.SetActive (true);
	}

	public GameObject baton {
		set {
			_baton = value;
		}
	}

	public Camera camera {
		set {
			_camera = value;
		}
	}

	public GameObject scene {
		set {
			_scene = value;
		}
	}
	public GameObject posAvatar {
		set {
			_posAvatar = value;
		}
	}

	public Material jeanGhost {
		set {
			_jeanGhost = value;
		}
	}
	
	public Material shirtGhost {
		set {
			_shirtGhost = value;
		}
	}
}