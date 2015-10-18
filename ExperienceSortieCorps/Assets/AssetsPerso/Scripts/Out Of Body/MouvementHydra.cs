using UnityEngine;
using System.Collections;

public class MouvementHydra : MonoBehaviour {
	
	public bool 				ActivateRotations;
	public SixenseHands			Hand;
	public Vector3				Sensitivity = new Vector3( 0.01f, 0.01f, 0.01f );
	
	protected bool				m_enabled = false;
	protected Quaternion		m_initialRotation;
	protected Vector3			m_initialPosition;
	protected Vector3			m_baseControllerPosition;
	
	// Use this for initialization
	void Start () {
		m_initialRotation = this.gameObject.transform.localRotation;
		m_initialPosition = this.gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Hand == SixenseHands.UNKNOWN )
		{
			return;
		}
		
		SixenseInput.Controller controller = SixenseInput.GetController( Hand );
		if ( controller != null && controller.Enabled )  
		{		
			UpdateObject(controller);
		}
	}
	
	/*void OnGUI()
	{
		if ( !m_enabled )
		{
			GUI.Box( new Rect( Screen.width / 2 - 100, Screen.height - 40, 200, 30 ),  "Press Start To Move/Rotate" );
		}
	}*/
	
	
	protected virtual void UpdateObject(  SixenseInput.Controller controller )
	{
		if ( Input.GetKeyDown (KeyCode.E) || Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.T)/*controller.GetButtonDown( SixenseButtons.START )*/ )
		{
			// enable position and orientation control
			m_enabled = !m_enabled;
			
			// delta controller position is relative to this point
			m_baseControllerPosition = new Vector3( controller.Position.x * Sensitivity.x,
			                                       controller.Position.y * Sensitivity.y,
			                                       controller.Position.z * Sensitivity.z );
			
			// this is the new start position
			m_initialPosition = this.gameObject.transform.localPosition;
			
			gameObject.GetComponentInChildren<MeshRenderer>().enabled = m_enabled;
		}
		
		/*if (m_enabled && controller.GetButtonDown (SixenseButtons.JOYSTICK)) 
		{
			this.gameObject.transform.Translate(0.0f,controller.JoystickY,controller.JoystickX);
		}*/
		if ( m_enabled )
		{
			///Déplacement du gameObject au joystick
			m_initialPosition += new Vector3(0.0f,controller.JoystickY*0.01f,controller.JoystickX*0.01f);
			Debug.Log(new Vector3(0.0f,controller.JoystickY,controller.JoystickX));
			
			//Déplacement en profondeur du gameObject via RB RT
			if(controller.GetButton( SixenseButtons.BUMPER )){
				m_initialPosition += new Vector3(0.005f,0.0f,0.0f);
			}else if (controller.GetButton( SixenseButtons.TRIGGER )){
				m_initialPosition -= new Vector3(0.005f,0.0f,0.0f);
			}
			
			// Rotation suivant l'axe Y via les boutons 1 & 3
			if(controller.GetButton( SixenseButtons.ONE )){
				m_initialRotation.y += 0.005f;
			}else if (controller.GetButton( SixenseButtons.THREE )){
				m_initialRotation.y -= 0.005f;
			}
			
			// Rotation suivant l'axe Z via les boutons 2 & 4
			if(controller.GetButton( SixenseButtons.TWO )){
				m_initialRotation.z += 0.005f;
			}else if (controller.GetButton( SixenseButtons.FOUR )){
				m_initialRotation.z -= 0.005f;
			}
			
			// Modifie la sensibilité via les boutons 2 & 4
			if(controller.GetButton( SixenseButtons.TWO )){
				Sensitivity += new Vector3(0.00001f,0.00001f,0.00001f);
				/*m_baseControllerPosition = new Vector3( -controller.Position.z * Sensitivity.x,
				                                      	 controller.Position.y * Sensitivity.y,
				                                      	 controller.Position.x * Sensitivity.z );*/
			}else if (controller.GetButton( SixenseButtons.FOUR )){
				if(Sensitivity.x > 0.0f){
					Sensitivity -= new Vector3(0.00001f,0.00001f,0.00001f);
					/*m_baseControllerPosition = new Vector3( -controller.Position.z * Sensitivity.x,
				                                       	controller.Position.y * Sensitivity.y,
				                                       	controller.Position.x * Sensitivity.z );*/
				}
			}
			
			UpdatePosition( controller );
			
			if (ActivateRotations){
				UpdateRotation( controller );
			}
		}
	}
	 
	protected void UpdatePosition( SixenseInput.Controller controller )
	{
		Vector3 controllerPosition = new Vector3( controller.Position.x * Sensitivity.x,
		                                         controller.Position.y * Sensitivity.y,
		                                         controller.Position.z * Sensitivity.z );
		
		// distance controller has moved since enabling positional control
		Vector3 vDeltaControllerPos = controllerPosition - m_baseControllerPosition;
		
		// update the localposition of the object
		this.gameObject.transform.localPosition = m_initialPosition + vDeltaControllerPos;
	}
	
	
	protected void UpdateRotation( SixenseInput.Controller controller )
	{
		Quaternion controllerRotation = new Quaternion( controller.Rotation.x,
		                                               controller.Rotation.y,
		                                               controller.Rotation.z,
		                                               controller.Rotation.w);
		
		this.gameObject.transform.localRotation = controllerRotation * m_initialRotation;
		//controller.Rotation.z
	}
}
