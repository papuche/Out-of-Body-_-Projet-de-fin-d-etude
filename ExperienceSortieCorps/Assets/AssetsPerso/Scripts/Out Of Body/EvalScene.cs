using UnityEngine;
using System.Collections;

public class EvalScene : MonoBehaviour {

	void Update () {
		/*SixenseInput.Controller controller = SixenseInput.GetController( SixenseHands.RIGHT );
		if(controller.GetButtonDown( SixenseButtons.START )){
			baton = true;
		}*/
		if(Input.GetKeyDown(KeyCode.T)){
			PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 0);
		}else if (Input.GetKeyDown (KeyCode.A)) {
			PlayerPrefs.SetInt (Utils.PREFS_CONDITION, 1);
		} else if (Input.GetKeyDown (KeyCode.Z)) {
			PlayerPrefs.SetInt(Utils.PREFS_CONDITION,2);
		} else if (Input.GetKeyDown (KeyCode.E)) {
			PlayerPrefs.SetInt(Utils.PREFS_CONDITION,3);
		} else if (Input.GetKeyDown (KeyCode.F)) {
			PlayerPrefs.SetInt(Utils.PREFS_CONDITION,4);
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			StartCoroutine(ChangeLevel());
		}
	}

	IEnumerator ChangeLevel(){
		float timeFade = gameObject.GetComponent<Fade>().BeginFade(1);
		yield return new WaitForSeconds(timeFade);
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
