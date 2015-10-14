using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectModel : MonoBehaviour
{
	[SerializeField]
	private GameObject _posAvatar;
	[SerializeField]
	private Slider _sliderGender;
	[SerializeField]
	private Slider _sliderUser;
	[SerializeField]
	private Button _validateButton;

	private string[] _chosenAvatar;
	private GameObject[] _go_models;
	private GameObject _avatar;
	private int _avatarIndex;
	private int _gender = 0;	// 0 : homme, 1 : femme
	private int _user = 0;		// 0 : utilisateur, 1 : expérimentateur 

	private float _offsetYPosAvatar = 0.68f;
	
	void Start ()
	{
		/* Gestion des listeners */
		_validateButton.onClick.AddListener (() => Validate ());
		_sliderGender.onValueChanged.AddListener (delegate {
			ChangeGender ();
		});
		_sliderUser.onValueChanged.AddListener (delegate {
			ChangeUser ();
		});

		_avatarIndex = PlayerPrefs.GetInt (Utils.PREFS_AVATAR_INDEX);
		_chosenAvatar = new string[2];

		_go_models = Resources.LoadAll<GameObject> (Utils.MODELS_DIRECTORY [_gender]);
		_avatar = (GameObject)Instantiate (_go_models [_avatarIndex]);
		_avatar.name = Utils.MODELS_DIRECTORY [(int)_sliderUser.value] + _go_models [_avatarIndex].name;
		_chosenAvatar [0] = _avatar.name;
		_chosenAvatar [1] = _avatar.name;

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
		ChangeAvatar ();	// Gestion des flèches pour le changement d'avatar
		_avatar.transform.Rotate (0.0f, 1.0f, 0.0f);
	}

	/// <summary>
	/// Supprime l'avatar instancié et en instancie un nouveau
	/// </summary>
	void ReloadAvatar ()
	{
		Quaternion srcRotation = _avatar.transform.localRotation;
		Destroy (_avatar);
		_avatar = (GameObject)Instantiate (_go_models [_avatarIndex]);
		_avatar.name = Utils.MODELS_DIRECTORY [(int)_sliderGender.value] + _go_models [_avatarIndex].name;
		_avatar.transform.parent = posAvatar.transform;
		applyOffsetY ();
		_avatar.transform.localRotation = srcRotation;
		initAvatar ();
		_chosenAvatar [_user] = _avatar.name;
	}

	void ChangeGender ()
	{
		if ((int)_sliderGender.value != _gender) {	// Si le genre de l'avatar change
			_gender = (int)_sliderGender.value;	// On sauvegarde le genre de l'avatar
			_go_models = Resources.LoadAll<GameObject> (Utils.MODELS_DIRECTORY [_gender]);	// On charge les modèles d'avatar correspondant au sexe
			ReloadAvatar ();								// On charge le nouvel avatar
		}
	}

	void ChangeUser ()
	{
		if ((int)sliderUser.value != _user) {	// Si on change l'utilisateur
			_user = (int)sliderUser.value;			// On sauvegarde l'utilisateur
			int oldUser = (_user == 0) ? 1 : 0;
			_chosenAvatar [oldUser] = _avatar.name;			// On sauvegarde le modele choisi par l'ancien utilisateur
			if (!_chosenAvatar [_user].Contains (Utils.MODELS_DIRECTORY [(int)_sliderGender.value])) {
				int gender = (int)_sliderGender.value == 0 ? 1 : 0;
				_go_models = Resources.LoadAll<GameObject> (Utils.MODELS_DIRECTORY [gender]);	// On charge les modèles d'avatar correspondant au sexe
				if (_chosenAvatar [_user] != null) {
					for (int i=0; i< _go_models.Length; i++) {
						if (_chosenAvatar [_user].Contains (_go_models [i].name)) {
							PlayerPrefs.SetInt (Utils.PREFS_AVATAR_INDEX, i);	// On charge l'avatar choisi anciennement par l'utilisateur
							break;
						}
					}
				}
				_sliderGender.value = gender;	// Change la position du slider qui gere le sexe et declenche le listener associé
			}
			else if (_chosenAvatar [_user] != null) {
				for (int i=0; i< _go_models.Length; i++) {
					if (_chosenAvatar [_user].Contains (_go_models [i].name)) {
						PlayerPrefs.SetInt (Utils.PREFS_AVATAR_INDEX, i);	// On charge l'avatar choisi anciennement par l'utilisateur
						break;
					}
				}
			}
		}
	}
	
	void ChangeAvatar ()
	{
		if (_avatarIndex != PlayerPrefs.GetInt (Utils.PREFS_AVATAR_INDEX)) {		// Si on change d'avatar
			_avatarIndex = PlayerPrefs.GetInt (Utils.PREFS_AVATAR_INDEX);			// On sauvegarde l'index de l'avatar
			ReloadAvatar ();
		}
	}

	void Validate ()
	{
		int avatarGender = 0;
		for (int i=0; i<Utils.MODELS_DIRECTORY.Length; i++) {
			if (_chosenAvatar [0].Contains (Utils.MODELS_DIRECTORY [i])) {
				avatarGender = i;
				break;
			}
		}
		if (_chosenAvatar [1].Contains (Utils.MODELS_DIRECTORY [avatarGender])) {
			GameObject src = (GameObject)Resources.Load (_chosenAvatar [0]);
			GameObject dst = (GameObject)Resources.Load (_chosenAvatar [1]);
			if (src != null && dst != null) {	// S'assure que les deux avatars sélectionnés sont bien de meme sexe
				string models = _chosenAvatar [0] + ";" + _chosenAvatar [1];
				PlayerPrefs.SetString (Utils.PREFS_MODEL, models);
				//Destroy (_avatar);
				/*GameObject.Find ("Canvas").SetActive (false);
				initModel.SetActive (true);*/
				Application.LoadLevel(Utils.MAINMENU_SCENE);
			}
		}
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
	
	public Slider sliderGender {
		set {
			_sliderGender = value;
		}
		get {
			return _sliderGender;
		}
	}
	
	public Slider sliderUser {
		set {
			_sliderUser = value;
		}
		get {
			return _sliderUser;
		}
	}

	public Button validateButton {
		set {
			_validateButton = value;
		}
		get {
			return _validateButton;
		}
	}
}
