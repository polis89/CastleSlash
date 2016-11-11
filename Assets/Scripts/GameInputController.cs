using UnityEngine;
using System.Collections;

public class GameInputController : MonoBehaviour {
	private static string logTAG = "GameInputController";
	private static ILogger logger = Debug.logger;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touches.Length != 0) {
			logger.Log (logTAG, "Touches: " + Input.touches);
		}
		if(Input.GetMouseButton(0)){
			//float x = Input.GetAxis ("Mouse X");
			//float y = Input.GetAxis ("Mouse Y");
			//logger.Log(logTAG, "Mouse X: " + x + "; Mouse Y: " + y);
		}
	}
}
