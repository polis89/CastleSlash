using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {
	private static string logTAG = "SpawnEnemyScript";
	private static ILogger logger = Debug.logger;

	public GameObject[] waypoints;

	public GameObject Spawn(GameObject enemy){
		//Set way waypoints in enemy Mover script
		GameObject inst = Instantiate (enemy);
		inst.GetComponent<MoveEnemy> ().waypoints = waypoints;
		return inst;
	}
}
