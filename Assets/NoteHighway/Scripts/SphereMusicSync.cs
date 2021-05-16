using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMusicSync : MonoBehaviour
{
    public GameObject musicObject;

    public AK.Wwise.RTPC musicMeter;

    public float minScale;
    public float maxScale;

    float previousScale;


    // Update is called once per frame
    void Update()
    {
        float momentaryVolume = musicMeter.GetValue(musicObject);

        float remappedRange = Util.remap(momentaryVolume, -24, 0, minScale, maxScale);

        float smoothScale = Mathf.Lerp(previousScale, remappedRange, 0.05f);

        Vector3 newScale = new Vector3(smoothScale, smoothScale, remappedRange);

        transform.localScale = newScale;

        previousScale = transform.localScale.x;


    }
}
