using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_manager : MonoBehaviour
{
    public RectTransform rect;
    public float duration = 1f;
    public AnimationCurve animCurve;

    private void OnEnable()
    {
        StartCoroutine(ScaleCorotine(Vector3.zero, Vector3.one));
    }

    private IEnumerator ScaleCorotine(Vector3 _Start, Vector3 _End)
    {
        float time = 0;
        while (time <=duration)
        {
            time += Time.deltaTime;
            rect.localScale= Vector3.Lerp(_Start, _End, animCurve.Evaluate(time/duration));
            yield return null;
        }
    }
}
