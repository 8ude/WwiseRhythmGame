using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

//make sure you're using the Koreo namespace
//using SonicBloom.Koreo;
using UnityEngine.UI;


public class JumpingScript : MonoBehaviour {

	//We're using this enum to provide more detailed scoring and feedback to the player
	//the payload of the Koreographer Event will determine which eval phase we are in
	public enum CommandEval {
		Missed = 0,
		Good = 1,
		Perfect = 2,
		Early = 3
	};

	public CommandEval currentEval, currentWindow;

	[Header("Window Lengths in MS, make sure they're even numbers")]
	public int perfectWindowLength = 50;
	public int goodWindowLength = 100;
	//missed Time is at the end of our window;
	int missedTime;
	

	[Header("Call - Response time in Beats")]
	public int cueOffset = 2;

	public AK.Wwise.Event jumpEvent, fallEvent;


	public Text EvalText, ScoreText;

	//these will be set on every cue
	//init at zero
	int perfectWindowStart = 0;
	int perfectWindowEnd, goodWindowStart, goodWindowEnd;

	private Animator _animator;

	//an extra bool because the animator doesn't like when i check "isJumping";
	private bool _BCanJump;
	
	//added an extra bool; things are a bit more complicated now that we have multiple
	//stages to our event window
	private bool _BIsJumping;

	//did we score for this obstacle?
	public bool _didScore;
	
	private int currentScore;

	

	
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		
		//we still need this boolean so that we don't double-jump, or jump
		//during a fall
		_BCanJump = false;
		//we are going to be in the "missed" stage until 
		//the koreography event tells us otherwise
		currentEval = CommandEval.Missed;
		
		//make sure feedback texts are disabled:
		EvalText.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//always get this bool from the animator, so we don't double jump
		_BIsJumping = _animator.GetBool("isJumping");
		
		ScoreText.text = "Score: " + currentScore.ToString();

		//update our state based on our time elapsed and last cue
		//first check to see if we've created a window
		if (perfectWindowStart > 0)
		{
			CheckWindow();
		}
		
		if (Input.GetKeyDown(KeyCode.B))
		{

			Debug.Log("Input time: " + RhythmHeckinWwiseSync.GetMusicTimeInMS());
			//check to see if where we are in the window
			if (_BCanJump && !_BIsJumping) {
				switch (currentEval) {
					case CommandEval.Missed:
						BFall();
						_didScore = true;
						break;
					case CommandEval.Good:
						BJump();
						RecordGood();
						_didScore = true;
						break;
					case CommandEval.Perfect:
						BJump();
						RecordPerfect();
						_didScore = true;
						break;	
				}
			}
			
			//if we can't jump, then fall :(
			else {
				BFall();
			}

			//wwise refactor - switch jumpEval enum based on Wwise MS

			
			
		}
	}

	//We trigger this in our animation clip so that B returns to idle after the jump/fall is complete
	public void ReturnToIdle() {
		_animator.SetBool("isJumping", false);
		_animator.SetBool("isFalling", false);
	}

	public void SetEvalWindow()
    {
		int nowTIme = RhythmHeckinWwiseSync.GetMusicTimeInMS();


		//convert the cue time to milliseconds
		int cueTime = nowTIme + Mathf.RoundToInt((cueOffset * RhythmHeckinWwiseSync.secondsPerBeat) * 1000);

		perfectWindowStart = cueTime - perfectWindowLength / 2;
		perfectWindowEnd = cueTime + perfectWindowLength / 2;
		goodWindowStart = cueTime - goodWindowLength / 2;
		goodWindowEnd = cueTime + goodWindowLength / 2;

		missedTime = goodWindowEnd + 1;

		_didScore = false;

		Debug.Log("good window start: " + goodWindowStart);
		Debug.Log("perfect window end: " + perfectWindowEnd);

    }

	public void CheckWindow() {

		//todo - switch enum

		int currentTime = RhythmHeckinWwiseSync.GetMusicTimeInMS();

		if (currentTime < goodWindowStart)
        {
			currentEval = CommandEval.Early;
        }

		//good window
		else if((currentTime > goodWindowStart && currentTime < perfectWindowStart) ||
			(currentTime > perfectWindowEnd && currentTime < goodWindowEnd))
		{
			//set can jump to true if B is not already jumping;
			if (!_BIsJumping)
			{
				_BCanJump = true;
			}

			currentEval = CommandEval.Good;
        }

		else if (currentTime> perfectWindowStart && currentTime < perfectWindowEnd)
        {
			currentEval = CommandEval.Perfect;
        }

		else if (currentTime > missedTime && !_didScore)
        {
			currentEval = CommandEval.Missed;
			Debug.Log("missed - falling");
			BFall();

			//flipping this bool so we don't keep falling forever
			_didScore = true;

		}
		
	}

	

	public void BFall() {
		if (!_animator.GetBool("isFalling")) {
			_animator.SetTrigger("Fall");
			_animator.SetBool("isFalling", true);

			fallEvent.Post(gameObject);


			//We've now added some feedback text
			//StopCoroutine(HideText(EvalText, 1f));
			EvalText.enabled = true;
			EvalText.text = "Missed!";
			StartCoroutine(HideText(EvalText, 1f));
		}
	}

	//perform the animations and SFX for jumping
	public void BJump() {
		_animator.SetTrigger("Jump");
		_animator.SetBool("isJumping", true);
		
		//we also need to prevent double jumping or double scoring
		_BCanJump = false;
		jumpEvent.Post(gameObject);

	}

	
	//here are a few methods for recording scores:
	//perfect gives us 10 points
	public void RecordPerfect() {
		//make sure there's not already a coroutine happening - this can cause problems
		StopCoroutine(HideText(EvalText, 1f));
		//enable the text and set it accordingly
		EvalText.enabled = true;
		EvalText.text = "Perfect!";
		//disable the text after 1 second
		StartCoroutine(HideText(EvalText, 1f));
		//increment the score
		currentScore += 10;
	}
	
	public void RecordGood() {
		StopCoroutine(HideText(EvalText, 1f));
		EvalText.enabled = true;
		EvalText.text = "Good!";
		StartCoroutine(HideText(EvalText, 1f));
		currentScore += 5;
	}

	
	

	//This hides a text element after a some time
	IEnumerator HideText(Text textToHide, float time) {
		yield return new WaitForSeconds(time);
		textToHide.enabled = false;
	}
	
	//Removed this from Koreographer
	/*
	public void BOutofJumpWindow(KoreographyEvent koreographyEvent) {
		_BCanJump = false;
	}
	*/

}
