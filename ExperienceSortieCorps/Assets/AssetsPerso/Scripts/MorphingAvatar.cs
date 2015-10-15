using UnityEngine;
using System.Collections;

public class MorphingAvatar : MonoBehaviour {
	
	public Mesh dstMesh;
	public Mesh srcMesh;
	private Mesh mesh;
	private bool initDone;
	private bool startMorph;
	private float time;
	private float _speed;
	//public GameObject[] lineRenderers;
	//Vector3 init = new Vector3(-1.68f,0.88f,-5.19f);
	// Use this for initialization
	void Start () {
		time = 0.0f;
		mesh = Instantiate(srcMesh) as Mesh;
		/*mesh.vertices = srcMesh.vertices;
		mesh.triangles = srcMesh.triangles;
		mesh.uv = srcMesh.uv;
		mesh.normals = srcMesh.normals;
		mesh.colors = srcMesh.colors;
		mesh.tangents = srcMesh.tangents;
		mesh.RecalculateBounds ();*/
		//GetComponent<SkinnedMeshRenderer> ().sharedMesh = srcMesh;
		GetComponent<SkinnedMeshRenderer> ().sharedMesh = mesh;

		//Vector3[] v0 = GetComponent<SkinnedMeshRenderer> ().sharedMesh.vertices;
		/*lineRenderers = new GameObject[mesh.vertices.Length];
		for (int i = 0; i< lineRenderers.Length; i++) {
			lineRenderers [i] = (GameObject)Instantiate (Resources.Load ("LineRenderer", typeof(GameObject)));
			lineRenderers[i].GetComponent<LineRenderer>().SetPosition(0,init+new Vector3(v0[i].z,v0[i].y,-v0[i].x));
			lineRenderers[i].GetComponent<LineRenderer>().SetPosition(1,init+new Vector3(v0[i].z,v0[i].y,-v0[i].x));
		}*/

		initDone = true;
		if (dstMesh == null) {
			Debug.Log ("dstMesh est null");
			initDone = false;
			return;
		}

		if (srcMesh == null) {
			Debug.Log ("srcMesh est null");
			initDone = false;
			return;
		}

		if (dstMesh.vertexCount != srcMesh.vertexCount) {
			Debug.Log ("nombre de vertex different");
			initDone = false;
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
	//	SixenseInput.Controller controller = SixenseInput.GetController( SixenseHands.RIGHT );

		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.F)/* || controller.GetButton( SixenseButtons.TWO )*/) {
			startMorph = true;
		}
		if (initDone && startMorph) {
			float deltaTime = Time.deltaTime * _speed;
			time += deltaTime;
			float tmp = Mathf.Clamp(time,0,1);
			Morph(tmp);
		}
	}

	void Morph(float t){
		Vector3[] v0 = srcMesh.vertices;
		Vector3[] v1 = dstMesh.vertices;
		Vector3[] vdst = new Vector3[mesh.vertexCount];
		for (int i=0; i<vdst.Length; i++) {
			vdst [i] = Vector3.Lerp (v0 [i], v1 [i], t);
		}
		GetComponent<SkinnedMeshRenderer> ().sharedMesh.vertices = vdst;
		GetComponent<SkinnedMeshRenderer> ().sharedMesh.RecalculateBounds ();
		/*v1 = GetComponent<SkinnedMeshRenderer> ().sharedMesh.vertices;
		for (int i=0; i<lineRenderers.Length; i++) {
			lineRenderers[i].GetComponent<LineRenderer>().SetPosition(1,init+new Vector3(v1[i].z,v1[i].y,-v1[i].x));
		}*/
	}

/*	public void OnRenderObject ()
	{
		GL.PushMatrix();
		GL.Begin(GL.LINES);
		
		GL.modelview = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		
		foreach(int triangle in mesh.triangles)
		{
			GL.Vertex(vertex1);
			GL.Color(color1);
			GL.Vertex(vertex2);
			GL.Color(color2);
			
			GL.Vertex(vertex2);
			GL.Color(color2);
			GL.Vertex(vertex3);
			GL.Color(color3);
			
			GL.Vertex(vertex3);
			GL.Color(color3);
			GL.Vertex(vertex1);
			GL.Color(color1);
		}
		
		GL.End();
		GL.PopMatrix();
	}*/

	public float speed {
		get {
			return _speed;
		}
		set {
			_speed = value;
		}
	}

}
