using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class SelectModel : MonoBehaviour
{
	[SerializeField]
	private GameObject _posAvatar;

	private GameObject[] _go_models;
	private GameObject _avatar;

	private int _avatarIndex = 0;

	private int _gender;

	private float _offsetYPosAvatar = 0.68f;
	
	void Start ()
	{
		_gender = PlayerPrefs.GetInt (Utils.PREFS_AVATAR_GENDER); // 0: homme ; 1: femme

		_go_models = Resources.LoadAll<GameObject> (Utils.MODELS_DIRECTORY [_gender]);
		_avatar = (GameObject)Instantiate (_go_models [_avatarIndex]);
		_avatar.name = Utils.MODELS_DIRECTORY [_gender] + _go_models [_avatarIndex].name;

		_avatar.transform.parent = posAvatar.transform;
		applyOffsetY ();
		_avatar.transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
		initAvatar ();
	}
	
	void initAvatar ()
	{
		GameObject modelRoot = _avatar.transform.FindChild ("python").gameObject;
		
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
		
		modelRoot.transform.FindChild ("Hips/Spine/Spine1/Spine2/Spine3/RightShoulder").transform.localRotation = new Quaternion (-0.5f, -0.3f, -0.3f, 0.8f);
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
	
	void Update ()
	{
		_avatar.transform.Rotate (0.0f, 1.0f, 0.0f);
		if (Input.GetMouseButtonDown (0)) {
			_avatarIndex --;
			if (_avatarIndex < 0)
				_avatarIndex = _go_models.Length -1;
			ReloadAvatar();
		}
		else if (Input.GetMouseButtonDown (1)) {
			_avatarIndex ++;
			if (_avatarIndex >= _go_models.Length)
				_avatarIndex = 0;
			ReloadAvatar();
		}
		if (PlayerPrefs.GetString (Utils.PREFS_VALIDATE_AVATAR) != null) {
			int difference = int.Parse(PlayerPrefs.GetString (Utils.PREFS_VALIDATE_AVATAR));
			PlayerPrefs.DeleteKey(Utils.PREFS_VALIDATE_AVATAR);
			Validate(difference);
		}
	}

	void ReloadAvatar ()
	{
		Quaternion srcRotation = _avatar.transform.localRotation;
		Destroy (_avatar);
		_avatar = (GameObject)Instantiate (_go_models [_avatarIndex]);
		_avatar.name = Utils.MODELS_DIRECTORY [_gender] + _go_models [_avatarIndex].name;
		_avatar.transform.parent = posAvatar.transform;
		applyOffsetY ();
		_avatar.transform.localRotation = srcRotation;
		initAvatar ();
	}

	void Validate (int difference)
	{
		GameObject src = _go_models[_avatarIndex];
		GameObject dst = SelectOtherAvatar (src, difference);
		if (src != null && dst != null) {
			string models = Utils.MODELS_DIRECTORY [_gender] + src.name + ";" + Utils.MODELS_DIRECTORY [_gender] + dst.name;
			PlayerPrefs.SetString (Utils.PREFS_MODEL, models);
			//Destroy (_avatar);
			/*GameObject.Find ("Canvas").SetActive (false);
			initModel.SetActive (true);*/
			//SocketClient.GetInstance().Write(models);
			Application.LoadLevel(Utils.WAITING_SCENE);
		}
	}

	GameObject SelectOtherAvatar(GameObject avatar, int difference){
		string[] weight = {"HC", "LHC", "MC", "HLC", "LC"};
		string[] muscle = {"HM", "LHM", "MM", "HLM", "LM"};

		string suffixeAvatar = "";

		for (int i = 0; i < weight.Length; i++) {
			for (int j = 0; j < muscle.Length; j++) {
				if(avatar.name.Substring(avatar.name.LastIndexOf("ale") + 3).Equals(weight[i] + muscle[j])){
					if(i + difference > weight.Length-1)
						suffixeAvatar = weight[weight.Length-1];
					else 
						suffixeAvatar = weight[i + difference];
					if(j + difference > muscle.Length-1)
						suffixeAvatar += muscle[muscle.Length-1];
					else 
						suffixeAvatar += muscle[j + difference];
				}
			}
		}
		foreach (GameObject character in _go_models) {
			if(character.name.Substring(character.name.LastIndexOf("ale") + 3).Equals(suffixeAvatar))
				return character;
		}
		return null;
	}

	void applyOffsetY(){
		Vector3 vector = Vector3.zero;
		vector.y -= _offsetYPosAvatar;
		_avatar.transform.localPosition = vector;
	}

	public GameObject posAvatar {
		set {
			_posAvatar = value;
		}
		get {
			return _posAvatar;
		}
	}
}
