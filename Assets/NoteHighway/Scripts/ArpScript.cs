using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArpScript : MonoBehaviour
{
    public GameObject musicObject;

    public AK.Wwise.RTPC arpVolume;

    //meter volume coming from Wwise
    public AK.Wwise.RTPC arpMeter;

    public float minScale, maxScale;

    Material material;


    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            arpVolume.SetValue(musicObject, 100f);
        }
        else
        {
            arpVolume.SetValue(musicObject, 0f);
        }

        float remappedScale = Util.remap(arpMeter.GetValue(musicObject), -48f, 0f, minScale, maxScale);

        transform.localScale = new Vector3(transform.localScale.x, remappedScale, transform.localScale.z);

        float remappedColor = Util.remap(arpMeter.GetValue(musicObject), -48f, 0f, 0f, 1f);

        material.color = new Color(0f, remappedColor * remappedColor, remappedColor);

    }
}
