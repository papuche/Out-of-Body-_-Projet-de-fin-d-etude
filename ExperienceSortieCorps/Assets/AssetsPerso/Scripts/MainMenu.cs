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
	public bool doorsExercices;
	public bool avatarChoice;
	public bool avatarExercice;
	public bool morfingExercice;
	public bool stickExercice;
	public bool allExercices;
	public bool parameters;

	// Use this for initialization
	void Start () {

		tex= Resources.Load("light-wood", typeof(Texture)) as Texture;


		doorsExercices = false;
		avatarChoice = false;
		avatarExercice = false;
		morfingExercice = false;
		stickExercice = false;
		allExercices = false;
		parameters = false;

	}
	
	// Update is called once per frame
	void Update () {}

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
			parameters=true;
		}
		if (parameters) {
			Debug.Log ("open parameters !");
		}

		// BUTTON DOORS EXERCICES
		GUI.skin = skinBtnDoors;
		if(GUI.Button (new Rect (Screen.width*15/100, Screen.height*18/100, Screen.width*23/100, Screen.height*38/100),"Exercice des portes"))
		{
			doorsExercices=true;
		}
		if (doorsExercices) {
			Debug.Log ("open doors !");
			Application.LoadLevel("Doors");
		}

		// BUTTON AVATAR CHOICE
		GUI.skin = skinBtnChoixAvatar;
		if(GUI.Button (new Rect (Screen.width*15/100, Screen.height*58/100, Screen.width*23/100, Screen.height*38/100), "Choix de l'avatar"))		
		{
			avatarChoice=true;
		}
		if (avatarChoice) {
			Debug.Log ("open avatar choice !");
			Application.LoadLevel("Out Of Body");
		}

		// BUTTON AVATAR EXERCICE
		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*50/100, Screen.height*20/100, Screen.width*35/100, Screen.height*15/100), "Exercice de l'avatar"))		
		{
			avatarExercice=true;
		}
		if (avatarExercice) {
			Debug.Log ("open avatar exercice !");
		}

		// BUTTON MORFING EXERCICE
		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*50/100, Screen.height*40/100, Screen.width*35/100, Screen.height*15/100), "Exercice de morfing"))		
		{
			avatarExercice=true;
		}
		if (avatarExercice) {
			Debug.Log ("open morfing exercice !");
		}

		// BUTTON STICK EXERCICE
		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*50/100, Screen.height*60/100, Screen.width*35/100, Screen.height*15/100), "Exercice du baton"))		
		{
			avatarExercice=true;
		}
		if (avatarExercice) {
			Debug.Log ("open stick exercice !");
		}

		// BUTTON COMPLETE EXERCICE
		GUI.skin = skinBtnExercice;
		if(GUI.Button (new Rect (Screen.width*50/100, Screen.height*80/100, Screen.width*35/100, Screen.height*15/100), "Tous les exercices"))		
		{
			avatarExercice=true;
		}
		if (avatarExercice) {
			Debug.Log ("open complele exercice !");
		}
	}


}
