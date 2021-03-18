using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoSingleton<ScreenShake>
{
    private float shakeTimeRemaining;
    private float shakePower;

    public void StartShaking(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0.0f)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float x = Random.Range(-1.0f, 1.0f) * shakePower;
            float y = Random.Range(-1.0f, 1.0f) * shakePower;

            Camera.main.transform.position += new Vector3(x, y, 0.0f);
        }
    }
}
