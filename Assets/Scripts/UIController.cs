using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	Button readyBtn;
	Text helpText;
	Text infoText;
	// Use this for initialization
	void Start () {
		readyBtn = GameObject.Find ("ReadyBtn").GetComponent<Button> ();
		helpText = GameObject.Find ("HelpText").GetComponent<Text> ();
		infoText = GameObject.Find ("InfoText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	public void readyButtonClick(){
		readyBtn.interactable = false;
		GameObject.FindWithTag ("Manager").GetComponent<Manager>().IsBuilding = false;
		setHelpText ("Выберите начальную вершину");
	}

	public void setHelpText(string text){
		helpText.text = text;
	}

	public void setInfoText(string text){
		infoText.text = text;
	}

	public void aStarBegin(){
		GameObject.FindWithTag ("Manager").GetComponent<AStar> ().initGraph();
		GameObject.FindWithTag ("Manager").GetComponent<AStar> ().start = true;
	}

	public void pauseBtn(){
		GameObject.FindWithTag ("Manager").GetComponent<AStar> ().paused = !GameObject.FindWithTag ("Manager").GetComponent<AStar> ().paused;
	}

	public void addAntBtn(){
		GameObject.FindWithTag ("Manager").GetComponent<Manager> ().resetColors ();
		GameObject.FindWithTag ("Manager").GetComponent<Manager> ().newColony ();
	}
}
