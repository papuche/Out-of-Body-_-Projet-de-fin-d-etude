using UnityEngine;
using System.Collections;

public class InitMorphing : MonoBehaviour {

	private GameObject _goSrc;

	[SerializeField]
	private float _speed=0.016f;
	[SerializeField]
	private Material _jeanGhost;
	[SerializeField]
	private Material _shirtGhost;

	// Use this for initialization
	void Start () {
		string[] model = PlayerPrefs.GetString (Utils.PREFS_MODEL).Split(';');
		_goSrc = GameObject.Find(model[0].Split('/')[2]);
		GameObject goDst = (GameObject)Resources.Load (model [1]);

		GameObject jean = (GameObject) Instantiate(_goSrc.transform.FindChild ("jeans01Mesh").gameObject);
		jean.name = "jeanGhost";
		jean.transform.parent = _goSrc.transform;
		jean.GetComponent<Renderer> ().material = jeanGhost;
		jean.SetActive (false);
		GameObject shirt = (GameObject) Instantiate(_goSrc.transform.FindChild ("shirt01Mesh").gameObject);
		shirt.name = "shirtGhost";
		shirt.transform.parent = _goSrc.transform;
		shirt.GetComponent<Renderer> ().material = shirtGhost;
		shirt.SetActive (false);

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
		_goSrc.AddComponent<AvatarGhost> ();
		SetLayerRecursively (_goSrc, 8);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void init(string name){
		
		MorphingAvatar morph = _goSrc.transform.FindChild (name).gameObject.AddComponent<MorphingAvatar> ();
		morph.speed = speed;
	}

	void SetLayerRecursively(GameObject go, int layerNumber) {
		if (go == null) return;
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)) {
			trans.gameObject.layer = layerNumber;
		}
	}

	public float speed {
		get {
			return _speed;
		}
		
		set {
			_speed = value;
		}
	}

	public Material jeanGhost {
		get {
			return _jeanGhost;
		}
		
		set {
			_jeanGhost = value;
		}
	}

	public Material shirtGhost {
		get {
			return _shirtGhost;
		}
		
		set {
			_shirtGhost = value;
		}
	}
}
