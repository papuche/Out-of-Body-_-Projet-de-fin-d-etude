using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(ResetSession))]
public class ResetSessionInspector : Editor {

	public override void OnInspectorGUI () {

		if(GUILayout.Button("Reset Sujet-Session numbers")){
			PlayerPrefs.DeleteKey("Sujet");
			PlayerPrefs.DeleteKey("Session");
		}
		
	}
}
