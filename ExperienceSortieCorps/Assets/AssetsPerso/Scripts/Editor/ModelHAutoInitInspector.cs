using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(ModelHAutoInit))]
public class ModelHAutoInitInspector : Editor {
	GameObject modelSrc;
	GameObject modelDst;
	GameObject model;
	/*GameObject modelSrc;
	GameObject modelDst;*/
	public override void OnInspectorGUI () {
		model = GameObject.FindObjectOfType<ModelHAutoInit>().gameObject;
		modelSrc = GameObject.Find("Src").transform.GetChild(0).gameObject;
		modelDst = GameObject.Find("Dst").transform.GetChild(0).gameObject;
		
		
		if(GUILayout.Button("Initialize Model")){
			initKinect();
			
			initMorphing("mhair02Mesh");
			initMorphing("high-polyMesh");
			initMorphing("jeans01Mesh");
			initMorphing("male1591Mesh");
			initMorphing("shirt01Mesh");
			//initMorphing("shoes01_hresMesh");
		}
		
	}
	
	void initKinect(){
		AvatarControllerClassic ctrl = model.AddComponent <AvatarControllerClassic>();
		
		ctrl.verticalMovement = true;
		GameObject modelRoot = model.transform.FindChild ("python").gameObject;
		
		ctrl.HipCenter = modelRoot.transform.FindChild("Hips");
		ctrl.Spine = modelRoot.transform.FindChild ("Hips/Spine");
		ctrl.ShoulderCenter = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3");
		ctrl.Neck = modelRoot.transform.FindChild("Hips/Spine/Spine1/Spine2/Spine3/Neck");
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
	}
	
	void initMorphing(string name){
		
		MorphingAvatar morph = model.transform.FindChild (name).gameObject.AddComponent<MorphingAvatar> ();
		morph.speed = 0.05f;
		morph.dstMesh = modelDst.transform.FindChild (name).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		morph.srcMesh = modelSrc.transform.FindChild (name).gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh;
	}
	
}
