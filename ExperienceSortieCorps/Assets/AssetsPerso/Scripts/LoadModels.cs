using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadModels : MonoBehaviour
{	
	[SerializeField]
	private GameObject _modelPanel;
	[SerializeField]
	public GameObject _content;		

	public Scrollbar horizontalScrollbar;

	private float _rotationSpeed = 2.0f;

	private int _itemCount = 10;
	private int _columnCount = 1;

	private GameObject[] _go_models;
	
	// Use this for initialization
	void Start ()
	{
		InitModels ();

		_itemCount = _go_models.Length;		// _itemCount vaut le nombre de modèles présents dans le répertoire "Models/Femme"
			
		RectTransform rowRectTransform = _modelPanel.GetComponent<RectTransform> ();		// On récupère l'élément RectTransform du panel qui contient les modèles
		RectTransform containerRectTransform = Content.GetComponent<RectTransform> ();	// On récupère l'élément RectTransform de la partie du menu affectée par la scrollbar
		
		float width = containerRectTransform.rect.width / _columnCount;	// On initialise la largeur de la partie du menu affectée par la scrollbar a partir de la largeur du panel qui contient les modèles
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;		// On initialise la hauteur de la partie du menu affectée par la scrollbar a partir de la hauteur du panel qui contient les modèles
		int rowCount = _itemCount / _columnCount;
		if (_itemCount % rowCount > 0)
			rowCount++;
		
		float scrollHeight = height * rowCount;		// Definit la hauteur de la scrollbar
		containerRectTransform.offsetMin = new Vector2 (containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2 (containerRectTransform.offsetMax.x, scrollHeight / 2);		

		GameObject frontCameras = new GameObject ();
		frontCameras.name = "FrontCameras";

		GameObject backCameras = new GameObject ();
		backCameras.name = "BackCameras";

		int j = 0;
		for (int i = 0; i < _itemCount; i++) {		// On parcourt les modeles
			//this is used instead of a double for loop because _itemCount may not fit perfectly into the rows/columns
			if (i % _columnCount == 0)
				j++;
			
			GameObject tmp = Instantiate (_modelPanel);		// Instancie le panel qui contient les modeles
			tmp.transform.SetParent (Content.transform);	// Associe le panel qui contient les modeles au parent
			
			RectTransform rectTransform = tmp.GetComponent<RectTransform> ();	// Recupere l'élement RectTransform du panel instancié précédemment
			
			float x = -containerRectTransform.rect.width / 2 + width * (i % _columnCount);	// Initialise la position en X du panel qui comprend les modeles
			float y = containerRectTransform.rect.height / 2 - height * j;					// Initialise la position en Y du panel qui comprend les modeles
			rectTransform.offsetMin = new Vector2 (x, y);
			
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2 (x, y);
			RawImage[] images = tmp.GetComponentsInChildren<RawImage> ();	// Récupère les deux images du panel qui contient les modeles

			images [0].texture = CreateCamera (_go_models [i], new Vector3 (0.0f, 0.8f, 1.7f), frontCameras, "FrontCamera_" + i);		// Recupere l'image de la camera située devant l'avatar
			images [1].texture = CreateCamera (_go_models [i], new Vector3 (0.0f, 0.8f, -1.7f), backCameras, "BackCamera_" + i);	// Recupere l'image de la camera située derrière l'avatar
			foreach (ToggleModel tm in tmp.GetComponentsInChildren<ToggleModel>()) {
				tm.model = _go_models [i].name;		// Associe le nom du modele pour chaque checkbox du panel
			}
		}
	}

	/// <summary>
	/// Initialise les avatars à partir des modèles
	/// </summary>
	void InitModels ()
	{
		//_go_models = Resources.LoadAll<GameObject> ("Models/Femme/");
		_go_models = Resources.LoadAll<GameObject> ("Models/Homme/");
		GameObject avatars = new GameObject ();							// Crée un gameobject qui contient l'ensemble des avatars instanciés
		avatars.name = "Avatars";
		for (int i = 0; i< _go_models.Length; i++) {
			_go_models [i] = (GameObject)Instantiate (_go_models [i]);	// Instancie un modele (avatar)
			_go_models [i].transform.parent = avatars.transform;
			_go_models [i].transform.Translate (5.0f * i, 0.0f, 0.0f);	// Positionne l'avatar sur la scene
			//_go_models [i].transform.Rotate (0.0f, 180.0f, 0.0f);			// Met de face l'avatar
		}
	}
	
	/// <summary>
	/// Créé une caméra
	/// </summary>
	/// <returns>Retourne la texture de l'image de la caméra</returns>
	/// <param name="g">Correspond a l'avatar de la scène.</param>
	/// <param name="v">La position de la camera</param>
	/// <param name="parent">Le parent de la camera</param>
	/// <param name="cameraName">Le nom de la camera</param>
	RenderTexture CreateCamera (GameObject g, Vector3 v, GameObject parent, string cameraName)
	{
		RenderTexture view = new RenderTexture (640, 640, 24);
		GameObject go = new GameObject ();
		go.name = cameraName;
		go.transform.parent = parent.transform;
		Camera cam = go.AddComponent<Camera> ();	// Crée une camera frontale
		go.transform.position = g.transform.position;	// Intialise la position de la camera a celle de l'avatar
		go.transform.Translate (v);		// Effectue une translation sur la camera pour la positionner devant l'avatar
		go.transform.LookAt (g.transform.FindChild ("python/Hips").position);	// Fixe le point de vue de la camera sur l'avatar
		cam.targetTexture = view;					// L'aperçu de la camera est envoye sur le RenderTexture frontview
		return view;
	}

	// Update is called once per frame
	void Update ()
	{
		foreach (GameObject g in _go_models) {
			g.transform.Rotate (0.0f, _rotationSpeed, 0.0f);
		}
	}

	// Panel qui contient les modèles
	public GameObject ModelPanel {
		get {
			return _modelPanel;
		}
		set { 
			_modelPanel = value;
		}
	}
	
	// Partie du menu affectée par le déplacement de la scrollbar
	public GameObject Content {
		get {
			return _content;
		}
		set { 
			_content = value;
		}
	}
}