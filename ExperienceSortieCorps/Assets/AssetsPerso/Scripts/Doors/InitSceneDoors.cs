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

	string SEPARATOR = "\t";
	
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

					string directory = PlayerPrefs.GetString (Utils.PREFS_PATH_FOLDER);
					if(!string.Empty.Equals(directory)){
						string username = directory.Remove (0, directory.LastIndexOf('\\') + 1).Split ('_')[0];

						CreateResultFile(directory, username);
						createTxT(username);

						CallPythonScript(username, PlayerPrefs.GetInt (Utils.PREFS_CONDITION), Path.Combine(directory, username + ".txt"));
					}
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

		string[] parameters = resSocket.Split ('_');

		int nbTries = int.Parse (parameters [0]);

		int nbWidth = int.Parse(parameters [1]);
		int nbHeight;
		if(_doorType != FilesConst.FULL_DOOR)
			nbHeight = 0;
		else
			nbHeight = int.Parse(parameters [3]);

		if (nbWidth > 0) {
			int widthStep = int.Parse (parameters [2]);
			int heightStep = int.Parse (parameters [4]);
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

		for (int i = 0; i < nbTries; i++) {
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
			modelSrcvalue = _modelSrcValues [0].ToString () + SEPARATOR + _modelSrcValues [1].ToString () + SEPARATOR + _modelSrcValues [2].ToString ();
			modelDstvalue = _modelDstValues [0].ToString () + SEPARATOR + _modelDstValues [1].ToString () + SEPARATOR + _modelDstValues [2].ToString ();
			modelDiffvalue = _differenceModels [0].ToString () + SEPARATOR + _differenceModels [1].ToString () + SEPARATOR + _differenceModels [2].ToString ();
		} else {
			modelSrcvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
			modelDstvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
			modelDiffvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
		}
		string filename = Path.Combine(dir, username + ".txt");
		StreamWriter file = null;
		if(!File.Exists(filename)) {
			file = new StreamWriter (filename, true);
			file.WriteLine ("Essai" + SEPARATOR + 
			                "Condition" + SEPARATOR + 
			                "Type de porte" + SEPARATOR + 
			                "Largeur de porte" + SEPARATOR + 
			                "Hauteur de porte" + SEPARATOR + 
			                "Reponse" + SEPARATOR + 
			                "Corpulence utilisateur" + SEPARATOR + SEPARATOR + SEPARATOR + 
			                "Corpulence docteur" + SEPARATOR + SEPARATOR + SEPARATOR + 
			                "Difference de corpulence" + SEPARATOR + SEPARATOR + SEPARATOR + 
			                "Temps de reponse");
		} else {
			file = new StreamWriter (filename, true);
		}
		int condition = PlayerPrefs.GetInt (Utils.PREFS_CONDITION);
		for (int nbDoor=0; nbDoor < _ordreOuverture.Count; nbDoor++) {
			file.WriteLine ((nbDoor + 1).ToString () + SEPARATOR + condition.ToString () + SEPARATOR + _doorType.ToString() + SEPARATOR + _ordreOuverture [nbDoor].width.ToString() + SEPARATOR + _ordreOuverture [nbDoor].height.ToString() + SEPARATOR + (_answers[nbDoor] == true ? "1" : "0") + SEPARATOR + modelSrcvalue + SEPARATOR + modelDstvalue + SEPARATOR + modelDiffvalue + SEPARATOR + _ordreOuverture[nbDoor].time.ToString());
		}
		file.Close ();
	}

	void createTxT(string username){
		string modelSrcvalue;
		string modelDstvalue;
		string modelDiffvalue;
		if (_modelSrcValues != null) {
			modelSrcvalue = _modelSrcValues [0].ToString () + SEPARATOR + _modelSrcValues [1].ToString () + SEPARATOR + _modelSrcValues [2].ToString ();
			modelDstvalue = _modelDstValues [0].ToString () + SEPARATOR + _modelDstValues [1].ToString () + SEPARATOR + _modelDstValues [2].ToString ();
			modelDiffvalue = _differenceModels [0].ToString () + SEPARATOR + _differenceModels [1].ToString () + SEPARATOR + _differenceModels [2].ToString ();
		} else {
			modelSrcvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
			modelDstvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
			modelDiffvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
		}
		
		string fileName = Path.Combine (FilesConst.SAVE_FILES_DIRECTORY, FilesConst.FILENAME_RESULT_TXT);
		StreamWriter fileWritter = null; 
		
		bool newFile = false;
		
		if (!File.Exists (fileName)) {
			fileWritter = new StreamWriter (fileName);
			fileWritter.WriteLine ("Participant" + SEPARATOR + 
			                       "Choix patient" + SEPARATOR + SEPARATOR + SEPARATOR + 
			                       "Choix experimentateur" + SEPARATOR + SEPARATOR + SEPARATOR + 
			                       "Difference corpulence" + SEPARATOR + SEPARATOR + SEPARATOR + 
			                       "Type porte" + SEPARATOR + 
			                       "Moyenne largeur OUI" + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + 
			                       "PSE" + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + 
			                       "JND");
			fileWritter.WriteLine (SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR +SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR + SEPARATOR +
			                       "C1" + SEPARATOR + "C2" + SEPARATOR + "C3" + SEPARATOR + "C4" + SEPARATOR +
			                       "C1" + SEPARATOR + "C2" + SEPARATOR + "C3" + SEPARATOR + "C4" + SEPARATOR + 
			                       "C1" + SEPARATOR + "C2" + SEPARATOR + "C3" + SEPARATOR + "C4");
			
			newFile = true;
			fileWritter.Close ();
		} 
		
		int nbOui = 0;
		float moyenne = 0;
		for (int i=0; i<_answers.Count; i++) {
			if (_answers[i]) {
				moyenne += _ordreOuverture[i].width;
				nbOui ++;
			}
		}
		moyenne = (nbOui > 0) ? moyenne / nbOui : 0;
		
		float pse = 0;
		float jnd = 0;

		string[] lines = File.ReadAllLines (fileName);

		string[] parameters = lines[lines.Length -1].Split('\t');
		
		int condition = PlayerPrefs.GetInt (Utils.PREFS_CONDITION);

		string emptyParam = "/";

		if (!parameters [0].Equals (username) || newFile) {	// Ajoute une ligne
			fileWritter = new StreamWriter (fileName, true);
			string res = username + SEPARATOR + modelSrcvalue + SEPARATOR + modelDstvalue + SEPARATOR + modelDiffvalue + SEPARATOR + _doorType + SEPARATOR;
			for(int i=1; i<5; i++) {
				if(condition == i) res += moyenne + SEPARATOR;
				else res += emptyParam + SEPARATOR;
			}
			for(int i=1; i<5; i++) {
				if(condition == i) res += pse.ToString() + SEPARATOR;
				else res += emptyParam + SEPARATOR;
			}
			for(int i=1; i<5; i++) {
				if(condition == i) res += jnd.ToString() + SEPARATOR;
				else res += emptyParam + SEPARATOR;
			}
			fileWritter.WriteLine (res);
			fileWritter.Close ();
		} else {	// Met a jour la derniere ligne
			string res = "";

			for(int i = 0; i < 11; i++)
				res += parameters[i] + SEPARATOR;

			res += WriteOrUpdateMoyenne(condition, moyenne, parameters);
			res += WriteOrUpdatePSE(condition, pse, parameters);
			res += WriteOrUpdateJND(condition, jnd, parameters);

			lines[lines.Length -1] = res;
			File.WriteAllLines(fileName, lines);
		}
	}

	/// <summary>
	/// Appelle le script python
	/// </summary>
	/// <param name="username">Le nom de l'utilisateur effectuant l'exercice</param>
	/// <param name="condition">La condition d'expérimentation</param>
	/// <param name="resultFilename">Le fichier de résultat généré pour l'utilisateur</param>
	void CallPythonScript(string username, int condition, string resultFilename){
		new System.Diagnostics.Process () {
			StartInfo = 
			{
				FileName = "cmd.exe",
				Arguments = "/c py drawGraphe.py " + username + " " + condition + " \"" + resultFilename + " \"",
				WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
			}
		}.Start();
	}

	string WriteOrUpdateMoyenne(int condition, float moyenne, string[] parameters){
		return WriteOrUpdateParameter(condition, parameters, moyenne, 10);
	}
	
	string WriteOrUpdatePSE(int condition, float pse, string[] parameters){
		return WriteOrUpdateParameter(condition, parameters, pse, 14);
	}
	
	string WriteOrUpdateJND(int condition, float jnd, string[] parameters){
		return WriteOrUpdateParameter(condition, parameters, jnd, 18);
	}

	string WriteOrUpdateParameter(int condition, string[] parameters, float newValue, int baseIndex){
		string res = "";
		for(int i = 1; i < 5; i++) {
			if(condition == i)
				res += newValue.ToString() + SEPARATOR;
			else 
				res += parameters[baseIndex + i] + SEPARATOR;
		}
		return res;
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
		for(int i = 0; i < resultat.Length; i++)
			resultat [i] = (dst [i] - src [i]) / src [i] * 100; 
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