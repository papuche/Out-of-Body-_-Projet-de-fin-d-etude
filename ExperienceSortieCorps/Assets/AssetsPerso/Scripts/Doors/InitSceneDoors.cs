using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

public class InitSceneDoors : MonoBehaviour {

	[SerializeField]
	private GameObject _fullDoors;
	[SerializeField]
	private GameObject _bottomDoors;
	[SerializeField]
	private GameObject _topDoors;
	[SerializeField]
	private GameObject _text;

	// Gestion de la largeur des portes
	private GameObject _piece;
	private int _nbDoors = 0;
	private List<Measure> _scales = new List<Measure>();
	private int _doorIndex;		// Index pointant dans scales
	private Vector3 _currentScale;	// Dimension de la porte dans la scène

	// Nombre de réponses jouées (affichées dans la scène)
	private int _nbAnswers=0;

	// Liste où sont enregistrés les largeur de portes jouées (pour le fichier de résultats)
	private List<Measure> _ordreOuverture = new List<Measure>();

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
	private string _doorType;

	void Start () {
		string doors = PlayerPrefs.GetString (Utils.PREFS_DOORS);

		if (doors.Equals (Utils.BOTTOM_DOORS)) {
			_bottomDoors.SetActive(true);
			_piece = _bottomDoors;
			_doorType = "Porte basse";
		} else if (doors.Equals (Utils.TOP_DOORS)) {
			_topDoors.SetActive (true);
			_piece = _topDoors;
			_doorType = "Porte haute";
		} else {
			_fullDoors.SetActive (true);
			_piece = _fullDoors;
			_doorType = "Porte pleine";
		}
		_currentScale = _piece.transform.localScale;

		// Assignation de labels à la condition du test et au sujet du test
		_sujet = PlayerPrefs.GetInt (Utils.PREFS_SUJET, 0);
		_condition = PlayerPrefs.GetInt (Utils.PREFS_CONDITION);		

		// Création du fichier de résultats
		_fileName = System.DateTime.Now.ToString ();
		_fileName = _fileName.Replace ("/", "-");
		_fileName = _fileName.Replace (":", "-");

		// Initialisation du tableau conprenant la largeur des portes.
		initScales ();

		createXML ();
		loadXMLFromAssest ();

		// Recupération du nom des modèles dans modelName
		string[] modelName = PlayerPrefs.GetString (Utils.PREFS_MODEL).Split (';');
		if(!modelName[0].Equals("")) {
			_modelSrcValues = ReadModelsValue (modelName [0].Split ('/') [2]);	// Les modèles du sujet sont enregistrés en [0]
			_modelDstValues = ReadModelsValue (modelName [1].Split ('/') [2]);	// Les modèles du phychologue sont enregistrés en [1]
			_differenceModels = calculEcart (_modelSrcValues, _modelDstValues);
		}

		// Mise à jour de la largeur de porte.
		_doorIndex = Random.Range (0, _scales.Count);
		applyScale ();

		_file = new StreamWriter ("Resultat.txt",true);
		
		_stop = false;
		// Ecriture de l'avancement dans la scène
		_nbAnswers++;
		_text.GetComponent<Text>().text = _nbAnswers.ToString() + "/" + _nbDoors.ToString();	
	}
	
	void Update () {
		if(!_stop){
			if (Input.GetKeyDown (KeyCode.O) || Input.GetMouseButtonDown(0)) {
				Reponse(true);
				_next = true;
			} else if (Input.GetKeyDown (KeyCode.N) || Input.GetMouseButtonDown(1)) {
				Reponse(false);
				_next = true;
			}
			if(_next){
				if(_scales.Count > 0){
					int nbTry = 0;
					float scale;
					if(_ordreOuverture[_ordreOuverture.Count-1].key.Equals(Utils.WIDTH_KEY))
					scale = _currentScale.x;
					else 
						scale = _currentScale.y;
						do{
							//Random.
							_doorIndex = Random.Range(0,(_scales.Count));
							nbTry++;
							if(nbTry > 10){
								break;
							}
						}while(scale == _scales[_doorIndex].scale || !_scales[_doorIndex].key.Equals(_ordreOuverture[_ordreOuverture.Count-1]));

					applyScale();
					_next = false;
					_nbAnswers++;
					_text.GetComponent<Text>().text = _nbAnswers.ToString() + "/" + _nbDoors.ToString();
				} else {
					_stop=true;
					modifyXml ();
					modifyTxT();
					_file.Close ();
					Application.LoadLevel(Utils.WAITING_SCENE);
				}
			}
		}
	}

