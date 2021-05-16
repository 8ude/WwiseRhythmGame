using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmHeckinWwiseSync : MonoBehaviour
{

    public AK.Wwise.Event rhythmHeckinEvent;

    [HideInInspector] public static float secondsPerBeat;
    [HideInInspector] public static float BPM;

    public UnityEvent OnB;
    public UnityEvent OnJump;

    public UnityEvent OnLevelEnded;

    public UnityEvent OnEveryGrid;
    public UnityEvent OnEveryBeat;
    public UnityEvent OnEveryBar;

    

    //id of the wwise event - using this to get the playback position
    static uint playingID;

    void Start()
    {

        playingID = rhythmHeckinEvent.Post(gameObject, (uint)(AkCallbackType.AK_MusicSyncAll | AkCallbackType.AK_EnableGetMusicPlayPosition), MusicCallbackFunction);

    }

    void MusicCallbackFunction(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {

        AkMusicSyncCallbackInfo _musicInfo = (AkMusicSyncCallbackInfo)in_info;

        switch (_musicInfo.musicSyncType)
        {
            case AkCallbackType.AK_MusicSyncUserCue:

                CustomCues(_musicInfo.userCueName, _musicInfo);

                secondsPerBeat = _musicInfo.segmentInfo_fBeatDuration;
                BPM = _musicInfo.segmentInfo_fBeatDuration * 60f;

                break;
            case AkCallbackType.AK_MusicSyncBeat:


                OnEveryBeat.Invoke();
                break;
            case AkCallbackType.AK_MusicSyncBar:
                //I want to make sure that the secondsPerBeat is defined on our first measure.
                secondsPerBeat = _musicInfo.segmentInfo_fBeatDuration;
                //Debug.Log("Seconds Per Beat: " + secondsPerBeat);

                OnEveryBar.Invoke();
                break;

            case AkCallbackType.AK_MusicSyncGrid:
                OnEveryGrid.Invoke();
                break;
            default:
                break;

        }

    }




    public void CustomCues(string cueName, AkMusicSyncCallbackInfo _musicInfo)
    {
        switch (cueName)
        {
            case "B":
                OnB.Invoke();
                break;
            case "Jump":
                OnJump.Invoke();
                break;
            case "LevelEnded":
                OnLevelEnded.Invoke();
                break;
            default:
                break;

        }
    }

    //this is pretty straightforward - get the elapsed time
    public static int GetMusicTimeInMS()
    {

        AkSegmentInfo segmentInfo = new AkSegmentInfo();

        AkSoundEngine.GetPlayingSegmentInfo(playingID, segmentInfo, true);

        return segmentInfo.iCurrentPosition;
    }

    //We're going to call this when we spawn a gem, in order to determine when it's crossing time should be
    //based on the current playback position, our beat duration, and our beat offset
    public int SetCrossingTimeInMS(int beatOffset)
    {
        AkSegmentInfo segmentInfo = new AkSegmentInfo();

        AkSoundEngine.GetPlayingSegmentInfo(playingID, segmentInfo, true);

        int offsetTime = Mathf.RoundToInt(1000 * secondsPerBeat * beatOffset);

        Debug.Log("setting time: " + segmentInfo.iCurrentPosition + offsetTime);

        return segmentInfo.iCurrentPosition + offsetTime;
    }


}
