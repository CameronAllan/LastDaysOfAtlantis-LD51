using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    public RectTransform clockHolder;


    public void UpdateClock(float elapsed, float max)
    {
        float rot = -((elapsed / max) * 360);
        clockHolder.localEulerAngles = new Vector3(0, 0, rot);
    }
}
