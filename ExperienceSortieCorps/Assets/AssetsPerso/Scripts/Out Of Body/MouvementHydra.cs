using UnityEngine;
using System.Collections;

public class MouvementHydra : MonoBehaviour {
	
	
	[SerializeField]
	private SixenseHands _hand;
	[SerializeField]
	private GameObject _avatarPosition;
	
	private Vector3 _sensitivity = new Vector3( 0.001f, 0.001f, 0.001f );
	private Quaternion m_initialRotation;
	private Vector3 m_initialPosition;
	
	private Vector3 tmpPosition;
	
	
	void Start () {
		/***
		 * TODO : Gérer le replacement du baton apres que l'avatar soit detecte par la kinect
		 ***/ 
		m_initialRotation = this.gameObject.transform.localRotation;
		_avatarPosition = _avatarPosition.transform.FindChild (PlayerPrefs.GetString (Utils.PREFS_MODEL).Split (';') [0].Split ('/') [2]).gameObject;
		m_initialPosition = _avatarPosition.transform.localPosition;
		m_initialPosition.y += 0.8f;
		m_initialPosition.z -= 0.28f;
		
		tmpPosition = m_initialPosition;
		
		gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
	}
	
	void Update () {
		if ( _hand != SixenseHands.UNKNOWN )
		{
			SixenseInput.Controller controller = SixenseInput.GetController( _hand );
			if ( controller != null && controller.Enabled ) { 
				UpdatePosition(controller);
				UpdateRotation(controller);
			}
		}
	}
	
	void UpdatePosition(SixenseInput.Controller controller)
	{
		Vector3 controllerPosition = new Vector3(controller.Position.x * _sensitivity.x, controller.Position.y * _sensitivity.y, controller.Position.z * _sensitivity.z);
		
		if (controller.GetButtonDown (SixenseButtons.TRIGGER)) 
			tmpPosition = m_initialPosition - controllerPosition;
		
		this.gameObject.transform.localPosition = tmpPosition + controllerPosition;
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
}
