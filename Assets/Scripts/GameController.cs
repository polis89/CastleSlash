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

	public GameObject leftRoad;
	public GameObject centerRoad;
	public GameObject rightRoad;

	public float moveInputSensivity;
	public float errorAngle;

	public GameObject playerObject;
	public Camera gameCamera;
	private ArrayList leftRoadList = new ArrayList ();
	private ArrayList centerRoadList = new ArrayList ();
	private ArrayList rightRoadList = new ArrayList ();

	public GameObject enemy; //TEST??

	// Use this for initialization
	void Start () {
		playerMoveScript = playerObject.GetComponent<PlayerMoveScript> ();
		StartCoroutine (SpawnCoroutine());
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
		//printArray ();
		if (direction_array_index == 2) {
			if (directionArray[0].Equals(Direction.up_right) && directionArray[1].Equals(Direction.down_right) ||
				directionArray[0].Equals(Direction.up_left) && directionArray[1].Equals(Direction.down_left)){
				// ^
				FireCannon();
				logger.Log(logTAG, "UP");
				return;
			}
			if (directionArray[0].Equals(Direction.down_right) && directionArray[1].Equals(Direction.up_right) ||
				directionArray[0].Equals(Direction.down_left) && directionArray[1].Equals(Direction.up_left)){
				// v
				logger.Log(logTAG, "DOWN");
				return;
			}
		}
	}

	void FireCannon(){
		switch (playerObject.GetComponent<PlayerMoveScript> ().GetPosition ()) {
		case FirePosition.left:
			if (leftRoadList.Count < 1) {
				return;
			}
			GameObject target = leftRoadList [0] as GameObject;
			if (target != null) {
				Destroy (target);
				leftRoadList.Remove (target);
			}
			break;
		case FirePosition.center:
			break;
		case FirePosition.right:
			break;
		}
	}

	void printArray(){
		string array = "";
		foreach (Direction dir in directionArray) {
			array += dir + ":";
		}
		logger.Log(logTAG, array);
	}

	IEnumerator SpawnCoroutine(){
		yield return new WaitForSeconds (2f);
		for (int i = 0; i < 4; i++) {
			GameObject newEnemy = leftRoad.GetComponent<SpawnEnemy> ().Spawn (enemy);
			leftRoadList.Add (newEnemy);
			yield return new WaitForSeconds (2f);
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
