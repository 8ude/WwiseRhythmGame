using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
	public double[] spawnTimeArray;

	private int spawnIndex = 0;

	
	private double musicStartTime;
	private double nextSpawnTime;
	// Use this for initialization
	void Start () {
		
		//find our music start time
		musicStartTime = AudioSettings.dspTime + 0.25d;
		GetComponent<AudioSource>().PlayScheduled(musicStartTime);

		for (int i = 0; i < spawnTimeArray.Length; i++) {
			spawnTimeArray[i] += musicStartTime;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (AudioSettings.dspTime >= nextSpawnTime) {
			SpawnBall();
			spawnIndex++;
			nextSpawnTime = spawnTimeArray[spawnIndex];

		}

	}

	void SpawnBall() {
		
	}
	
	
	
}
