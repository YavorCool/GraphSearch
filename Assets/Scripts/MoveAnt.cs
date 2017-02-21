using UnityEngine;
using System.Collections;

public class MoveAnt : MonoBehaviour {

	private Vector3 currentPoint;
	private Vector3 nextPoint;
	private float lastPointSwitchTime;
	public float speed = 1.0f;
	private bool pherUpdated = false;
	public bool last;
	public Vector3 moveTo{
		set{ 
			nextPoint = value;
		}
	}



	// Use this for initialization
	void Start () {
		lastPointSwitchTime = Time.time;
		currentPoint = gameObject.transform.position;
		nextPoint = currentPoint;
	}
	
	// Update is called once per frame
	void Update () {
		// 1 
		Vector3 startPosition = currentPoint;
		Vector3 endPosition = nextPoint;
		// 2 
		float pathLength = Vector3.Distance (startPosition, endPosition);
		float totalTimeForPath = pathLength / speed;
		float currentTimeOnPath = Time.time - lastPointSwitchTime;
		gameObject.transform.position = Vector3.Lerp (startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
		// 3 
		if (gameObject.transform.position.Equals(endPosition)) {
			lastPointSwitchTime = Time.time;
			currentPoint = gameObject.transform.position;
			if (!currentPoint.Equals (gameObject.GetComponent<ChooseVertex> ().endVertex.transform.position)) {
				gameObject.GetComponent<ChooseVertex> ().chooseNextVertex ();
			} else {
				if (!pherUpdated && last) {
					GameObject.FindWithTag ("Manager").GetComponent<Manager> ().updateEdges ();
					pherUpdated = true;
				}
			}

		}
	}
}
