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

	private float _initialScaleX;
	private float _initialScaleY;

	private System.DateTime _time;

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
	private string _fileName;
	private XmlDocument _xmlModel;

	private List<bool> _answers = new List<bool>();
	private bool _next = false;
	private bool _stop;

	private int _doorType;

	void Start () {
		string doors = PlayerPrefs.GetString (Utils.PREFS_DOORS);

		if (doors.Equals (Utils.BOTTOM_DOORS)) {
			_bottomDoors.SetActive(true);
			_piece = _bottomDoors;
			_doorType = FilesConst.BOTTOM_DOOR;
		} else if (doors.Equals (Utils.TOP_DOORS)) {
			_topDoors.SetActive (true);
			_piece = _topDoors;
			_doorType = FilesConst.TOP_DOOR;
		} else {
			_fullDoors.SetActive (true);
			_piece = _fullDoors;
			_doorType = FilesConst.FULL_DOOR;
		}
		_currentScale = _piece.transform.localScale;
		_initialScaleX = _currentScale.x;
		_initialScaleY = _currentScale.y;

		// Assignation de labels à la condition du test et au sujet du test

		// Création du fichier de résultats
		_fileName = System.DateTime.Now.ToString ();
		_fileName = _fileName.Replace ("/", "-");
		_fileName = _fileName.Replace (":", "-");

		// Initialisation du tableau conprenant la largeur des portes.
		initScales ();

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
			_ordreOuverture[_ordreOuverture.Count-1].time = (System.DateTime.Now - _time).TotalMilliseconds;
			if(_next){
				if(_scales.Count > 0){
					int nbTry = 0;
					do {
						//Random.
						_doorIndex = Random.Range(0,(_scales.Count));
						nbTry++;
						if(nbTry > 10){
							break;
						}
					} while(_currentScale.x == _scales[_doorIndex].width && _currentScale.y == _scales[_doorIndex].height);

					applyScale();
					_next = false;
					_nbAnswers++;
					_text.GetComponent<Text>().text = _nbAnswers.ToString() + "/" + _nbDoors.ToString();
				} else {
					_stop=true;

					string dir = PlayerPrefs.GetString (Utils.PREFS_PATH_FOLDER);
					string username = dir.Remove (0, dir.LastIndexOf('\\') + 1).Split ('_')[0];
					CreateResultFile(dir, username);
					createTxT(username);

					SocketClient.GetInstance().Write(Utils.SOCKET_END_DOOR);	// Envoi de la trame de fin d'exercice des portes au client
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
		int nbHeight;
		if(_doorType != FilesConst.FULL_DOOR)
			nbHeight = 0;
		else
			nbHeight = int.Parse(resSocket.Split ('_') [3]);

		if (nbWidth > 0) {
			int widthStep = int.Parse (resSocket.Split ('_') [2]);
			int heightStep = int.Parse (resSocket.Split ('_') [4]);
			if (nbWidth % 2 != 0) {	// Si nbWidth est un nombre impair
				for (int i = -nbWidth / 2; i < (nbWidth + 1) / 2; i++) {
					if (nbHeight > 0) {
						if (nbHeight % 2 != 0) {	// Si nbHeight est un nombre impair
							for (int j = -nbHeight / 2; j < (nbHeight + 1) / 2; j++) {
								measures.Add (new Measure ((float)(widthStep * i / 100.0 + 1.0) * _initialScaleX, (float)(heightStep * j / 100.0 + 1.0) * _initialScaleY));
							}
						} else {	// Si nbHeight est un nombre pair
							for (int j = -nbHeight / 2; j < nbHeight / 2; j++) {
								measures.Add (new Measure ((float)(widthStep * i / 100.0 + 1.0) * _initialScaleX, (float)(heightStep * j / 100.0 + 1.0) * _initialScaleY + heightStep * _initialScaleY / (2 * 100)));
							}
						}
					} else {
						measures.Add (new Measure ((float)(widthStep * i / 100.0 + 1.0) * _initialScaleX, _initialScaleY));
					}
				}
			} else {	// Si nbWidth est un nombre pair
				for (int i = -nbWidth / 2; i < nbWidth / 2; i++) {
					if (nbHeight > 0) {
						if (nbHeight % 2 != 0) {	// Si nbHeight est un nombre impair
							for (int j = -nbHeight / 2; j < (nbHeight + 1) / 2; j++) {
								measures.Add (new Measure ((float)(widthStep * i / 100.0 + 1.0) * _initialScaleX + widthStep * _initialScaleX / (2 * 100), (float)(heightStep * j / 100.0 + 1.0) * _initialScaleY));
							}
						} else {	// Si nbHeight est un nombre pair
							for (int j = -nbHeight / 2; j < nbHeight / 2; j++) {
								measures.Add (new Measure ((float)(widthStep * i / 100.0 + 1.0) * _initialScaleX + widthStep * _initialScaleX / (2 * 100), (float)(heightStep * j / 100.0 + 1.0) * _initialScaleY + heightStep * _initialScaleY / (2 * 100)));
							}
						}
					} else {
						measures.Add (new Measure ((float)(widthStep * i / 100.0 + 1.0) * _initialScaleX + widthStep * _initialScaleX / (2 * 100), _initialScaleY));
					}
				}
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
		_currentScale.x = _scales[_doorIndex].width;
		_currentScale.y = _scales[_doorIndex].height;
		_ordreOuverture.Add (_scales[_doorIndex]);
		_scales.RemoveAt(_doorIndex);
		_piece.transform.localScale = _currentScale;
		_time = System.DateTime.Now;
	}

	void loadXMLFromAssest(){
		_xmlModel = new XmlDocument();
		TextAsset textXml = (TextAsset)Resources.Load("Models", typeof(TextAsset));
		_xmlModel.LoadXml(textXml.text);
	}

	void CreateResultFile(string dir, string username){
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
		string filename = Path.Combine(dir, username + ".txt");
		StreamWriter file = null;
		if(!File.Exists(filename)) {
			file = new StreamWriter (filename, true);
			file.WriteLine ("Essai\tCondition\tType de porte\tLargeur de porte\tHauteur de porte\tReponse\tCorpulence utilisateur\t \t \tCorpulence docteur\t \t \tDifference de corpulence\t \t \tTemps de reponse");
		} else {
			file = new StreamWriter (filename, true);
		}
		int condition = PlayerPrefs.GetInt (Utils.PREFS_CONDITION);
		for (int i=0; i < _ordreOuverture.Count; i++) {
			file.WriteLine ((i + 1).ToString () + "\t" + condition.ToString () + "\t" + _doorType.ToString() + "\t" + _ordreOuverture [i].width.ToString() + "\t" + _ordreOuverture [i].height.ToString() + "\t" + (_answers[i] == true ? "1" : "0") + "\t" + modelSrcvalue + "\t" + modelDstvalue + "\t" + modelDiffvalue + "\t" + _ordreOuverture[i].time.ToString());
		}
		file.Close ();
	}

	void createTxT(string username){
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
		
		string fileName = Path.Combine (FilesConst.SAVE_FILES_DIRECTORY, FilesConst.FILENAME_RESULT_TXT);
		StreamWriter fileWritter = null; 
		
		string SEPERATOR = "\t";
		
		if (!File.Exists (fileName)) {
			fileWritter = new StreamWriter (fileName);
			fileWritter.WriteLine ("Participant" + SEPERATOR + 
			                       "Choix patient" + SEPERATOR + SEPERATOR + SEPERATOR + 
			                       "Choix experimentateur" + SEPERATOR + SEPERATOR + SEPERATOR + 
			                       "Difference corpulence" + SEPERATOR + SEPERATOR + SEPERATOR + 
			                       "Type porte" + SEPERATOR + 
			                       "Moyenne largeur OUI" + SEPERATOR + SEPERATOR + SEPERATOR + SEPERATOR + 
			                       "PSE" + SEPERATOR + SEPERATOR + SEPERATOR + SEPERATOR + 
			                       "JND");
			fileWritter.WriteLine (SEPERATOR + SEPERATOR + SEPERATOR + SEPERATOR + SEPERATOR + SEPERATOR +SEPERATOR + SEPERATOR + SEPERATOR + SEPERATOR + SEPERATOR +
			                       "C1" + SEPERATOR + "C2" + SEPERATOR + "C3" + SEPERATOR + "C4" + SEPERATOR +
			                       "C1" + SEPERATOR + "C2" + SEPERATOR + "C3" + SEPERATOR + "C4" + SEPERATOR + 
			                       "C1" + SEPERATOR + "C2" + SEPERATOR + "C3" + SEPERATOR + "C4");
		} else {
			fileWritter = new StreamWriter (fileName, true);
		}
				
		int nbOui = 0;
		float moyenne = 0;
		for (int i=0; i<_answers.Count; i++) {
			if (_answers[i]) {
				moyenne += _ordreOuverture[i].width;
				nbOui ++;
			}
		}
		moyenne = (nbOui > 0)? moyenne/nbOui : 0;
		
		fileWritter.WriteLine (username +SEPERATOR + modelSrcvalue + SEPERATOR + modelDstvalue + SEPERATOR + modelDiffvalue + SEPERATOR + _doorType + SEPERATOR + moyenne);

		fileWritter.Close ();
	}

	void Reponse(bool rep){
		_answers.Add (rep);
		_next = true;
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
		private float _width;
		private float _height;
		private double _time;

		public Measure (float width, float height)
		{
			_width = width;
			_height = height;
			_time = 0;
		}
		

		public float width {
			get {
				return _width;
			}
			set {
				_width = value;
			}
		}

		public float height {
			get {
				return _height;
			}
			set {
				_height = value;
			}
		}

		public double time {
			get {
				return _time;
			}
			set {
				_time = ((int)value)/1000.0;
			}
		}
	}
}