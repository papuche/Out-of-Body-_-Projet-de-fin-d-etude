using UnityEngine;
using System.Collections;

public class InitMorphing : MonoBehaviour {

	private GameObject _goSrc;

	// Use this for initialization
	void Start () {
		string name = PlayerPrefs.GetString ("Model");
		string[] model = name.Split(';');
		_goSrc = GameObject.Find(model[0].Split('/')[2]);
		GameObject goDst = (GameObject)Resources.Load (model [1]);

		bool isMale = false;
		if(model[0].Contains(Utils.MODELS_DIRECTORY[0])) {
			isMale = true;
		}
		init("high-polyMesh");
		init("jeans01Mesh");
		init("shirt01Mesh");
		
		if(isMale) {
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void init(string name){
		
		MorphingAvatar morph = _goSrc.transform.FindChild (name).gameObject.AddComponent<MorphingAvatar> ();
		morph.speed = 0.016f;
	}
}
