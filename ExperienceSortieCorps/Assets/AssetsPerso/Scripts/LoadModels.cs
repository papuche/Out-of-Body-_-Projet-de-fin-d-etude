using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadModels : MonoBehaviour {
	
	public GameObject ModelPanel;	// Panel qui contient les modèles
	public GameObject Content;		// Partie du menu affectée par le déplacement de la scrollbar
	public int itemCount, columnCount = 1;

	// Use this for initialization
	void Start () {
		GameObject[] gos = Resources.LoadAll<GameObject>("Models/Femme/");
		
		//Debug.Log (gos.Length);
		itemCount = gos.Length;		// itemCount vaut le nombre de modèles présents dans le répertoire "Models/Femme"
			
		RectTransform rowRectTransform = ModelPanel.GetComponent<RectTransform>();		// On récupère l'élément RectTransform du panel qui contient les modèles
		RectTransform containerRectTransform = Content.GetComponent<RectTransform>();	// On récupère l'élément RectTransform de la partie du menu affectée par la scrollbar
		
		float width = containerRectTransform.rect.width / columnCount;	// On initialise la largeur de la partie du menu affectée par la scrollbar a partir de la largeur du panel qui contient les modèles
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;		// On initialise la hauteur de la partie du menu affectée par la scrollbar a partir de la hauteur du panel qui contient les modèles
		int rowCount = itemCount / columnCount;
		if (itemCount % rowCount > 0)
			rowCount++;
		
		float scrollHeight = height * rowCount;		// Definit la hauteur de la scrollbar
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);		
		
		int j = 0;
		for (int i = 0; i < itemCount; i++) {		// On parcourt les modeles
			//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
			if (i % columnCount == 0)
				j++;
			
			GameObject tmp = Instantiate (ModelPanel);		// Instancie le panel qui contient les modeles
			tmp.transform.SetParent (Content.transform);	// Associe le panel qui contient les modeles au parent
			
			RectTransform rectTransform = tmp.GetComponent<RectTransform> ();	// Recupere l'élement RectTransform du panel instancié précédemment
			
			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);	// Initialise la position en X du panel qui comprend les modeles
			float y = containerRectTransform.rect.height / 2 - height * j;					// Initialise la position en Y du panel qui comprend les modeles
			rectTransform.offsetMin = new Vector2 (x, y);
			
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2 (x, y);
			RawImage[] images = tmp.GetComponentsInChildren<RawImage> ();	// Récupère les deux images du panel qui contient les modeles

			createPictures(gos[i],images,i);	// Crée les images a partir du modele et les ajoute au panel
			foreach (ToggleModel tm in tmp.GetComponentsInChildren<ToggleModel>()) {
				tm.model = gos[i].name;		// Associe le nom du modele pour chaque checkbox du panel
			}
		}
		//Debug.Log (gos.Length);
	}

	void createPictures(GameObject go , RawImage[] images, int nbModel){

		GameObject g = (GameObject)Instantiate (go);				// Instancie un modele (avatar)
		g.transform.Translate(5.0f * nbModel,0.0f, 0.0f);			// Positionne l'avatar sur la scene
		RenderTexture frontView = new RenderTexture(640,640,24);
		RenderTexture backView = new RenderTexture(640,640,24);
		GameObject front = new GameObject ();
		Camera frontCam = front.AddComponent<Camera> ();	// Crée une camera frontale
		front.transform.position = g.transform.position;	// Intialise la position de la camera a celle de l'avatar
		front.transform.Translate (0.0f, 0.8f,1.5f);		// Effectue une translation sur la camera pour la positionner devant l'avatar
		front.transform.LookAt (g.transform.FindChild("python/Hips").position);	// Fixe le point de vue de la camera sur l'avatar
		frontCam.targetTexture = frontView;					// L'aperçu de la camera est envoye sur le RenderTexture frontview
		GameObject back = new GameObject ();
		Camera backCam = back.AddComponent <Camera> ();		// Crée une nouvelle camera
		back.transform.position = g.transform.position;		// Intialise la position de la camera a celle de l'avatar
		back.transform.Translate (0.0f, 0.8f, -1.7f);		// Effectue une translation sur la camera pour la positionner derriere l'avatar
		back.transform.LookAt (g.transform.FindChild("python/Hips").position);	// Fixe le point de vue de la camera sur l'avatar
		backCam.targetTexture = backView;					// L'aperçu de la camera est envoye sur le RenderTexture backview
		images[0].texture = frontView;						// Images[0] = camera frontale
		images[1].texture = backView;						// Images[1] = camera derriere l'avatar
	}
	// Update is called once per frame
	void Update () {
		
	}
}
