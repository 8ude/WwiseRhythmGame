using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SonicBloom.Koreo;
using UnityEngine.UI;

public class TextBounce : MonoBehaviour {

	public float textScaleRange = 5f;

	private int startSize;
	private Text text;

	private float OutputValue;


	// Use this for initialization
	void Start () {
		//Koreographer.Instance.RegisterForEvents("BKickEvent", ChangeScale);

		text = GetComponent<Text>();

		startSize = text.fontSize;

	}
	
	// Update is called once per frame
	void Update () {

	}

	//todo wwise - change to some kind of dotween or coroutine

	//void ChangeScale(KoreographyEvent koreographyEvent) {
		//float newScale = koreographyEvent.GetValueOfCurveAtTime(Koreographer.Instance.GetMusicSampleTime());
		//Debug.Log("curve value: " + newScale);

		//text.fontSize = Mathf.RoundToInt(newScale * textScaleRange + startSize);
	//}

	
}
