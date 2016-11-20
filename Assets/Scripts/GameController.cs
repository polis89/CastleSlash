using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private static string logTAG = "GameControllerScript";
	private static ILogger logger = Debug.logger;
	private PlayerMoveScript playerMoveScript;
	private bool isMoving; //Screen was touched and not untouched
	private Vector3 startMoveTouchCoord;
	private Direction[] directionArray;
	private int direction_array_index; //Count of elements in direction Array, and also index for nex Insertion

	public GameObject playerObject;
	public Camera gameCamera;
	public float moveInputSensivity;
	public float errorAngle;

	// Use this for initialization
	void Start () {
		playerMoveScript = playerObject.GetComponent<PlayerMoveScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			CheckMove ();
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
		directionArray = new Direction[8];
		direction_array_index = 0;
		startMoveTouchCoord = gameCamera.ScreenToWorldPoint(Input.mousePosition);
	}

	void EndMove(){
		isMoving = false;
		MakeAction ();
		directionArray = null;
	}

	void CheckMove (){
		// Return if Array of directions full
		if (direction_array_index > directionArray.Length - 1)
			return;
		Vector3 mousePositionVector = gameCamera.ScreenToWorldPoint (Input.mousePosition);
		float x = mousePositionVector.x;
		float y = mousePositionVector.y;
		if (Mathf.Abs(y -  startMoveTouchCoord.y) > moveInputSensivity || Mathf.Abs(x -  startMoveTouchCoord.x) > moveInputSensivity) {
			Direction direction = CheckDirection (startMoveTouchCoord, mousePositionVector);
			// If first movement or diferrent to previous movement
			if(direction_array_index == 0 || !direction.Equals(directionArray[direction_array_index - 1])){
				directionArray [direction_array_index] = direction;
				direction_array_index++;
			}
			startMoveTouchCoord = mousePositionVector;
		}
	}

	Direction CheckDirection(Vector3 startPosition, Vector3 endPosition){
		Vector3 changeVector = endPosition - startPosition;
		float x = changeVector.x;
		float y = changeVector.y;
		//logger.Log (logTAG, "x: " + x + "; y: " + y);
		if (Vector3.Angle (changeVector, Vector3.right) < errorAngle) {
			return Direction.right;
		}
		if (Vector3.Angle (changeVector, Vector3.down) < errorAngle) {
			return Direction.down;
		}
		if (Vector3.Angle (changeVector, Vector3.left) < errorAngle) {
			return Direction.left;
		}
		if (Vector3.Angle (changeVector, Vector3.up) < errorAngle) {
			return Direction.up;
		}
		if (x > 0 && y > 0) {
			return Direction.up_right;
		}
		if (x > 0 && y < 0) {
			return Direction.down_right;
		}
		if (x < 0 && y > 0) {
			return Direction.up_left;
		}
		if (x < 0 && y < 0) {
			return Direction.down_left;
		}
		return Direction.none;
	}

	void MakeAction (){
		printArray ();
		if (direction_array_index == 2) {
			if (directionArray[0].Equals(Direction.up_right) && directionArray[1].Equals(Direction.down_right)){
				// ^
				logger.Log(logTAG, "UP");
				return;
			}
			if (directionArray[0].Equals(Direction.down_right) && directionArray[1].Equals(Direction.up_right)){
				// v
				logger.Log(logTAG, "DOWN");
				return;
			}
		}
	}
	void printArray(){
		foreach (Direction dir in directionArray) {
			logger.Log (logTAG, dir);
		}
	}
}

public enum Direction{
	none,
	down,
	left,
	right,
	up,
	up_right,
	up_left,
	down_right,
	down_left
}
