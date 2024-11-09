using System.Collections;
using System;
using UnityEngine;

public class TickMachine : MonoBehaviour
{
    public static Action OnTick;

    private readonly WaitForSecondsRealtime _waitFiveSecond = new(5f);

    private void OnEnable() => StartCoroutine(TickCoroutine());

    private void OnDisable() => StopCoroutine(TickCoroutine());

    IEnumerator TickCoroutine()
    {
        while (true)
        {
            yield return _waitFiveSecond;
            OnTick?.Invoke();
        }
    }
}
