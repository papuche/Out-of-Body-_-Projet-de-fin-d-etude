using UnityEngine;
using System.Collections;

public class MouvementHydra : MonoBehaviour {
	

	[SerializeField]
	private float _sensitivity=0.0005f;
	[SerializeField]
	private SixenseHands _hand;
	[SerializeField]
	private GameObject _avatarPosition;
	
	private Vector3 _sensitivityVector;
	private Quaternion m_initialRotation;
	private Vector3 m_initialPosition;
	
	private Vector3 _tmpPosition;
	
	
	void Start () { 
		m_initialRotation = this.gameObject.transform.localRotation;
		
		_avatarPosition = _avatarPosition.transform.FindChild (PlayerPrefs.GetString (Utils.PREFS_MODEL).Split (';') [0].Split ('/') [2]).gameObject;
		GetInitialPositionAvatar ();
		
		_tmpPosition = m_initialPosition;
		
		
		gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
	}
	
	void Update () {

		_sensitivityVector = new Vector3( _sensitivity, _sensitivity, _sensitivity );

		if ( _hand != SixenseHands.UNKNOWN )
		{
			SixenseInput.Controller controller = SixenseInput.GetController( _hand );
			if ( controller != null && controller.Enabled ) { 
				UpdatePosition(controller);
				UpdateRotation(controller);
			}
		}
	}
	
	void GetInitialPositionAvatar(){
		m_initialPosition = _avatarPosition.transform.localPosition;
		m_initialPosition.y += 0.3f;
		m_initialPosition.z -= 0.28f;
	}
	
	void UpdatePosition(SixenseInput.Controller controller)
	{
		Vector3 controllerPosition = new Vector3(controller.Position.x * _sensitivityVector.x, controller.Position.y * _sensitivityVector.y, controller.Position.z * _sensitivityVector.z);
		
		if (controller.GetButtonDown (SixenseButtons.TRIGGER)) {
			GetInitialPositionAvatar();
			_tmpPosition = m_initialPosition - controllerPosition;
		}
		this.gameObject.transform.localPosition = _tmpPosition + controllerPosition;
	}
	
	void UpdateRotation(SixenseInput.Controller controller) {
		Quaternion controllerRotation = new Quaternion( controller.Rotation.x, controller.Rotation.y, controller.Rotation.z, controller.Rotation.w);		
		
		this.gameObject.transform.localRotation = m_initialRotation * controllerRotation;
	}
	
	public GameObject avatarPosition {
		get {
			return _avatarPosition;
		}
		set {
			_avatarPosition = value;
		}
	}
	
	public SixenseHands hand{
		get {
			return _hand;
		}
		set {
			_hand = value;
		}
	}

	public float sensitivity{
		get {
			return _sensitivity;
		}
		set {
			_sensitivity = value;
		}
	}
}
