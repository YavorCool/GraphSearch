using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AStar : MonoBehaviour {

	private List<GameObject> vertices;
	public double[,] graphMatrix;

	public GameObject startVertex;
	public GameObject endVertex;

	public bool paused = false;

	public GameObject currentVertex;

	public float simRate;
	private float lastStepTime;

	private double pathLength;

	private bool showPath = false;
	private bool simulate = false;

	List<GameObject> openList = new List<GameObject> ();
	List<GameObject> closedList = new List<GameObject> ();

	public bool start{
		set{ 
			simulate = value;
		}
	}

	// Use this for initialization
	void Start () {
	}


	public void initGraph(){
		graphMatrix = gameObject.GetComponent<Manager> ().graphMatrix;
		vertices = gameObject.GetComponent<Manager> ().verticesList;
		startVertex = gameObject.GetComponent<Manager> ().start;
		endVertex = gameObject.GetComponent<Manager> ().end;
		foreach (GameObject vertex in vertices) {
			vertex.GetComponent<Vertex> ().costToEnd = Vector3.Distance (vertex.transform.position, endVertex.transform.position);
		}

		lastStepTime = Time.time;
		openList.Add (startVertex);
		currentVertex = startVertex;
		currentVertex.GetComponent<SpriteRenderer>().color = new Color (0,1,1,1);
	}
	
	// Update is called once per frame
	void Update () {
		if (!paused) {
			if (currentVertex != endVertex && openList.Count != 0 && Time.time > lastStepTime + simRate && simulate) {
				openList.Remove (currentVertex);
				currentVertex.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 1);
				closedList.Add (currentVertex);
				for (int i = 0; i < vertices.Count; i++) {
					if (graphMatrix [vertices.IndexOf (currentVertex), i] > 0 && !closedList.Contains (vertices [i])) {
						if (openList.Contains (vertices [i])) {
							if (currentVertex.GetComponent<Vertex> ().realCost > vertices [i].GetComponent<Vertex> ().realCost) {
								vertices [i].GetComponent<Vertex> ().parentVertex = currentVertex;
								vertices [i].GetComponent<Vertex> ().realCost = currentVertex.GetComponent<Vertex> ().realCost + graphMatrix [vertices.IndexOf (currentVertex), i];
								vertices [i].GetComponent<Vertex> ().sumCost = vertices [i].GetComponent<Vertex> ().realCost + vertices [i].GetComponent<Vertex> ().costToEnd;
							}
						} else {
							vertices [i].GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0, 1);
							vertices [i].GetComponent<Vertex> ().realCost = currentVertex.GetComponent<Vertex> ().realCost + graphMatrix [vertices.IndexOf (currentVertex), i];
							vertices [i].GetComponent<Vertex> ().sumCost = vertices [i].GetComponent<Vertex> ().realCost + vertices [i].GetComponent<Vertex> ().costToEnd;
							vertices [i].GetComponent<Vertex> ().parentVertex = currentVertex;
							openList.Add (vertices [i]);
						}
					}
				}
				currentVertex = getMinCostVertex (openList);
				currentVertex.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 1, 1);
				lastStepTime = Time.time;
			} else {
				if (currentVertex == endVertex && currentVertex != null) {
					showPath = true;
					simulate = false;
				}
				
			}
			if (showPath && Time.time > lastStepTime + simRate) {
				pathLength += graphMatrix [vertices.IndexOf (currentVertex.GetComponent<Vertex> ().parentVertex), vertices.IndexOf (currentVertex)];
				currentVertex = currentVertex.GetComponent<Vertex> ().parentVertex;
				currentVertex.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 0, 1);
				if (currentVertex == startVertex) {
					showPath = false;
					Debug.Log ("Длина пути:" + pathLength);
				}
				lastStepTime = Time.time;
			}
		}
	}


	public void clearGraph(){
		currentVertex = startVertex;
		openList.Clear();
		closedList.Clear();
		gameObject.GetComponent<Manager> ().resetColors ();
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
}