	/// <summary>
	/// Initialisation du tableau conprenant les échelles des portes.
	/// </summary>
	void initScales(){
		List<Measure> measures = new List<Measure> ();

		string resSocket = PlayerPrefs.GetString (Utils.PREFS_PARAM_DOORS);

		int nbTries = int.Parse (resSocket.Split ('_') [0]);

		int nbWidth = int.Parse(resSocket.Split ('_') [1]);
		if(nbWidth > 0){
			int widthStep = int.Parse(resSocket.Split ('_') [2]);
			for (int i = 0; i < nbWidth; i++){
				measures.Add (new Measure((float)(widthStep * (i + 1) / 100.0 + 1.0), Utils.WIDTH_KEY));
			}
		}

		int nbHeight = int.Parse(resSocket.Split ('_') [3]);
		if(nbHeight > 0){
			int heightStep = int.Parse(resSocket.Split ('_') [4]);
			for (int i = 0; i < nbHeight; i++){
				measures.Add (new Measure((float)(heightStep * (i + 1) / 100.0 + 1.0), Utils.HEIGHT_KEY));
			}
		}

		for (int i = 0; i<nbTries; i++) {
			_scales.AddRange(measures);
		}
		_nbDoors = _scales.Count;

	}

	/// <summary>
	/// Met a jour l'échelle de la porte en fonction de la valeur de _widthIndex
	/// </summary>
	void applyScale(){
		if(_scales[_doorIndex].key.Equals(Utils.WIDTH_KEY)){
			_currentScale.x = _scales[_doorIndex].scale;
			_currentScale.y = 1.0f;
		}
		else {
			_currentScale.x = 1.0f;
			_currentScale.y = _scales[_doorIndex].scale;
		}
		_ordreOuverture.Add (_scales[_doorIndex]);
		_scales.RemoveAt(_doorIndex);
		_piece.transform.localScale = _currentScale;
	}

	void loadXMLFromAssest(){
		_xmlModel = new XmlDocument();
		_textXml = (TextAsset)Resources.Load("Models", typeof(TextAsset));
		_xmlModel.LoadXml(_textXml.text);
	}

	void modifyTxT(){
		int essai = 1;
		string modelSrcvalue;
		string modelDstvalue;
		string modelDiffvalue;
		if (_modelSrcValues != null) {
			modelSrcvalue = _modelSrcValues [0].ToString () + "\t" + _modelSrcValues [1].ToString () + "\t" + _modelSrcValues [2].ToString ();
			modelDstvalue = _modelDstValues [0].ToString () + "\t" + _modelDstValues [1].ToString () + "\t" + _modelDstValues [2].ToString ();
			modelDiffvalue = _differenceModels [0].ToString () + "\t" + _differenceModels [1].ToString () + "\t" + _differenceModels [2].ToString ();
		} else {
			modelSrcvalue = "0\t0\t0";
			modelDstvalue = "0\t0\t0";
			modelDiffvalue = "0\t0\t0";
		}
		foreach (bool answer in _answers) {
			_file.WriteLine (_sujet.ToString () + "\t" + _condition.ToString () + "\t" + essai.ToString () + "\t" + _doorType + "\t" + (_ordreOuverture [essai - 1].key.Equals(Utils.WIDTH_KEY) ? "Longueur" : "Largeur") + "\t" + _ordreOuverture [essai - 1].scale.ToString() + "\t" + (answer == true ? "1" : "0") + "\t" + modelSrcvalue + "\t" + modelDstvalue + "\t" + modelDiffvalue);
			essai++;
		}
	}

	void modifyXml(){
		int indexlist = 0;
		XmlNode nodeact = _xmlDoc.SelectSingleNode("Ouvertures");
		foreach(XmlElement node in nodeact.SelectNodes("Ouverture")){
			node.FirstChild.InnerText = _doorType;
			node.SelectSingleNode("Modification").InnerText = _ordreOuverture[indexlist].key.Equals(Utils.WIDTH_KEY) ? "Longueur" : "Largeur";
			node.SelectSingleNode("Taille").InnerText = _ordreOuverture[indexlist].scale.ToString();
			if(_answers[indexlist]){
				node.LastChild.InnerText = "Oui";
			}else{
				node.LastChild.InnerText = "Non";
			}
			indexlist++;
		}

		_xmlDoc.Save(_fileName+".xml");
	}

	void Reponse(bool rep){
		_answers.Add (rep);
		_next = true;
	}

	void createXML(){
		_xmlDoc = new XmlDocument ();
		XmlElement root = (XmlElement)_xmlDoc.AppendChild(_xmlDoc.CreateElement("Ouvertures"));

		for(int i = 0; i < _scales.Count; i++){
			XmlElement el = (XmlElement)root.AppendChild(_xmlDoc.CreateElement("Ouverture"));
			el.AppendChild(_xmlDoc.CreateElement("Type"));
			el.AppendChild(_xmlDoc.CreateElement("Modification"));
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
	
	public GameObject text{
		get {
			return _text;
		}
		set { 
			_text = value;
		}
	}

	public GameObject fullDoors{
		get {
			return _fullDoors;
		}
		set { 
			_fullDoors = value;
		}
	}

	public GameObject bottomDoors{
		get {
			return _bottomDoors;
		}
		set { 
			_bottomDoors = value;
		}
	}

	public GameObject topDoors{
		get {
			return _topDoors;
		}
		set { 
			_topDoors = value;
		}
	}

	private class Measure {
		private float _scale;
		private string _key;

		public Measure (float scale, string key)
		{
			_scale = scale;
			_key = key;
		}


		public float scale {
			get {
				return _scale;
			}
			set {
				_scale = value;
			}
		}

		public string key {
			get {
				return _key;
			}
			set {
				_key = value;
			}
		}
	}
}