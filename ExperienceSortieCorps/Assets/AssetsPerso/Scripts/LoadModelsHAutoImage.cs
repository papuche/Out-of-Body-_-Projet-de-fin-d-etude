using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadModelsHAutoImage : MonoBehaviour {

	public GameObject uiModel;
	public GameObject Content;
	public int itemCount = 10, columnCount = 1;
	// Use this for initialization
	void Start () {
		GameObject[] gs = Resources.LoadAll<GameObject>("Models/Homme/");
		List<GameObject> gos = new List<GameObject>();
		
		foreach (GameObject g in gs) {
			gos.Add(g);
		}
		
		Debug.Log (gos.Count);
		itemCount = gos.Count;
		
		RectTransform rowRectTransform = uiModel.GetComponent<RectTransform>();
		RectTransform containerRectTransform = Content.GetComponent<RectTransform>();
		
		float width = containerRectTransform.rect.width / columnCount;
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;
		int rowCount = itemCount / columnCount;
		if (itemCount % rowCount > 0)
			rowCount++;
		
		float scrollHeight = height * rowCount;
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
		
		
		int j = 0;
		for (int i = 0; i < itemCount; i++) {
			//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
			if (i % columnCount == 0)
				j++;
			
			GameObject tmp = Instantiate (uiModel);
			tmp.transform.SetParent (Content.transform);
			
			RectTransform rectTransform = tmp.GetComponent<RectTransform> ();
			
			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2 (x, y);
			
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2 (x, y);
			RawImage[] images = tmp.GetComponentsInChildren<RawImage> ();

			createPictures(gos[i],images,i);
			/*Sprite Front = Resources.Load<Sprite> (gos [i].name + "F");
			Sprite Back = Resources.Load<Sprite> (gos [i].name + "B");*/

			/*images [0].sprite = Front;
			images [1].sprite = Back;*/
			//tmp.GetComponentInChildren<Text>().text = gos[i].name;
			//			tmp.GetComponentInChildren<LoadScene>().model = gos[i].name;
			foreach (ToggleModel tm in tmp.GetComponentsInChildren<ToggleModel>()) {
				tm.model = gos [i].name;
			}
		}
		Debug.Log (gos.Count);
	}

	void createPictures(GameObject go , RawImage[] images, int nbModel){

		GameObject g = (GameObject)Instantiate (go);
		g.transform.Translate(5.0f * nbModel,0.0f, 0.0f);
		RenderTexture frontView = new RenderTexture(640,640,24);
		RenderTexture backView = new RenderTexture(640,640,24);
		GameObject front = new GameObject ();
		Camera frontCam = front.AddComponent<Camera> ();
		//frontCam.clearFlags = CameraClearFlags.SolidColor;
		front.transform.position = g.transform.position;
		front.transform.Translate (0.0f, 0.8f,1.5f);;
		front.transform.LookAt (g.transform.FindChild("python/Hips").position);
		frontCam.targetTexture = frontView;
		GameObject back = new GameObject ();
		Camera backCam = back.AddComponent <Camera> ();
		//backCam.clearFlags = CameraClearFlags.SolidColor;
		back.transform.position = g.transform.position;
		back.transform.Translate (0.0f, 0.8f, -1.7f);
		back.transform.LookAt (g.transform.FindChild("python/Hips").position);
		backCam.targetTexture = backView;
		images [0].texture = frontView;
		images [1].texture = backView;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
