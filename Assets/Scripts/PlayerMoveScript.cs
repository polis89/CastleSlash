using UnityEngine;
using System.Collections;

public class PlayerMoveScript : MonoBehaviour {
	private static string logTAG = "PlayerMoveScript";
	private static ILogger logger = Debug.logger;

	private FirePosition position;

	public Camera gameCamera;
	public float rotationLeft;
	private float rotationMiddle = 0;
	public float rotationRight;

	public float touchDiapazon;

	private bool isMoving;
	private Vector3 startMoveTouchCoord;

	//Grenzen for player move, set by every touchDown
	private float grenzeLeft;
	private float grenzeRight;

	void Start (){
		SetPosition (FirePosition.center);
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			Vector3 mousePosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
			float x_delta = mousePosition.x - startMoveTouchCoord.x;
			float rotationZ = 0;
			switch (position) {
			case FirePosition.center:
				rotationZ = rotationRight * (x_delta / touchDiapazon);
				break;
			case FirePosition.left:
				rotationZ = rotationLeft + rotationRight * (x_delta / touchDiapazon);
				break;
			case FirePosition.right:
				rotationZ = 360 - rotationLeft + rotationRight * (x_delta / touchDiapazon);
				break;
			}
			if (mousePosition.x >= grenzeLeft &&
					mousePosition.x <= grenzeRight) {
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.rotation.eulerAngles.x, 0, rotationZ);
			} else if (mousePosition.x < grenzeRight) {
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.rotation.eulerAngles.x, 0, rotationLeft);
			} else {
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.rotation.eulerAngles.x, 0, rotationRight);
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
		startMoveTouchCoord = gameCamera.ScreenToWorldPoint(Input.mousePosition);
		//Set move boundaries
		switch (position) {
		case FirePosition.center:
			grenzeLeft = startMoveTouchCoord.x - touchDiapazon;
			grenzeRight = startMoveTouchCoord.x + touchDiapazon;
			break;
		case FirePosition.left:
			grenzeLeft = startMoveTouchCoord.x;
			grenzeRight = startMoveTouchCoord.x + touchDiapazon * 2;
			break;
		case FirePosition.right:
			grenzeLeft = startMoveTouchCoord.x - touchDiapazon * 2;
			grenzeRight = startMoveTouchCoord.x;
			break;
		}
		isMoving = true;
	}

	void EndMove(){
		isMoving = false;
		float angleZ = gameObject.transform.rotation.eulerAngles.z;
		float grenzeStep = (Mathf.Abs (rotationLeft) + Mathf.Abs (rotationRight)) / 3;
		if (angleZ > rotationLeft - grenzeStep && angleZ <= rotationLeft + 1) { //+1 for Fault
			SetPosition (FirePosition.left);
		} else if (angleZ >= 360 - grenzeStep || angleZ <= rotationLeft - grenzeStep) {
			SetPosition (FirePosition.center);
		} else {
			SetPosition (FirePosition.right);
		}
	}

	void SetPosition(FirePosition positionNew){
		//logger.Log (logTAG, "Position: " + positionNew);		
		position = positionNew;
		float rotationZ = 0;
		switch (positionNew) {
		case FirePosition.left:
			rotationZ = rotationLeft;
			break;
		case FirePosition.right:
			rotationZ = rotationRight;
			break;
		}
		Vector3 rotation = new Vector3 (gameObject.transform.rotation.eulerAngles.x, 0, rotationZ);
		gameObject.transform.eulerAngles = rotation;
	}

	public FirePosition GetPosition(){
		return position;
	}
}

public enum FirePosition{
	center,
	right,
	left
}
