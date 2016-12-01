using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {
	private static string logTAG = "SpawnEnemyScript";
	private static ILogger logger = Debug.logger;

	public GameObject[] waypoints;

	public void Spawn(GameObject enemy){
		Instantiate (enemy).GetComponent<MoveEnemy> ().waypoints = waypoints;
	}
}
