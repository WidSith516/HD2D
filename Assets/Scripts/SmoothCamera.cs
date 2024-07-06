using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    static Vector3 cameraSourcePos;

    static float smoothTime = 0.3f;

    static bool bInSmooth;

    static float startTime;

    static Vector3 delta = new Vector3(150, 0, 300);

    static int dir = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!bInSmooth)
        {
            return;
        }

        float alpha = (Time.time - startTime) / smoothTime;
        if (alpha > 1)
        {
            Camera.main.transform.position = cameraSourcePos + delta * dir;
            bInSmooth = false;
        }
        else
        {
            Vector3 nextPos = delta * alpha * dir;
            Camera.main.transform.position = cameraSourcePos + nextPos;
        }
    }

    static public void DoSmooth(bool bReverse = false)
    {
        bInSmooth = true;
        startTime = Time.time;
        cameraSourcePos = Camera.main.transform.position;

        if (bReverse)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
    }

    static public void RestCamera()
    {
        Camera.main.transform.Translate(cameraSourcePos);
        bInSmooth = false;
    }
}
