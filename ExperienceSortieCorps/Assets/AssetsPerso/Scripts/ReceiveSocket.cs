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
			
			// BUTTON AVATAR CHOICE
			if (message.Equals (Utils.SOCKET_AVATAR)) {
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 0);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}
			
			//BOUTON QUITTER
			if (message.Equals (Utils.SOCKET_MORPHING)) {
				PlayerPrefs.SetString (Utils.PREFS_OUTOFBODY, Utils.MORPHING_PARAMETER);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}
			
			if (message.Equals (Utils.SOCKET_STOP)) {
				Application.LoadLevel (Utils.MAINMENU_SCENE);
			}
			
			// EXERCICE DES PORTES : PARAMETRE "PORTES ENTIERE"
			if (message.Contains (Utils.SOCKET_PORTE_ENTIERE)) {
				message = message.Split('/')[1];
				// send playerprefs
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.FULL_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}
			
			// EXERCICE DES PORTES : PARAMETRE "DEMI-PORTES BASSES"
			if (message.Contains (Utils.SOCKET_PORTE_DEMIBAS)) {
				message = message.Split('/')[1];
				// send playerprefs
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.BOTTOM_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}
			
			// EXERCICE DES PORTES : PARAMETRE "DEMI-PORTES HAUTES"
			if (message.Contains (Utils.SOCKET_PORTE_DEMIHAUT)) {
				message = message.Split('/')[1];
				// send playerprefs
				PlayerPrefs.SetString (Utils.PREFS_DOORS, Utils.TOP_DOORS);
				Application.LoadLevel (Utils.DOORS_SCENE);
			}
			
			// EXERCICE SORTIE DE CORPS : SANS PARAMETRES
			if (message.Equals(Utils.SOCKET_NOTHING)) {
				PlayerPrefs.SetString (Utils.PREFS_OUTOFBODY, Utils.NO_PARAMETER);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}
			
			// EXERCICE SORTIE DE CORPS : PARAMETRE BATON
			if (message.Equals(Utils.SOCKET_BATON)) {
				PlayerPrefs.SetString (Utils.PREFS_OUTOFBODY, Utils.STICK_PARAMETER);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}
			
			// EXERCICE SORTIE DE CORPS : PARAMETRES BATON ET MORPHING
			
			if (message.Equals(Utils.SOCKET_BATON_MORPHING)) {
				PlayerPrefs.SetString (Utils.PREFS_OUTOFBODY, Utils.ALL_PARAMETERS);
				PlayerPrefs.SetInt (Utils.PREFS_LAUNCH_MODEL, 1);
				Application.LoadLevel (Utils.OUTOFBODY_SCENE);
			}



			SocketClient.message = null;
		}
	}
}

