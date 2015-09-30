using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;

public class InitSceneDoors : MonoBehaviour {

	// Controles de l'utilisateur
	// Oui : Clic gauche souris ou touche "O"
	// Non : Clic droit souris ou touche "N"

	[SerializeField]
	private GameObject _piece;
	[SerializeField]
	private GameObject _text;

	// Gestion de la largeur des portes
	private int _scalesNumber=30;
	private float _scalesMin=0.3f;
	private float _scalesMax=1.2f;
	private List<float> _scales = new List<float>();
	private int _doorWidthIndex;		// Index pointant dans scales
	private Vector3 _currentScale;	// Dimension de la porte dans la scène

	// Nombre de réponses jouées (affichées dans la scène)
	private int _nbAnswers=0;

	// Liste où sont enregistrés les largeur de portes jouées (pour le fichier de résultats)
	private List<float> _ordreOuverture = new List<float>();

	// Modèle sélectionné par le sujet
	private float[] _modelSrcValues;
	// Modèle sélectionné par le psychologue
	private float[] _modelDstValues;
	// Ecart de morphologie
	private float[] _differenceModels;

	// Fichier de résultats
	private StreamWriter _file;
	private string _path;
	private string _fileName;
	private TextAsset _textXml;
	private XmlDocument _xmlDoc;
	private XmlDocument _xmlModel;

	private List<bool> _answers = new List<bool>();
	private bool _next = false;
	private bool _stop;

	private int _sujet;
	private int _session;
	private int _condition;

