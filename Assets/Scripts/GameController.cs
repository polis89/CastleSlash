using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private static string logTAG = "GameControllerScript";
	private static ILogger logger = Debug.logger;
	private PlayerMoveScript playerMoveScript;

	public GameObject playerObject;

	// Use this for initialization
	void Start () {
		playerMoveScript = playerObject.GetComponent<PlayerMoveScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		StartMove ();
	}

	void OnMouseUp(){
		EndMove ();
	}

	void StartMove(){
	}

	void EndMove(){
		logger.Log (logTAG, "Player position: " + playerMoveScript.GetPosition());	
	}
}
