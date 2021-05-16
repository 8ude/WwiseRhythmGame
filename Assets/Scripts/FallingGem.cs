using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGem : MonoBehaviour
{

    [Header("Timing values - to be filled when instantiated")]
    public double OkWindowStart;
    public double GoodWindowStart, PerfectWindowStart, PerfectWindowEnd, GoodWindowEnd, OkWindowEnd;
    public float crossingTime;

    public NoteHighwayWwiseSync wwiseSync;

    //public double cueTime;

    public enum CueState { Early = 0, OK = 1, Good = 2, Perfect = 3, Late = 4}
    public CueState gemCueState;

    public Vector3 destination;

    private Vector3 velocity;

    //testing consistency of crossing error
    public float crossPositionOffset;

    //debugging crossing sync issues
    private bool _gemCrossed = false;

    public string playerInput;

    

    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        destination = GameObject.FindGameObjectWithTag("NowCrossing").transform.position;

        //we want to stay in the lane, so the destination will have the same x and y coordinates as the start.
        destination.x = transform.position.x;
        destination.y = transform.position.y;
        destination.z -= crossPositionOffset;

        //velocity = distance/time -- we want to make sure that the cue crosses our destination on beat
        velocity = (destination - transform.position) / (float)(0.001f*(crossingTime - wwiseSync.GetMusicTimeInMS()));

        gemCueState = CueState.Early;
        _gemCrossed = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        UpdateWindow();

        if (wwiseSync.GetMusicTimeInMS() >= crossingTime && !_gemCrossed)
        {
            _gemCrossed = true;
        }

        //we want to make sure that the cue crosses our destination on beat, so we update the velocity every frame
        //but we also want it to keep going after it crosses the destination, so we're going to do a distance check 
        if (Vector3.Distance(destination, transform.position) > 0.5f && !_gemCrossed) 
        {
            velocity = (destination - transform.position) / (float)(0.001f * (crossingTime - wwiseSync.GetMusicTimeInMS()));
        }
  
    }

    public void UpdateWindow()
    {
        //check our cue state against the Clock Script

        //for this case (more-or-less typical japanese rhythm game style), our detection windows are 
        // early - ok - good - perfect - good - ok - late

        switch (gemCueState)
        {

            case CueState.Early:
                //check to see if we've gotten to "ok"
                if (wwiseSync.GetMusicTimeInMS() > OkWindowStart)
                {
                    gemCueState = CueState.OK;
                }
                break;
            case CueState.OK:
                //check to see if we've gotten to "good"...
                if (wwiseSync.GetMusicTimeInMS() > GoodWindowStart && wwiseSync.GetMusicTimeInMS() < PerfectWindowStart)
                {
                    gemCueState = CueState.Good;

                }
                //... or maybe we're at the end of the last "ok" window
                else if (wwiseSync.GetMusicTimeInMS() > OkWindowEnd)
                {
                    gemCueState = CueState.Late;
                }
                break;
            case CueState.Good:
                //check to see if we've gotten to "perfect"
                if (wwiseSync.GetMusicTimeInMS() > PerfectWindowStart && wwiseSync.GetMusicTimeInMS() < PerfectWindowEnd)
                {
                    gemCueState = CueState.Perfect;
                }
                //
                else if (wwiseSync.GetMusicTimeInMS() > GoodWindowEnd)
                {
                    gemCueState = CueState.OK;
                }
                break;
            case CueState.Perfect:
                if (wwiseSync.GetMusicTimeInMS() > PerfectWindowEnd)
                {
                    gemCueState = CueState.Good;
                }
                break;
            default:
                //if we're "late" there are no more potential state changes
                break;


        }
    }

    
}
