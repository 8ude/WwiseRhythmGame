using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script synchronizes looping animations to the Clock
/// At the moment, it's only set up to synchronize single animations
/// 
/// Credit to Graham Tattersall for the base code --- https://www.gamasutra.com/blogs/GrahamTattersall/20190515/342454/Coding_to_the_Beat__Under_the_Hood_of_a_Rhythm_Game_in_Unity.php
/// 
/// </summary>
public class AnimationSync : MonoBehaviour
{

    
    //public variables
    [Header("animation length in beats")]
    public int numBeatsInLoop;

    //private variables
    [Header("numbers for visualization purposes - do not edit")]
    [SerializeField] float beatLoopPosition;

    //number of loops completed
    [SerializeField] int numLoops;

    //our loop position on a 0-1 scale
    [SerializeField] float loopPositionNormalized;

    //you should connect this Wwise -- either through a reference or a static variable
    //for instance, on update : TimeMS = RhythmHeckinWwiseSync.GetTimeInMS();
    public int TimeMS;
    public float secondsPerBeat;

    //animator variables
    Animator animator;
    AnimatorStateInfo animatorStateInfo;
    int currentState;

    

    private void Awake()
    {
        numLoops = 0;
    }

    void Start()
    {
        //Load the animator attached to this object
        animator = GetComponent<Animator>();

        //Get the info about the current animator state
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Convert the current state name to an integer hash for identification
        currentState = animatorStateInfo.fullPathHash;
    }

    void Update()
    {
        //make sure our clock has started (Clock has a bit of a start delay)
        if (TimeMS >= 0d)
        {
            //Start playing the current animation from wherever the current beat loop is
            animator.Play(currentState, -1, GetNormalizedLoopPosition(secondsPerBeat));
            //Set the speed to 0 so it will only change frames when you next update it
            animator.speed = 0;
        }
    }

    public float GetNormalizedLoopPosition(double timeMS)
    {
        //our Clock script doesn't intrinsically have the time in beats, so we need to do a bit of conversion
        //note that this assumes that timeMS starts at the same time as our song!  make sure your song file has no silence at the beginning.

        float timeInBeats = (float)timeMS * 0.001f / secondsPerBeat;

        //update our loop if we get to the end of it
        if (timeInBeats > (numLoops + 1) * numBeatsInLoop)
        {
            numLoops++;
        }

        //update our loop position (in number of beats
        beatLoopPosition = timeInBeats - (numLoops * numBeatsInLoop);

        //then normalize beatLoopPosition on a 0-1 scale
        loopPositionNormalized = beatLoopPosition / numBeatsInLoop;

        return loopPositionNormalized;

    }
}
