﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Script launched when the user has to select an avatar
/// </summary>
public class SelectModel : MonoBehaviour
{
	/// <summary>
	/// Refers to the positionAvatar Gameobject of the scene
	/// </summary>
	[SerializeField]
	private GameObject _posAvatar;

	/// <summary>
	/// An array containing all the models ( = avatars )
	/// </summary>
	private GameObject[] _go_models;

	/// <summary>
	/// The model shown in the scene
	/// </summary>
	private GameObject _avatar;

	/// <summary>
	/// The index of the avatar in the avatar list
	/// </summary>
	private int _avatarIndex = 0;

	/// <summary>
	/// The _gender chosen by the user
	/// </summary>
	private int _gender;

	private float _offsetYPosAvatar = 0.68f;
	
	void Start ()
	{
		_gender = PlayerPrefs.GetInt (Utils.PREFS_AVATAR_GENDER); // 0: man ; 1: woman

		_go_models = Resources.LoadAll<GameObject> (Utils.MODELS_DIRECTORY [_gender]);
		_avatar = (GameObject)Instantiate (_go_models [_avatarIndex]);
		_avatar.name = Utils.MODELS_DIRECTORY [_gender] + _go_models [_avatarIndex].name;

		_avatar.transform.parent = posAvatar.transform;
		applyOffsetY ();
		_avatar.transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
		initAvatar ();
	}

	/// <summary>
	/// Initialize the avatar
	/// </summary>
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
		if (Input.GetMouseButtonDown (0)) {	// If right button of the mouse is pressed
			_avatarIndex --;
			if (_avatarIndex < 0)
				_avatarIndex = _go_models.Length -1;
			ReloadAvatar();
		}
		else if (Input.GetMouseButtonDown (1)) {	// If left button of the mouse is pressed
			_avatarIndex ++;
			if (_avatarIndex >= _go_models.Length)
				_avatarIndex = 0;
			ReloadAvatar();
		}
		if (!string.Empty.Equals(PlayerPrefs.GetString (Utils.PREFS_VALIDATE_AVATAR))) {	// If the user validates the current avatar in the Web interface
			int difference = int.Parse(PlayerPrefs.GetString (Utils.PREFS_VALIDATE_AVATAR));
			PlayerPrefs.DeleteKey(Utils.PREFS_VALIDATE_AVATAR);
			Validate(difference);
		}
	}

	/// <summary>
	/// Delete the current avatar in the scene and instantiate a new avatar
	/// </summary>
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

	/// <summary>
	/// Validate the selected avatar
	/// </summary>
	/// <param name="difference">The difference between the chosen avatar and the one chosen by the experimentator</param>
	void Validate (int difference)
	{
		GameObject src = _go_models[_avatarIndex];
		GameObject dst = SelectOtherAvatar (src, difference);
		if (src != null && dst != null) {
			CreateNewDirectory();
			string models = Utils.MODELS_DIRECTORY [_gender] + src.name + ";" + Utils.MODELS_DIRECTORY [_gender] + dst.name;
			PlayerPrefs.SetString (Utils.PREFS_MODEL, models);
			PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 1);
			Application.LoadLevel(Utils.WAITING_SCENE);
		}
	}

	/// <summary>
	/// Create the directory which will contains the user's files
	/// </summary>
	void CreateNewDirectory(){
		if (!Directory.Exists (FilesConst.SAVE_FILES_DIRECTORY)) {	// Si le répertoire contenant les résultats n'existent pas
			Directory.CreateDirectory (FilesConst.SAVE_FILES_DIRECTORY);	// On le crée
		}
		int dirIndex = 0;
		foreach (string directory in Directory.GetDirectories(FilesConst.SAVE_FILES_DIRECTORY)) {
			string dir = directory.Remove (0, FilesConst.SAVE_FILES_DIRECTORY.Length + 1);
			if (dir.Contains (FilesConst.USER_PREFIX_DIRECTORY) && int.Parse (dir.Remove (0, FilesConst.USER_PREFIX_DIRECTORY.Length).Split('_')[0]) > dirIndex)
				dirIndex = int.Parse (dir.Remove (0, FilesConst.USER_PREFIX_DIRECTORY.Length).Split('_')[0]);
		}
		string time = System.DateTime.Now.ToString ().Replace ("/", "-").Replace (":", "-");
		PlayerPrefs.SetString (Utils.PREFS_PATH_FOLDER, Directory.CreateDirectory (FilesConst.SAVE_FILES_DIRECTORY + "/" + FilesConst.USER_PREFIX_DIRECTORY + (dirIndex + 1).ToString () + "_" + time).FullName);
	}

	/// <summary>
	/// Selects the experimentator's avatar, taking care with the difference between the two models
	/// </summary>
	/// <returns>The experimentator's avatar</returns>
	/// <param name="avatar">The user's avatar</param>
	/// <param name="difference">The difference between the two models</param>
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
