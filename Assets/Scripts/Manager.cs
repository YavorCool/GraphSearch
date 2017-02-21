using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
public class Manager : MonoBehaviour {
	[SerializeField]
	private GameObject vertexPrefab;
	[SerializeField]
	private GameObject edgePrefab;
	[SerializeField]
	private GameObject antPrefab;
	private int antsCreated;
	[SerializeField]
	private int colonySize;
	private int antsCount;
	public bool creatingAnts;
	[SerializeField]
	private float antCreatingRate;
	private float lastAntTime;
	public Camera cam;
	public double ispar;

	private List<GameObject> vertices = new List<GameObject>();
	private List<GameObject> edges = new List<GameObject> ();
	private bool isBuilding = true;
	private bool createFinished = false;
	private bool matrixCreated = false;

	private GameObject chosenVertex1;
	private GameObject chosenVertex2;

	private GameObject startVertex;
	private GameObject endVertex;

	[SerializeField]
	public double [,] graphMatrix;

	public List<GameObject> verticesList {
		get{ 
			return vertices;
		}
	}
		

	public bool IsBuilding{
		get{
			return isBuilding;
		}
		set{ 
			isBuilding = value;
		}
	}

	public bool isCreated{
		get{
			return createFinished;
		}
		set{ 
			createFinished = value;
		}
	}

	public GameObject chooseVertex1 {
		get { 
			return chosenVertex1;
		}

		set { 
			chosenVertex1 = value;
			chosenVertex1.GetComponent<SpriteRenderer> ().color = new Color(0.5f, 0.7f, 0.4f, 1f);
		}
	}

	public GameObject chooseVertex2 {
		get { 
			return chosenVertex2;
		}
		set { 
			chosenVertex2 = value;
		}
	}


	public GameObject start {
		get { 
			return startVertex;
		}

		set { 
			startVertex = value;
			startVertex.GetComponent<SpriteRenderer> ().color = new Vector4(0.35f,0.49f,0.1f, 1f);
		}
	}

	public GameObject end {
		get { 
			return endVertex;
		}
		set { 
			endVertex = value;
			endVertex.GetComponent<SpriteRenderer> ().color = new Vector4(0.2f,0.19f,0.7f,1);
		}
	}


	// Use this for initialization
	void Start () {
	}

	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && isBuilding) {
			Vector3 m_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (m_pos.x < cam.ScreenToWorldPoint(new Vector3(Screen.width - 200, 0, 0)).x) {
				m_pos.z = 0;
				if (vertexNear (m_pos)) {
					return;
				}
				GameObject newVertex = Instantiate (vertexPrefab, m_pos, Quaternion.identity) as GameObject;
				vertices.Add (newVertex);
			}
		}
		if (createFinished && !matrixCreated) {
			CreateMatrix ();
			matrixCreated = true;
		}

		if (creatingAnts && antsCount< colonySize && Time.time > lastAntTime + antCreatingRate) {
			if (antsCount == colonySize - 1) {
				CreateAnt (true);
				creatingAnts = false;
			}
			CreateAnt (false);
			antsCount++;
			lastAntTime = Time.time;
		}

	}

	public void newColony(){
		antsCount = 0;
		creatingAnts = true;
	}


	private bool vertexNear(Vector3 coords){
		foreach (GameObject vertex in vertices) {			
			if (Vector3.Distance (vertex.transform.position, coords) < 1.5) {
				return true;
			}
		}
		return false;
	}


	public void CreateEdge(){
		GameObject newEdge = Instantiate (edgePrefab);
		newEdge.GetComponent<Edge>().v1 = chosenVertex1;
		newEdge.GetComponent<Edge>().v2 = chosenVertex2;
		newEdge.GetComponent<Edge>().setDist();
		newEdge.transform.position = (newEdge.GetComponent<Edge> ().v2.transform.position + newEdge.GetComponent<Edge> ().v1.transform.position)/2;
		edges.Add (newEdge);
		chosenVertex1.GetComponent<SpriteRenderer> ().color =   new Vector4 (0,0,0, 255);
		chosenVertex1 = null;
		chosenVertex2 = null;
	}

	public void CreateAnt( bool isLast){
		GameObject newAnt = Instantiate (antPrefab, startVertex.transform.position, Quaternion.identity) as GameObject;
		newAnt.GetComponent<MoveAnt> ().last = isLast;
	}



	public void CreateMatrix(){ 
		int i, j;
		graphMatrix = new double[vertices.Count,vertices.Count];

		for (i = 0; i < vertices.Count; i++) {
			for (j = 0; j < edges.Count; j++) {
				if (vertices [i] == edges [j].GetComponent<Edge> ().v1) {
					graphMatrix [i,vertices.IndexOf (edges [j].GetComponent<Edge> ().v2)] = edges [j].GetComponent<Edge> ().dist;
				}
			}
		}

	}
		

	GameObject getMinCostVertex(List<GameObject> open){
		GameObject minCostVertex = open[0];
		double minCost = open[0].GetComponent<Vertex>().sumCost;
		foreach (GameObject vertex in open) {
			if (vertex.GetComponent<Vertex>().sumCost < minCost) {
				minCost = vertex.GetComponent<Vertex> ().sumCost;
				minCostVertex = vertex;
			}
		}

		return minCostVertex;
	}

	public GameObject getEdge(GameObject start, GameObject end){
		for (int i = 0; i < edges.Count; i++) {
			if (start == edges [i].GetComponent<Edge> ().v1 && end == edges [i].GetComponent<Edge> ().v2) {
				return edges [i];
			}
		}
		return null;
	}

	public void initEdges(){
		foreach (GameObject edge in edges) {
			edge.GetComponent<Edge> ().pher = 0.1;
		}
	}

	public void updateEdges(){
		foreach (GameObject edge in edges) {
			edge.GetComponent<Edge> ().pher *= (1.0 - ispar);
		}

	}

	public void resetColors (){
		foreach (GameObject vertex in vertices) {
			vertex.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0, 1);
		}
	}
}
