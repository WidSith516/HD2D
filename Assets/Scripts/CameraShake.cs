using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 srcPos;
    float vol;

    // Start is called before the first frame update
    void Start()
    {
        srcPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera(float inVol, float duration)
    {
        vol = inVol;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", duration);
    }

    void DoShake()
    {
        float offsetX = Random.Range(-vol, vol);
        float offsetY = Random.Range(-vol, vol);
        transform.localPosition = srcPos + new Vector3(offsetX, offsetY, 0);
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        transform.localPosition = srcPos;
    }
}
