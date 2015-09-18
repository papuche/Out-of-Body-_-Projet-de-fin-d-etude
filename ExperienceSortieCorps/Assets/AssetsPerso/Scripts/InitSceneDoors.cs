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

	public GameObject piece;
	public GameObject text;

	// Gestion de la largeur des portes
	int scalesNumber=30;
	float scalesMin=0.3f;
	float scalesMax=1.2f;
	List<float> scales = new List<float>();
	int doorWidthIndex;		// Index pointant dans scales
	Vector3 currentScale;	// Dimension de la porte dans la scène

	// Nombre de réponses jouées (affichées dans la scène)
	int nbAnswers=0;

	// Liste où sont enregistrés les largeur de portes jouées (pour le fichier de résultats)
	List<float> ordreOuverture = new List<float>();

	// Modèle sélectionné par le sujet
	float[] modelSrcValues;
	// Modèle sélectionné par le psychologue
	float[] modelDstValues;
	// Ecart de morphologie
	float[] differenceModels;

	// Fichier de résultats
	StreamWriter file;
	string path;
	string fileName;
	TextAsset textXml;
	XmlDocument xmlDoc;
	XmlDocument xmlModel;

	List<bool> answers = new List<bool>();
	bool next = false;
	bool stop;

	int sujet;
	int session;
	int condition;

	void Start () {

		// Assignation de labels à la condition du test et au sujet du test
		sujet = PlayerPrefs.GetInt ("Sujet", 0);
		condition = PlayerPrefs.GetInt ("Condition");		

		// Création du fichier de résultats
		fileName = System.DateTime.Now.ToString ();
		fileName = fileName.Replace ("/", "-");
		fileName = fileName.Replace (":", "-");

		// Initialisation du tableau conprenant la largeur des portes.
		initScales ();

		createXML ();
		loadXMLFromAssest ();

		// Debug.Log (PlayerPrefs.GetString ("Model"));

		// Recupération du nom des modèles dans modelName
		string[] modelName = PlayerPrefs.GetString ("Model").Split (';');	
		modelSrcValues = ReadModelsValue (modelName [0].Split ('/') [1]);	// Les modèles du sujet sont enregistrés en [0]
		modelDstValues = ReadModelsValue (modelName [1].Split ('/') [1]);	// Les modèles du phychologue sont enregistrés en [1]
		differenceModels = calculEcart (modelSrcValues, modelDstValues);

		// Mise à jour de la largeur de porte.
		updateWidthDoor ();

		// Ecriture de la largeur de porte 
		file = new StreamWriter ("Resultat.txt",true);

		// Utile pour la fonction update
		stop = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!stop){

			if (Input.GetMouseButtonDown (0) || Input.GetKeyDown(KeyCode.O)) {
				Reponse(true);
				next = true;
			} else if (Input.GetMouseButtonDown (1) || Input.GetKeyDown(KeyCode.N)) {
				Reponse(false);
				next = true;
			}

			if(next){
				if(scales.Count > 0){
					// Mise à jour de la largeur de porte.
					updateWidthDoor ();
				}
				else {
					stop=true;
					modifyXml ();
					modifyTxT();
					file.Close ();
					Application.Quit();
				}
				next = false;
			}
		}
	}

	/**
	 * 
	 * 		Initialisation du tableau conprenant la largeur des portes.
	 * 
	 **/
	void initScales(){
		for (int i=0; i<scalesNumber; i++) {
			scales.Add (Random.Range (scalesMin, scalesMax));
		}
	}

	/**
	 * 
	 * 		Sélection aléatoire d'un index du tableau conprenant la largeur des portes.
	 * 
	 **/
	int selectRamdomIndex(){
		return Random.Range (0, scales.Count);
	}

	/**
	 * 
	 * 		Mise à jour de la largeur de porte.
	 * 
	 **/
	void updateWidthDoor(){

		doorWidthIndex = selectRamdomIndex ();		// Récupération aléatoire d'un index
		currentScale = piece.transform.localScale;	// Récupération de l'échelle de la porte dans la scène
		currentScale.x = scales[doorWidthIndex];	// Affectation de la coordonée sélectionée dans scales[] sur currentScale.x (largeur de currentScale)
		
		// Ajout de la largeur dans ordreOuverture (pour le fichier de résultats) et suppression de la porte sélectionée dans scales
		ordreOuverture.Add (currentScale.x);
		scales.RemoveAt(doorWidthIndex);
		
		// Modification de la largeur de porte dans la scène.
		piece.transform.localScale = currentScale;	
		
		// Ecriture de l'avancement dans la scène
		nbAnswers++;
		text.GetComponent<Text>().text = nbAnswers.ToString()+"/"+scalesNumber.ToString();
	}

	void loadXMLFromAssest(){
		
		xmlModel = new XmlDocument();
		textXml = (TextAsset)Resources.Load("Models", typeof(TextAsset));
		xmlModel.LoadXml(textXml.text);
	}

	void modifyTxT(){
		int essai = 1;
		string modelSrcvalue = modelSrcValues [0].ToString () + "\t" + modelSrcValues [1].ToString () + "\t" + modelSrcValues [2].ToString ();
		string modelDstvalue = modelDstValues [0].ToString () + "\t" + modelDstValues [1].ToString () + "\t" + modelDstValues [2].ToString ();
		string modelDiffvalue = differenceModels [0].ToString () + "\t" + differenceModels [1].ToString () + "\t" + differenceModels [2].ToString ();
		foreach (bool answer in answers) {
			if (answer) {
				file.WriteLine(sujet.ToString()+"\t"+condition.ToString()+"\t"+essai.ToString()+"\t"+ordreOuverture[essai-1].ToString()+"\t"+"1"+"\t"+modelSrcvalue+"\t"+modelDstvalue+"\t"+modelDiffvalue);
			} else {
				file.WriteLine(sujet.ToString()+"\t"+condition.ToString()+"\t"+essai.ToString()+"\t"+ordreOuverture[essai-1].ToString()+"\t"+"0"+"\t"+modelSrcvalue+"\t"+modelDstvalue+"\t"+modelDiffvalue);
			}
			essai++;
		}
	}
	void modifyXml(){
		int indexlist = 0;
		XmlNode nodeact = xmlDoc.SelectSingleNode("Ouvertures");
		foreach(XmlElement node in nodeact.SelectNodes("Ouverture")){
			node.FirstChild.InnerText = ordreOuverture[indexlist].ToString();
			if(answers[indexlist]){
				node.LastChild.InnerText = "Oui";
			}else{
				node.LastChild.InnerText = "Non";
			}
			indexlist++;
		}

		xmlDoc.Save(fileName+".xml");
	}

	public void Reponse(bool rep){
		answers.Add (rep);
		next = true;
	}

	void createXML(){
		xmlDoc = new XmlDocument ();
		XmlElement root = (XmlElement)xmlDoc.AppendChild(xmlDoc.CreateElement("Ouvertures"));

		for(int i = 0; i < scales.Count; i++){
			XmlElement el = (XmlElement)root.AppendChild(xmlDoc.CreateElement("Ouverture"));
			el.AppendChild(xmlDoc.CreateElement("Taille"));
			el.AppendChild(xmlDoc.CreateElement("Reponse"));
		}
	}

	float[] ReadModelsValue(string name){
		string nameNode= "", waist = "", hips= "", chest= "";
		foreach (XmlElement node in xmlModel.SelectNodes("Models/Model")) {
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
}