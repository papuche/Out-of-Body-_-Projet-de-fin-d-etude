using UnityEngine;
using System.Collections;

public class InitModel : MonoBehaviour {
	public Material jeanGhost;
	public Material shirtGhost;
	public GameObject posAvatar;
	GameObject goSrc;

	void Awake(){

		bool isMale = false;
		if (PlayerPrefs.GetInt("gender",1) != 0){
			isMale = true;
		}

		string name = PlayerPrefs.GetString ("Model");
		string[] model = name.Split(';');
		goSrc = (GameObject)Instantiate(Resources.Load(model[0]));
		GameObject goDst = (GameObject)Resources.Load (model [1]);
		PlayerPrefs.SetString ("ModelSRC", goSrc.name);
		PlayerPrefs.SetString ("ModelDST", goDst.name);
		goSrc.transform.parent = posAvatar.transform;
		goSrc.transform.localPosition = Vector3.zero;
		goSrc.transform.localRotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
		initAvatar ();
		initKinect ();

		GameObject jean = (GameObject) Instantiate(goSrc.transform.FindChild ("jeans01Mesh").gameObject);
		jean.name = "jeanGhost";
		jean.transform.parent = goSrc.transform;
		jean.GetComponent<Renderer> ().material = jeanGhost;
		jean.SetActive (false);
		GameObject shirt = (GameObject) Instantiate(goSrc.transform.FindChild ("shirt01Mesh").gameObject);
		shirt.name = "shirtGhost";
		shirt.transform.parent = goSrc.transform;
		shirt.GetComponent<Renderer> ().material = shirtGhost;
		shirt.SetActive (false);

		initMorphing("high-polyMesh");
		initMorphing("jeans01Mesh");
		initMorphing("shirt01Mesh");

		if(isMale){
			initMorphing("male1591Mesh");
		}else{
			initMorphing("female1605Mesh");
		}

		foreach (MorphingAvatar morph in goSrc.GetComponentsInChildren<MorphingAvatar>()) {
			Mesh meshSrc = morph.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
			Mesh meshDst = goDst.transform.FindChild(morph.gameObject.name).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
			morph.dstMesh = meshDst;
			morph.srcMesh = meshSrc;
		}

		goSrc.AddComponent<AvatarGhost> ();
		SetLayerRecursively (goSrc, 8);
		Debug.Log (name);
	}

	void initKinect(){
		//goSrc.transform.position = posAvatar.transform.position;
		//goSrc.transform.localRotation = posAvatar.transform.localRotation;
		AvatarControllerClassic ctrl = goSrc.AddComponent <AvatarControllerClassic>();
		
		ctrl.verticalMovement = true;
		GameObject modelRoot = goSrc.transform.FindChild ("python").gameObject;
		
		ctrl.HipCenter = modelRoot.transform.FindChild("Hips");
		ctrl.Spine = modelRoot.transform.FindChild ("Hips/Spine");
		ctrl.ShoulderCenter = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/Neck");
		ctrl.Neck = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/Neck/Head");
		//ctrl.Head = model.transform.FindChild("Head");
		
		ctrl.ClavicleLeft = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra");
		ctrl.ShoulderLeft = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm");
		ctrl.ElbowLeft = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm");
		ctrl.HandLeft = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand");
		
		
		ctrl.ClavicleRight = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra");
		ctrl.ShoulderRight = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm");
		ctrl.ElbowRight = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm");
		ctrl.HandRight = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand");
		
		
		ctrl.HipLeft = modelRoot.transform.FindChild("Hips/LeftUpLeg");
		ctrl.KneeLeft = modelRoot.transform.FindChild("Hips/LeftUpLeg/LeftLeg");
		ctrl.FootLeft = modelRoot.transform.FindChild("Hips/LeftUpLeg/LeftLeg/LeftFoot");
		
		
		ctrl.HipRight = modelRoot.transform.FindChild("Hips/RightUpLeg");
		ctrl.KneeRight = modelRoot.transform.FindChild("Hips/RightUpLeg/RightLeg");
		ctrl.FootRight = modelRoot.transform.FindChild("Hips/RightUpLeg/RightLeg/RightFoot");
		
		ctrl.BodyRoot = modelRoot.transform.FindChild("Hips");
		ctrl.OffsetNode = modelRoot;

		ctrl.Init ();
	}

	void initMorphing(string name){
		
		MorphingAvatar morph = goSrc.transform.FindChild (name).gameObject.AddComponent<MorphingAvatar> ();
		morph.speed = 0.016f;
		/*morph.dstMesh = modelDst.transform.FindChild (name).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		morph.srcMesh = modelSrc.transform.FindChild (name).gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh;*/
	}

	void initAvatar(){

		GameObject modelRoot = goSrc.transform.FindChild ("python").gameObject;

		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder").transform.localRotation = new Quaternion (-0.5f, 0.3f, 0.3f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra").transform.localRotation = new Quaternion (-0.6f, 0.3f, 0.2f, 0.7f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm").transform.localRotation = new Quaternion (-0.3f, 0.9f, 0.3f, -0.3f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm").transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand").transform.localRotation = new Quaternion (0.1f, -0.6f, 0.0f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftHandThumb1").transform.localRotation = new Quaternion (-0.1f, -0.5f, -0.4f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandIndex").transform.localRotation = new Quaternion (-0.1f, -0.2f, -0.1f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandMiddle").transform.localRotation = new Quaternion (-0.1f, 0.0f, 0.0f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandPinky").transform.localRotation = new Quaternion (-0.1f, -0.2f, 0.1f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/LeftShoulder/LeftShoulderExtra/LeftArm/LeftForeArm/LeftHand/LeftInHandRing").transform.localRotation = new Quaternion (-0.1f, -0.1f, 0.1f, 1.0f);

		modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder").transform.localRotation = new Quaternion (-0.5f, -0.3f, -0.3f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra").transform.localRotation = new Quaternion (-0.6f, -0.3f, -0.2f, 0.7f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm").transform.localRotation = new Quaternion (0.3f, 0.9f, 0.3f, 0.3f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm").transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand").transform.localRotation = new Quaternion (0.1f, 0.6f, 0.0f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightHandThumb1").transform.localRotation = new Quaternion (-0.1f, 0.5f, 0.4f, 0.8f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandIndex").transform.localRotation = new Quaternion (-0.1f, 0.2f, 0.1f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandMiddle").transform.localRotation = new Quaternion (-0.1f, 0.0f, 0.0f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandPinky").transform.localRotation = new Quaternion (-0.1f, 0.2f, -0.1f, 1.0f);
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder/RightShoulderExtra/RightArm/RightForeArm/RightHand/RightInHandRing").transform.localRotation = new Quaternion (-0.1f, 0.1f, 0.0f, 1.0f);
	}

	void SetLayerRecursively(GameObject go, int layerNumber) {
		if (go == null) return;
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)) {
			trans.gameObject.layer = layerNumber;
		}
	}
}
