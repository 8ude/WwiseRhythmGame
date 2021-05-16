using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PunchScale : MonoBehaviour
{
    public float punchScaleMagnitude = 2f;
    //simple punch scale effect from DoTween
    public void PunchScaleEffect()
    {
        transform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, punchScaleMagnitude), 0.25f, 10, 0f);
    }
}
