using UnityEngine;
using System.Collections;

public class PlayerMoveScript : MonoBehaviour {
	public Camera camera;
	public GameObject player;
	public GameObject playerModel;
	public Vector3 leftPosition;
	public Vector3 middlePosition;
	public Vector3 rightPosition;

	private static string logTAG = "PlayerMoveScript";
	private static ILogger logger = Debug.logger;
	private bool isMoving;
	private Vector3 startMoveTouchCoord;
	private Vector3 startMovePlayerCoord;

	void Start (){
		startMovePlayerCoord = middlePosition;
		logger.Log (logTAG, "startMovePlayerCoord" + startMovePlayerCoord);
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
			float x_delta = mousePosition.x - startMoveTouchCoord.x;
			float playerWantedPosition = startMovePlayerCoord.x + x_delta;
			if (playerWantedPosition >= leftPosition.x &&
			   playerWantedPosition <= rightPosition.x) {
				player.transform.position = new Vector3(playerWantedPosition, player.transform.position.y, player.transform.position.z);
			}
		}
	}

	void OnMouseDown(){
		StartMove ();
	}

	void OnMouseUp(){
		EndMove ();
	}

	void StartMove(){
		isMoving = true;
		startMoveTouchCoord = camera.ScreenToWorldPoint(Input.mousePosition);
		startMovePlayerCoord = player.transform.position;
		logger.Log (logTAG, "StartMove " + startMoveTouchCoord.x);	
	}

	void EndMove(){
		logger.Log (logTAG, "EndMove");		
		isMoving = false;
	}
}
