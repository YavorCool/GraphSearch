using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChooseVertex : MonoBehaviour {
	private GameObject currentVertex;
	private List<GameObject> visited = new List<GameObject>();

	private List<GameObject> edges;
	private List<GameObject> vertices;
	public double[,] graphMatrix;

	private GameObject startVertex;
	public GameObject endVertex;

	public double alpha, beta;
	public double deltaPher;




	void Start(){
		graphMatrix = GameObject.FindWithTag("Manager").GetComponent<Manager> ().graphMatrix;
		vertices = GameObject.FindWithTag("Manager").GetComponent<Manager> ().verticesList;
		startVertex = GameObject.FindWithTag("Manager").GetComponent<Manager> ().start;
		endVertex = GameObject.FindWithTag("Manager").GetComponent<Manager> ().end;
		currentVertex = startVertex;
	}


	// Use this for initialization
	public void chooseNextVertex(){
		visited.Add (currentVertex);
		List<int> probabilities = new List<int>();
		List<GameObject> toVisit = new List<GameObject> ();
		double psum = 0;
		for (int i = 0; i < vertices.Count; i++) {
			if (graphMatrix [vertices.IndexOf (currentVertex), i] > 0 && !visited.Contains (vertices [i])) {
				GameObject edge = GameObject.FindWithTag ("Manager").GetComponent<Manager> ().getEdge (currentVertex, vertices [i]);
				double eta = 1 / edge.GetComponent<Edge>().dist;
				double tau = edge.GetComponent<Edge> ().pher;
				psum += Math.Pow (tau, alpha) * Math.Pow (eta, beta);
				toVisit.Add (vertices [i]);
			}
		}

		probabilities.Add (0);
		for (int i = 0; i < toVisit.Count; i++) {
			GameObject edge = GameObject.FindWithTag ("Manager").GetComponent<Manager> ().getEdge (currentVertex, toVisit[i]);
			double eta = 1 / edge.GetComponent<Edge>().dist;
			double tau = edge.GetComponent<Edge> ().pher;
			double p = Math.Pow (tau, alpha) * Math.Pow (eta, beta) / psum;
			p = Math.Round (p, 2);
			probabilities.Add ((int)(p * 100) + probabilities[i]);
		}

		GameObject chosenEdge = null;
		System.Random rnd = new System.Random();
		int num = rnd.Next (0, 100);
		for (int i = 0; i < probabilities.Count-1; i++) {
			if (probabilities [i] <= num && num < probabilities [i + 1]) {
				gameObject.GetComponent<MoveAnt> ().moveTo = toVisit [i].transform.position;
				//toVisit [i].GetComponent<SpriteRenderer> ().color = new Color (1,0,0,1);
				visited.Add (toVisit [i]);
				chosenEdge = GameObject.FindWithTag ("Manager").GetComponent<Manager> ().getEdge (currentVertex, toVisit[i]);
				currentVertex = toVisit [i];
				break;
			}
		}

		if (chosenEdge != null) {
			chosenEdge.GetComponent<Edge> ().pher += deltaPher;
		}

	}
}
