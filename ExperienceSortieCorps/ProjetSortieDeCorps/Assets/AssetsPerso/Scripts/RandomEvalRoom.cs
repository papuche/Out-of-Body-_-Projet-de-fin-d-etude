using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;

public class RandomEvalRoom : MonoBehaviour {

	public GameObject porte;
	public GameObject text;
	public Material doorMat;
	public Material roomMat;
	float offset = 0.05f;

	List<float> scales;
	Color[] colors = new Color[10];
	Color defaultDoorColor;
	Color defaultRoomColor;
	int rndNumber;
	float scale;
	Vector3 tmp;
	int nbAnswers = 1;
	int nbTotalScales;
	string path;
	string fileName;
	XmlDocument xmlDoc;
	XmlDocument xmlModel;
	TextAsset textXml;
	List<float> ordreOuverture;
	StreamWriter file;
	float[] modelSrcValues;
	float[] modelDstValues;
	float[] differenceModels;
	List<bool> answers;
	bool next;
	bool stop;

	int sujet;
	int session;
	int condition;
	// Use this for initialization
	void Start () {
		//loadXMLFromAssest ();

		sujet = PlayerPrefs.GetInt ("Sujet", 0);
		condition = PlayerPrefs.GetInt ("Condition");
		fileName = System.DateTime.Now.ToString ();

		fileName = fileName.Replace ("/", "-");
		fileName = fileName.Replace (":", "-");

		ordreOuverture = new List<float>();

		answers = new List<bool>();
		next = false;

		defaultDoorColor = doorMat.color;
		defaultRoomColor = roomMat.color;
		initColors ();
		initScales ();
		createXML ();
		loadXMLFromAssest ();
		string[] modelName = PlayerPrefs.GetString ("Model").Split (';');
		modelSrcValues = ReadModelsValue (modelName [0].Split ('/') [1]);
		modelDstValues = ReadModelsValue (modelName [1].Split ('/') [1]);
		differenceModels = calculEcart (modelSrcValues, modelDstValues);
		rndNumber = (int)Random.Range(0.0f,10.0f);
		//doorMat.color = colors[rndNumber];
		rndNumber = (int)Random.Range(0.0f,10.0f);
		//roomMat.color = colors[rndNumber];
		rndNumber = (int)Random.Range(0.0f,(float)scales.Count);
		tmp = porte.transform.localScale;
		tmp.x = scales[rndNumber];
		ordreOuverture.Add (tmp.x);
		scales.RemoveAt(rndNumber);
		porte.transform.localScale = tmp;
//		StreamReader fileR = new StreamWriter ("Resultat.txt");
//		fileR.
		file = new StreamWriter ("Resultat.txt",true);
		//file.WriteLine ("1\t1\t1\t2.2\t1");

		stop = false;
		nbTotalScales = scales.Count + 1;
		text.GetComponent<Text>().text = nbAnswers.ToString()+"/"+nbTotalScales.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if(!stop){
			if (Input.GetKeyDown (KeyCode.O) || Input.GetMouseButtonDown(0)) {
				Reponse(true);
				next = true;
			} else if (Input.GetKeyDown (KeyCode.N) || Input.GetMouseButtonDown(1)) {
				Reponse(false);
				next = true;
			}
			if(next){
				rndNumber = (int)Random.Range(0.0f,10.0f);
				//doorMat.color = colors[rndNumber];
				rndNumber = (int)Random.Range(0.0f,10.0f);
				//roomMat.color = colors[rndNumber];
				if(scales.Count > 0){
					int nbTry = 0;
					tmp = porte.transform.localScale;
					do{
						rndNumber = Random.Range(0,(scales.Count-1));
						//rndNumber = (int)Random.Range(0.0f,((float)scales.Count-1.0f));
						nbTry++;
						if(nbTry > 10){
							break;
						}
					}while(tmp.x == scales[rndNumber]);
					
					tmp.x = scales[rndNumber];
					ordreOuverture.Add (tmp.x);
					scales.RemoveAt(rndNumber);
					porte.transform.localScale = tmp;
				}else{
					stop=true;
					//modifyXml ();
					modifyTxT();
					file.Close ();
					//Application.Quit();
					Application.LoadLevel(Application.loadedLevel+1);
				}
				next = false;
				nbAnswers++;
				text.GetComponent<Text>().text = nbAnswers.ToString()+"/"+nbTotalScales.ToString();
			}
		}
	}

	void OnApplicationQuit(){
		doorMat.color = defaultDoorColor;
		roomMat.color = defaultRoomColor;
		
	}

	void initColors(){
		colors [0] = Color.black;
		colors [1] = Color.blue;
		colors [2] = Color.cyan;
		colors [3] = Color.gray;
		colors [4] = Color.green;
		colors [5] = Color.grey;
		colors [6] = Color.magenta;
		colors [7] = Color.red;
		colors [8] = Color.white;
		colors [9] = Color.yellow;
	}

	void initScales(){
		scales = new List<float> ();
		List<float> rangescales = new List<float> ();
		//int nb = offset / 2.5f;
		float scale = 0.25f;
		rangescales.Add (0.25f);
		rangescales.Add (0.60f);
		rangescales.Add (0.75f);
		rangescales.Add (0.90f);
		rangescales.Add (1.0f);
		rangescales.Add (1.2f);
		/*for (int i = 0; i < 10; i++) {
			scales.Add (scale);
			scale += offset;
		}*/
		int i = 0;
		while (i<5) {
			scales.AddRange(rangescales);
			i++;
		}
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
		//xmlDoc.Save(Application.dataPath +"/Resources/"+fileName+".xml");
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