using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class FallingGemInput
{
    [Tooltip("this must match the name in unity's input manager")]
    public string playerInput;
    public GameObject cueStartLocation;
    public GameObject cuePrefab;

}

//This script drives the level, in conjunction with NoteHighwayWwiseSync
public class GemGenerator : MonoBehaviour
{
    //the "cue" here can be a number of things
    //for now it's just the spawn time offset - the number of beats that a cue ball is travelling towards the player

    //right now, this assumes that each beatEvent will have the same cue offset. this makes sense in a "note highway" game
    //for a counter example, the game Rhythm Heaven likes to vary the amount of time between the cue and the action
    //if you want to vary the cue time, you might program some different kinds of events, when you get a certain cue from wwise.


    [Header("Cue Offset in Beats")]
    public int cueBeatOffset;

    //Make sure the OkWindow > GoodWindow > PerfectWindow!!!  Also make sure that you don't have successive notes at shorter timespans than your OkWindow
    [Header("Window Sizes in MS")]
    public int OkWindowMillis = 200;
    public int GoodWindowMillis = 100;
    public int PerfectWindowMillis = 50;

    // these classes keep the input, the spawn location, and the gem prefab together
    public FallingGemInput fallingGemQ, fallingGemW, fallingGemO, fallingGemP;


    [Header("Assign this - Gems won't work otherwise")]
    public NoteHighwayWwiseSync wwiseSync;

    //In the example scene+song, there is a cue called "EndLevel" which happens at the end of the song
    public void EndLevel()
    {
        Debug.Log("Level Ended");
    }


    //We connect these next three methods to relevant events on our Note Highway Wwise Sync
    public void GenerateQCue()
    {
        //we need to instantiate the cue, set the desired player input accordingly, and then set the window timings
        GameObject newCue = Instantiate(fallingGemQ.cuePrefab, fallingGemQ.cueStartLocation.transform.position, Quaternion.identity);

        FallingGem fallingGem = newCue.GetComponent<FallingGem>();

        fallingGem.playerInput = fallingGemQ.playerInput;

        SetGemTimings(fallingGem);
    }

    public void GenerateWCue()
    {
        GameObject newCue = Instantiate(fallingGemW.cuePrefab, fallingGemW.cueStartLocation.transform.position, Quaternion.identity);

        FallingGem fallingGem = newCue.GetComponent<FallingGem>();

        fallingGem.playerInput = fallingGemW.playerInput;

        SetGemTimings(fallingGem);
    }

    

    public void GenerateOCue()
    {
        GameObject newCue = Instantiate(fallingGemO.cuePrefab, fallingGemO.cueStartLocation.transform.position, Quaternion.identity);

        FallingGem fallingGem = newCue.GetComponent<FallingGem>();

        fallingGem.playerInput = fallingGemO.playerInput;

        SetGemTimings(fallingGem);
    }

    public void GeneratePCue()
    {
        GameObject newCue = Instantiate(fallingGemP.cuePrefab, fallingGemP.cueStartLocation.transform.position, Quaternion.identity);

        FallingGem fallingGem = newCue.GetComponent<FallingGem>();

        fallingGem.playerInput = fallingGemP.playerInput;

        SetGemTimings(fallingGem);
    }



    void SetGemTimings(FallingGem fallingGem)
    {

        fallingGem.wwiseSync = wwiseSync;

        fallingGem.crossingTime = (float)wwiseSync.SetCrossingTimeInMS(cueBeatOffset);

        //Set Window Timings - we're going to use wwise for this
        fallingGem.OkWindowStart = fallingGem.crossingTime - (0.5f * OkWindowMillis);
        fallingGem.OkWindowEnd = fallingGem.crossingTime + (0.5f * OkWindowMillis);
        fallingGem.GoodWindowStart = fallingGem.crossingTime - (0.5f * GoodWindowMillis);
        fallingGem.GoodWindowEnd = fallingGem.crossingTime + (0.5f * GoodWindowMillis);
        fallingGem.PerfectWindowStart = fallingGem.crossingTime - (0.5f * PerfectWindowMillis);
        fallingGem.PerfectWindowEnd = fallingGem.crossingTime + (0.5f * PerfectWindowMillis);
    }

}
