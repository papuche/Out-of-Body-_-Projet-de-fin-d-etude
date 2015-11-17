using UnityEngine;
using System.Collections;

public class InitMorphing : MonoBehaviour {
	
	[SerializeField]
	private Material _jeanGhost;
	[SerializeField]
	private Material _shirtGhost;

	private float _speed = 0.016f;

	private GameObject _goSrc;

	private GameObject _jean;
	private GameObject _shirt;

	void Start () {
		string[] model = PlayerPrefs.GetString (Utils.PREFS_MODEL).Split(';');
		_goSrc = GameObject.Find(model[0].Split('/')[2]);
		GameObject goDst = (GameObject)Resources.Load (model [1]);

		_jean = (GameObject) Instantiate(_goSrc.transform.FindChild ("jeans01Mesh").gameObject);
		_jean.name = "jeanGhost";
		_jean.transform.parent = _goSrc.transform;
		_jean.GetComponent<Renderer> ().material = _jeanGhost;

		_shirt = (GameObject) Instantiate(_goSrc.transform.FindChild ("shirt01Mesh").gameObject);
		_shirt.name = "shirtGhost";
		_shirt.transform.parent = _goSrc.transform;
		_shirt.GetComponent<Renderer> ().material = _shirtGhost;


		/**
		 * Gestion du ghost
		 **/ 
		if (PlayerPrefs.GetInt (Utils.PREFS_GHOST) == 1) {
			_jean.SetActive (true);
			_shirt.SetActive (true);
		} else {
			_jean.SetActive (false);
			_shirt.SetActive (false);
		}

		init("high-polyMesh");
		init("jeans01Mesh");
		init("shirt01Mesh");
		
		if(model[0].Contains(Utils.MODELS_DIRECTORY[0])) {
			init("male1591Mesh");
		}else {
			init("female1605Mesh");
		}

		foreach (MorphingAvatar morph in _goSrc.GetComponentsInChildren<MorphingAvatar>()) {
			Mesh meshSrc = morph.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
			Mesh meshDst = goDst.transform.FindChild(morph.gameObject.name).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
			morph.dstMesh = meshDst;
			morph.srcMesh = meshSrc;
		}
		
		SetLayerRecursively (_goSrc, 8);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void init(string name){
		
		MorphingAvatar morph = _goSrc.transform.FindChild (name).gameObject.AddComponent<MorphingAvatar> ();
		morph.speed = _speed;
	}

	void SetLayerRecursively(GameObject go, int layerNumber) {
		if (go == null) return;
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)) {
			trans.gameObject.layer = layerNumber;
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
