using UnityEngine;
using System.Collections;

public class ReceiveSocket : MonoBehaviour
{

	private SocketClient _socketClient;

	// Use this for initialization
	void Start ()
	{
		_socketClient = SocketClient.GetInstance();
	}
	
	void Update() {

		if(Input.GetKeyDown (KeyCode.Q))
			Application.Quit();
		if (_socketClient.message != null) {

			string message = _socketClient.message;

			if (message.Equals(Utils.SOCKET_EXIT)) {
				Application.Quit();
			}
			
			else if(message.Contains(Utils.SOCKET_VALIDATE)){
				PlayerPrefs.SetString(Utils.PREFS_VALIDATE_AVATAR, message.Split('/')[1]);
			}

			// BUTTON AVATAR CHOICE
			else if (message.Contains (Utils.SOCKET_AVATAR)) {
				for(int i=0; i<Utils.SOCKET_GENDER.Length; i++){
					if(message.Substring(0, 1).Equals(Utils.SOCKET_GENDER[i])) {
						PlayerPrefs.SetInt (Utils.PREFS_AVATAR_GENDER, i);
					}
				}
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 0);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}

			// BUTTON RETOUR
			else if (message.Equals (Utils.SOCKET_STOP)) {
				Application.LoadLevel (Utils.WAITING_SCENE);
			}
						
			// EXERCICE DES PORTES : PARAMETRE "PORTES ENTIERE"
			else if (message.Contains (Utils.SOCKET_PORTE_ENTIERE)) {
				PlayerPrefs.SetString (Utils.PREFS_PARAM_DOORS, message.Split('/')[1]);
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.FULL_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}
			
			// EXERCICE DES PORTES : PARAMETRE "DEMI-PORTES BASSES"
			else if (message.Contains (Utils.SOCKET_PORTE_DEMIBAS)) {
				PlayerPrefs.SetString (Utils.PREFS_PARAM_DOORS, message.Split('/')[1]);
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.BOTTOM_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}
			
			// EXERCICE DES PORTES : PARAMETRE "DEMI-PORTES HAUTES"
			else if (message.Contains (Utils.SOCKET_PORTE_DEMIHAUT)) {
				PlayerPrefs.SetString (Utils.PREFS_PARAM_DOORS, message.Split('/')[1]);
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.TOP_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}

			// EXERCICE DE SORTIE DE CORPS
			else if(message.Contains(Utils.SOCKET_OUT_OF_BODY)){
				string[] parameters = message.Remove (0, Utils.SOCKET_OUT_OF_BODY.Length + 1).Split('_');
				int baton = int.Parse(parameters[0]);
				int morphing = int.Parse(parameters[1]);
				int ghost = int.Parse(parameters[2]);

				PlayerPrefs.SetInt(Utils.PREFS_BATON, baton);
				PlayerPrefs.SetInt(Utils.PREFS_MORPHING, morphing);
				PlayerPrefs.SetInt(Utils.PREFS_GHOST, ghost);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);

				if(morphing == 0 && baton == 0) 
					PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 1);
				else if(morphing == 1 && baton == 0) 
					PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 2);
				else if(morphing == 0 && baton == 1) 
					PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 3);
				else if(morphing == 1 && baton == 1) 
					PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 4);

				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}

			_socketClient.message = null;
		}
	}

	/// <summary>
	/// Méthode appélée lorsque l'application se ferme. Permet de réinitialiser le choix de l'avatar effectué lors de la session
	/// </summary>
	void OnApplicationQuit(){
		_socketClient.stopThread = true;
		_socketClient.socket.Close ();
		_socketClient.StopAllProcess ();
		PlayerPrefs.DeleteKey (Utils.PREFS_MODEL);
		PlayerPrefs.DeleteKey (Utils.PREFS_PATH_FOLDER);
	}
}