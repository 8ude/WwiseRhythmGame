using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using SonicBloom.Koreo;

public class ObstacleScript : MonoBehaviour {

	public Transform spawnLocation;
	
	//to do more mickey-mousing, we want our obstacle to get to the drop location in one beat, then progress to the jump location
	public Transform dropLocation;
	public Transform jumpLocation;
	public GameObject obstaclePrefab;
	public Text JumpText;
	
	
	void Start () {
		

		JumpText.enabled = false;

	}
	
	public void SpawnObstacle()
    {
		GameObject newSpawn = Instantiate(obstaclePrefab, spawnLocation);
		//caching the Obstacle Move Script to set some values;
		ObstacleMove obsMove = newSpawn.GetComponent<ObstacleMove>();
		//set drop location and end location accordingly;
		obsMove.dropLocation = dropLocation.position;
		obsMove.endLocation = jumpLocation.position;

	}

	public void JumpTextCue()
    {
		StopCoroutine(HideText(JumpText, 1f));
		JumpText.enabled = true;
		StartCoroutine(HideText(JumpText, 1f));

	}
	
	
	IEnumerator HideText(Text textToHide, float time) {
		yield return new WaitForSeconds(time);
		textToHide.enabled = false;
	}
}
