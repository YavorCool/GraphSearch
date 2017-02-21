using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Vertex : MonoBehaviour {
	GameObject Manager;
	public double sumCost;
	public double costToEnd;
	public double realCost;
	public GameObject parentVertex;

	private int pheromones;

	// Use this for initialization
	void Start () {
		Manager = GameObject.FindWithTag ("Manager");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown(){
		if (Manager.GetComponent<Manager> ().IsBuilding) {
			if (Manager.GetComponent<Manager> ().chooseVertex1 == null) {
				Manager.GetComponent<Manager> ().chooseVertex1 = gameObject;
			} else {
				Manager.GetComponent<Manager> ().chooseVertex2 = gameObject;
				Manager.GetComponent<Manager> ().CreateEdge ();
			}
		} else if(!Manager.GetComponent<Manager> ().isCreated) {
			if (Manager.GetComponent<Manager> ().start == null) {
				Manager.GetComponent<Manager> ().start = gameObject;
				Manager.GetComponent<UIController> ().setHelpText ("Выберите конечную вершину");
			} else {
				Manager.GetComponent<Manager> ().end = gameObject;
				Manager.GetComponent<Manager> ().initEdges ();
				Manager.GetComponent<Manager> ().isCreated = true;
			}
		}

	}
}
