using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ReceiveSocket : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		SocketClient.GetInstance();
	}
	
	void Update() {
		
		if (SocketClient.message != null) {
			
			string message = SocketClient.message;

			if(message.Contains(Utils.SOCKET_VALIDATE)){
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
			
			// EXERCICE SORTIE DE CORPS : SANS PARAMETRES
			else if (message.Equals(Utils.SOCKET_NOTHING)) {
				PlayerPrefs.SetString (Utils.PREFS_OUTOFBODY, Utils.NO_PARAMETER);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}

			// EXERCICE SORTIE DE CORPS : PARAMETRE MORPHING
			else if (message.Equals (Utils.SOCKET_MORPHING)) {
				PlayerPrefs.SetString (Utils.PREFS_OUTOFBODY, Utils.MORPHING_PARAMETER);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}
			
			// EXERCICE SORTIE DE CORPS : PARAMETRE BATON
			else if (message.Equals(Utils.SOCKET_BATON)) {
				PlayerPrefs.SetString (Utils.PREFS_OUTOFBODY, Utils.STICK_PARAMETER);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}
			
			// EXERCICE SORTIE DE CORPS : PARAMETRES BATON ET MORPHING
			else if (message.Equals(Utils.SOCKET_BATON_MORPHING)) {
				PlayerPrefs.SetString (Utils.PREFS_OUTOFBODY, Utils.ALL_PARAMETERS);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}

			else if (message.Equals(Utils.SOCKET_GHOST)) {
				PlayerPrefs.SetString(Utils.PREFS_GHOST, message);
			}

			SocketClient.message = null;
		}
	}

	/// <summary>
	/// Méthode appélée lorsque l'application se ferme. Permet de réinitialiser le choix de l'avatar effectué lors de la session
	/// </summary>
	void OnApplicationQuit(){
		PlayerPrefs.DeleteKey (Utils.PREFS_MODEL);
		//SocketClient.GetInstance().StopThread ();
		//SocketClient.GetInstance ().DeleteInstance ();
	}
}

