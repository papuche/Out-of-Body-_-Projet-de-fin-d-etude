using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

public class EvalRoom : MonoBehaviour {

	public GameObject ouverture;

	List<float> scales;
	int rndNumber;
	float scale;
	Vector3 tmp;

	string path;
	string fileName;
	XmlDocument xmlDoc;
	TextAsset textXml;

	List<bool> answers;
	bool next;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
