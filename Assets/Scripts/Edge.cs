using UnityEngine;
using System.Collections;
using System;


public class Edge : MonoBehaviour {
	private GameObject vertex1;
	private GameObject vertex2;
	private float distance;
	private double pheromones;

	public GameObject v1 {
		get { 
			return vertex1;
		}
		set { 
			vertex1 = value;
			gameObject.GetComponent<LineRenderer> ().SetPosition (0, vertex1.transform.position);
		}
	}

	public GameObject v2 {
		get { 
			return vertex2;
		}
		set { 
			vertex2 = value;
			gameObject.GetComponent<LineRenderer> ().SetPosition (1, vertex2.transform.position);
		}
	}

	public float dist {
		get{ 
			return distance;
		}
	}

	public void setDist(){
		distance = Vector3.Distance (vertex1.transform.position, vertex2.transform.position);
	}

	public double pher {
		get { 
			return pheromones;
		}

		set { 
			pheromones = value;
		}
	}
		

	void OnMouseOver(){
		string line = "";
		line += "Длина: " + Math.Round (distance, 2) + "\n";
		line += "Феромоны: " + Math.Round (pheromones, 2);

		GameObject.FindWithTag ("Manager").GetComponent<UIController> ().setInfoText (line);
	}
}
