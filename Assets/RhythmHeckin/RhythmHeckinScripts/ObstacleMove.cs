using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObstacleMove : MonoBehaviour {

	public float speed;
	
	//This had a 90 degree turn so that it -hopefully- accentuates the beat a bit more

	public Vector3 initLocation;
	public Vector3 dropLocation;
	public Vector3 endLocation;

	public float beatsToDropLocation = 1f;
	public float beatsToEndLocation = 1f;
	
	private Vector3 destination;

	private Vector3 direction;
	
	
	
	
	void Start () {
		
		
		initLocation = transform.position;
		direction = endLocation - initLocation;
		destination = endLocation;
		float distance = direction.magnitude;
		
		direction.Normalize();
		float travelTime = beatsToEndLocation * (RhythmHeckinWwiseSync.secondsPerBeat);

		speed = distance / travelTime;
		
	
		
		Destroy(gameObject, 4.0f);
		
	}
	
	// Update is called once per frame
	void Update () {

		
		transform.Translate(direction * speed * Time.deltaTime);
		
		//change directions when we are within 0.05 units of the target
		//only do this when the obstacle is traveling down
		Vector3 checkVector = destination - transform.position;
		if (checkVector.magnitude < 0.2f && destination == dropLocation) {
			destination = endLocation;
			ChangeDirections(endLocation, beatsToEndLocation);
		}
		
	}

	void ChangeDirections(Vector3 newDestination, float beatsToNewDestination) {
		direction = newDestination - transform.position;
		speed = direction.magnitude / (beatsToNewDestination * (RhythmHeckinWwiseSync.secondsPerBeat));
		
		//normalize gives the vector a magnitude of 1
		direction.Normalize();

	}
}