	void Start () {

		// Assignation de labels à la condition du test et au sujet du test
		_sujet = PlayerPrefs.GetInt ("Sujet", 0);
		_condition = PlayerPrefs.GetInt ("Condition");		

		// Création du fichier de résultats
		_fileName = System.DateTime.Now.ToString ();
		_fileName = _fileName.Replace ("/", "-");
		_fileName = _fileName.Replace (":", "-");

		// Initialisation du tableau conprenant la largeur des portes.
		initScales ();

		createXML ();
		loadXMLFromAssest ();

		// Debug.Log (PlayerPrefs.GetString ("Model"));

		// Recupération du nom des modèles dans modelName
		string[] modelName = PlayerPrefs.GetString ("Model").Split (';');
		if(!modelName[0].Equals("")) {
			_modelSrcValues = ReadModelsValue (modelName [0].Split ('/') [2]);	// Les modèles du sujet sont enregistrés en [0]
			_modelDstValues = ReadModelsValue (modelName [1].Split ('/') [2]);	// Les modèles du phychologue sont enregistrés en [1]
			_differenceModels = calculEcart (_modelSrcValues, _modelDstValues);
		}

		// Mise à jour de la largeur de porte.
		updateWidthDoor ();

		// Ecriture de la largeur de porte 
		_file = new StreamWriter ("Resultat.txt",true);

		// Utile pour la fonction update
		_stop = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!_stop){

			if (Input.GetMouseButtonDown (0) || Input.GetKeyDown(KeyCode.O)) {
				Reponse(true);
				_next = true;
			} else if (Input.GetMouseButtonDown (1) || Input.GetKeyDown(KeyCode.N)) {
				Reponse(false);
				_next = true;
			}

			if(_next){
				if(_scales.Count > 0){
					// Mise à jour de la largeur de porte.
					updateWidthDoor ();
				}
				else {
					_stop=true;
					modifyXml ();
					modifyTxT();
					_file.Close ();
					Application.Quit();
				}
				_next = false;
			}
		}
	}
	/// <summary>
	/// Initialisation du tableau conprenant la largeur des portes.
	/// </summary>
	void initScales(){
		for (int i=0; i<_scalesNumber; i++) {
			_scales.Add (Random.Range (_scalesMin, _scalesMax));
		}
	}

	/// <summary>
	/// Sélection aléatoire d'un index du tableau conprenant la largeur des portes.
	/// </summary>
	/// <returns>The ramdom index.</returns>
	int selectRamdomIndex(){
		return Random.Range (0, _scales.Count);
	}

	/// <summary>
	/// Mise à jour de la largeur de porte.
	/// </summary>
	void updateWidthDoor(){
		_doorWidthIndex = selectRamdomIndex ();		// Récupération aléatoire d'un index
		_currentScale = _piece.transform.localScale;	// Récupération de l'échelle de la porte dans la scène
		_currentScale.x = _scales[_doorWidthIndex];	// Affectation de la coordonée sélectionée dans scales[] sur currentScale.x (largeur de currentScale)
		
		// Ajout de la largeur dans ordreOuverture (pour le fichier de résultats) et suppression de la porte sélectionée dans scales
		_ordreOuverture.Add (_currentScale.x);
		_scales.RemoveAt(_doorWidthIndex);
		
		// Modification de la largeur de porte dans la scène.
		_piece.transform.localScale = _currentScale;	
		
		// Ecriture de l'avancement dans la scène
		_nbAnswers++;
		_text.GetComponent<Text>().text = _nbAnswers.ToString() + "/" + _scalesNumber.ToString();
	}

	void loadXMLFromAssest(){
		
		_xmlModel = new XmlDocument();
		_textXml = (TextAsset)Resources.Load("Models", typeof(TextAsset));
		_xmlModel.LoadXml(_textXml.text);
	}

	void modifyTxT(){
		int essai = 1;
		string modelSrcvalue = _modelSrcValues [0].ToString () + "\t" + _modelSrcValues [1].ToString () + "\t" + _modelSrcValues [2].ToString ();
		string modelDstvalue = _modelDstValues [0].ToString () + "\t" + _modelDstValues [1].ToString () + "\t" + _modelDstValues [2].ToString ();
		string modelDiffvalue = _differenceModels [0].ToString () + "\t" + _differenceModels [1].ToString () + "\t" + _differenceModels [2].ToString ();
		foreach (bool answer in _answers) {
			if (answer) {
				_file.WriteLine(_sujet.ToString() + "\t" + _condition.ToString() + "\t" + essai.ToString() + "\t" + _ordreOuverture[essai-1].ToString() + "\t" + "1" + "\t" + modelSrcvalue + "\t" + modelDstvalue + "\t" + modelDiffvalue);
			} else {
				_file.WriteLine(_sujet.ToString() + "\t" + _condition.ToString() + "\t" + essai.ToString() + "\t" + _ordreOuverture[essai-1].ToString() + "\t" + "0" + "\t" + modelSrcvalue + "\t" + modelDstvalue + "\t" + modelDiffvalue);
			}
			essai++;
		}
	}
	void modifyXml(){
		int indexlist = 0;
		XmlNode nodeact = _xmlDoc.SelectSingleNode("Ouvertures");
		foreach(XmlElement node in nodeact.SelectNodes("Ouverture")){
			node.FirstChild.InnerText = _ordreOuverture[indexlist].ToString();
			if(_answers[indexlist]){
				node.LastChild.InnerText = "Oui";
			}else{
				node.LastChild.InnerText = "Non";
			}
			indexlist++;
		}

		_xmlDoc.Save(_fileName+".xml");
	}

	public void Reponse(bool rep){
		_answers.Add (rep);
		_next = true;
	}

	void createXML(){
		_xmlDoc = new XmlDocument ();
		XmlElement root = (XmlElement)_xmlDoc.AppendChild(_xmlDoc.CreateElement("Ouvertures"));

		for(int i = 0; i < _scales.Count; i++){
			XmlElement el = (XmlElement)root.AppendChild(_xmlDoc.CreateElement("Ouverture"));
			el.AppendChild(_xmlDoc.CreateElement("Taille"));
			el.AppendChild(_xmlDoc.CreateElement("Reponse"));
		}
	}

	float[] ReadModelsValue(string name){
		string nameNode= "", waist = "", hips= "", chest= "";
		foreach (XmlElement node in _xmlModel.SelectNodes("Models/Model")) {
			nameNode = node.GetAttribute ("name");
			if(nameNode.Equals(name)){
				XmlNode waistNode = node.SelectSingleNode("Waist");
				waist = waistNode.InnerText;
				XmlNode chestNode = node.SelectSingleNode("Chest");
				chest = chestNode.InnerText;
				XmlNode hipsNode = node.SelectSingleNode("Hips");
				hips = hipsNode.InnerText;
			}
		}
		float[] values = new float[3];
		values [0] = float.Parse (waist);
		values [1] = float.Parse (chest);
		values [2] = float.Parse (hips);
		return values;
	}
	
	float[] calculEcart(float[] src, float[] dst){
		float[] resultat = new float[3];
		resultat [0] = (dst [0] - src [0]) / src [0] * 100; 
		resultat [1] = (dst [1] - src [1]) / src [1] * 100;
		resultat [2] = (dst [2] - src [2]) / src [2] * 100;
		return resultat;
	}

	public GameObject piece {
		get {
			return _piece;
		}
		set { 
			_piece = value;
		}
	}
	public GameObject text{
		get {
			return _text;
		}
		set { 
			_text = value;
		}
	}
}