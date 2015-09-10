using UnityEngine;
using System.Collections;

public class EvalScene : MonoBehaviour {
	
	bool morph = false;
	bool baton = false;
	// Update is called once per frame
	void Start(){

	}
	void Update () {
		/*SixenseInput.Controller controller = SixenseInput.GetController( SixenseHands.RIGHT );
		if(controller.GetButtonDown( SixenseButtons.START )){
			baton = true;
		}*/
		if(Input.GetKeyDown(KeyCode.T)){
			PlayerPrefs.SetInt ("Condition", 0);
		}else if (Input.GetKeyDown (KeyCode.A)) {
			PlayerPrefs.SetInt ("Condition", 1);
		} else if (Input.GetKeyDown (KeyCode.Z)) {
			PlayerPrefs.SetInt("Condition",2);
		} else if (Input.GetKeyDown (KeyCode.E)) {
			PlayerPrefs.SetInt("Condition",3);
		} else if (Input.GetKeyDown (KeyCode.F)) {
			PlayerPrefs.SetInt("Condition",4);
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			StartCoroutine(ChangeLevel());
		}
	}

	IEnumerator ChangeLevel(){
		float timeFade = gameObject.GetComponent<Fade>().BeginFade(1);
		yield return new WaitForSeconds(timeFade);
		Debug.Log("test2");
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
