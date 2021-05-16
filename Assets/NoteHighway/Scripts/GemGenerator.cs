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
    //for now it's just the spawn time offset (in number of beats)
    //right now, this assumes that each beatEvent will have the same cue offset.
    //if you don't want this to be the case, have a seperate beatmap/input evaluator pair for these other events
    //(example - rhythm heaven has varying cue lengths)
    [Header("Cue Offset in Beats")]
    public int cueBeatOffset;

    //Make sure the OkWindow > GoodWindow > PerfectWindow!!!  Also make sure that you don't have successive notes at shorter timespans than your OkWindow
    [Header("Window Sizes in MS")]
    public int OkWindowMillis = 200;
    public int GoodWindowMillis = 100;
    public int PerfectWindowMillis = 50;

    //you can + should mix this up - I chose 3 inputs just to keep things simple
    //note that you should do a "find references" to find other areas of the code that you should change
    public FallingGemInput fallingGemR, fallingGemG, fallingGemB;


    [Header("Assign this - Gems won't work otherwise")]
    public NoteHighwayWwiseSync wwiseSync;

    //In the example scene+song, there is a cue called "EndLevel" which happens at the end of the song
    public void EndLevel()
    {
        Debug.Log("Level Ended");
    }


    //We connect these next three methods to relevant events on our Note Highway Wwise Sync
    public void GenerateRCue()
    {
        //we need to instantiate the cue, set the desired player input accordingly, and then set the window timings
        GameObject newCue = Instantiate(fallingGemR.cuePrefab, fallingGemR.cueStartLocation.transform.position, Quaternion.identity);

        FallingGem fallingGem = newCue.GetComponent<FallingGem>();

        fallingGem.playerInput = fallingGemR.playerInput;

        SetGemTimings(fallingGem);
    }

    public void GenerateGCue()
    {
        GameObject newCue = Instantiate(fallingGemG.cuePrefab, fallingGemG.cueStartLocation.transform.position, Quaternion.identity);

        FallingGem fallingGem = newCue.GetComponent<FallingGem>();

        fallingGem.playerInput = fallingGemG.playerInput;

        SetGemTimings(fallingGem);
    }

    

    public void GenerateBCue()
    {
        GameObject newCue = Instantiate(fallingGemB.cuePrefab, fallingGemB.cueStartLocation.transform.position, Quaternion.identity);

        FallingGem fallingGem = newCue.GetComponent<FallingGem>();

        fallingGem.playerInput = fallingGemB.playerInput;

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
