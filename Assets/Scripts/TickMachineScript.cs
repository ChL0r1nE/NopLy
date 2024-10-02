using System.Collections;
using System;
using UnityEngine;

public class TickMachineScript : MonoBehaviour
{
    public static Action OnTick;

    private WaitForSecondsRealtime _waitFiveSecond = new WaitForSecondsRealtime(5);

    private void OnEnable() => StartCoroutine(TickCoroutine());

    IEnumerator TickCoroutine()
    {
        while (true)
        {
            yield return _waitFiveSecond;
            OnTick?.Invoke();
        }
    }
}
