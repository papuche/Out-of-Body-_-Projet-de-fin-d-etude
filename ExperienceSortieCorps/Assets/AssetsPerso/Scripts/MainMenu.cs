using UnityEngine;
using System.Collections;
using System.Net;

public class MainMenu : MonoBehaviour {
	public Texture tex;

	public GUISkin skinBtnDoors;
	public GUISkin skinBtnChoixAvatar; 
	public GUISkin skinBtnExercice; 
	public GUISkin skinQuit; 
	public GUISkin skinParameters; 
	public GUISkin skinTextTitle; 


	//variables for loadLevel
	private bool doorsExercices = false;
	private bool avatarChoice = false;
	private bool avatarExercice = false;
	private bool morfingExercice = false;
	private bool stickExercice = false;
	private bool allExercices = false;
	private bool parameters = false;

	public void OnGUI(){

		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), tex);

		GUI.skin = skinTextTitle;
		GUI.TextField(new Rect(Screen.width*30/100, Screen.height*5/100, Screen.width*40/100, Screen.height * 10 / 100),"Out Of Body Experiment");

		//BOUTON QUITTER		
		GUI.skin = skinQuit;
		if (GUI.Button (new Rect (Screen.width * 80 / 100, Screen.height * 5 / 100, Screen.width * 10 / 100, Screen.height * 7 / 100), "Quit")) {
			Debug.Log ("Quitter !");
			Application.Quit(); 
		}

		//BOUTON PARAMETRE		
		GUI.skin = skinParameters;
		if (GUI.Button (new Rect (Screen.width * 3 / 100, Screen.height * 80 / 100, 80, 70), "")) {
			Debug.Log ("open parameters !");
		}

		// BUTTON DOORS EXERCICES
		GUI.skin = skinBtnDoors;
		if(GUI.Button (new Rect (Screen.width*15/100, Screen.height*18/100, Screen.width*23/100, Screen.height*38/100),"Exercice des portes"))
		{
			Debug.Log ("open doors !");
			Application.LoadLevel(Utils.DOORS_SCENE);
		}

		// BUTTON AVATAR CHOICE
		GUI.skin = skinBtnChoixAvatar;
		if(GUI.Button (new Rect (Screen.width*15/100, Screen.height*58/100, Screen.width*23/100, Screen.height*38/100), "Choix de l'avatar"))		
		{
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_MODEL, 0);
			Application.LoadLevel(Utils.OUTOFBODY_SCENE);
		}

		// BUTTON AVATAR EXERCICE
		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*50/100, Screen.height*20/100, Screen.width*35/100, Screen.height*15/100), "Exercice de l'avatar"))		
		{
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_MODEL, 1);
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_MORPHING, 0);
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_BATON, 0);
			Application.LoadLevel(Utils.OUTOFBODY_SCENE);
		}

		// BUTTON MORFING EXERCICE
		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*50/100, Screen.height*40/100, Screen.width*35/100, Screen.height*15/100), "Exercice de morphing"))		
		{
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_MODEL, 1);
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_MORPHING, 1);
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_BATON, 0);
			Application.LoadLevel(Utils.OUTOFBODY_SCENE);
		}

		// BUTTON STICK EXERCICE
		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*50/100, Screen.height*60/100, Screen.width*35/100, Screen.height*15/100), "Exercice du baton"))		
		{
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_MODEL, 1);
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_BATON, 1);
			PlayerPrefs.SetInt(Utils.PREFS_LAUNCH_MORPHING, 0);
			Application.LoadLevel(Utils.OUTOFBODY_SCENE);
		}

		// BUTTON COMPLETE EXERCICE
		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*50/100, Screen.height*80/100, Screen.width*35/100, Screen.height*15/100), "Tous les exercices"))		
		{
			Debug.Log ("open complete exercice !");
		}

		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*1/100, Screen.height*25/100, Screen.width*10/100, Screen.height*10/100), "Full"))		
		{
			PlayerPrefs.SetString(Utils.PREFS_DOORS, Utils.FULL_DOORS);
			Application.LoadLevel(Utils.DOORS_SCENE);
		}


		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*1/100, Screen.height*40/100, Screen.width*10/100, Screen.height*10/100), "Bottom"))		
		{
			PlayerPrefs.SetString(Utils.PREFS_DOORS, Utils.BOTTOM_DOORS);
			Application.LoadLevel(Utils.DOORS_SCENE);
		}

		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*1/100, Screen.height*55/100, Screen.width*10/100, Screen.height*10/100), "Top"))		
		{
			PlayerPrefs.SetString(Utils.PREFS_DOORS, Utils.TOP_DOORS);
			Application.LoadLevel(Utils.DOORS_SCENE);
		}
	}
}
