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
	
	[SerializeField]
	private GameObject _initModel;
	
	// Use this for initialization
	void Start () {
		_camera.gameObject.SetActive (false);
		if (PlayerPrefs.GetInt (Utils.PREFS_LAUNCH_MODEL) == 0) {
			_scene.AddComponent<SelectModel> ().posAvatar = _posAvatar;
		} else {
			_initModel.SetActive (true);
			switch (PlayerPrefs.GetInt (Utils.PREFS_CONDITION)) {
			case 1:
				break;
			case 2:
				EnableMorphing();
				break;
			case 3:
				EnableBaton();
				break;
			case 4:
				EnableMorphing();
				EnableBaton();
				break;
			}
		}
		_camera.gameObject.SetActive (true);
	}
	
	void EnableBaton(){
		_baton.SetActive (true);
	}
	
	void EnableMorphing(){
		InitMorphing initMorphing = _scene.AddComponent<InitMorphing> ();
		initMorphing.jeanGhost = _jeanGhost;
		initMorphing.shirtGhost = _shirtGhost;
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
	
	public GameObject initModel {
		set {
			_initModel = value;
		}
	}
}