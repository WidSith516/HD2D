using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public delegate void OnQteFinished(QteResult qteResult);


public enum QteResult
{
    Failed,
    Success,
    Perfect,
};

public class QTEController : MonoBehaviour
{
    float QTE_TARGET = 0.5f;
    Scrollbar sbQte;
    bool bInQte;
    float qteStartTime;
    float qteTotalTime;
    float qteSpeed;
    int qteDirect;
    float qteTolerance;
    OnQteFinished onQteFinishDelegate;

    // Start is called before the first frame update
    void Start()
    {
        // test function
        // StartQteTask(5, 0.5f, 0.05f, OnQteFinishedCB);
    }

    public void Init()
    {
        // 初始化QTE条
        sbQte = gameObject.GetComponentInChildren<Scrollbar>();
        resetQte();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bInQte)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // 按下空格并抬起,判断是否qte成功
            if (QTE_TARGET - qteTolerance < sbQte.value && sbQte.value < QTE_TARGET + qteTolerance)
            {
                Debug.Log("Qte 成功");
                StopQteTask(QteResult.Success);
                return;
            }
            else
            {
                Debug.Log("Qte 失败");
                StopQteTask(QteResult.Failed);
                return;
            }
        }

        float timePast = Time.time * 1000 - qteStartTime;
        if (timePast > qteTotalTime)
        {
            Debug.Log("Qte 超时");
            StopQteTask(QteResult.Failed);
            return;
        }

        float nextVal = (Time.deltaTime * qteSpeed * qteDirect) + sbQte.value;
        if (qteDirect == 1 && nextVal > 1)
        {
            qteDirect = -1;
        }
        
        if (qteDirect == -1 && nextVal < 0)
        {
            qteDirect = 1;
        }

        sbQte.value += Time.deltaTime * qteSpeed * qteDirect;
    }

    void resetQte()
    {
        // 显示HUD
        UIController.ShowHUD();

        // 不可交互
        sbQte.interactable = false;

        // 不可见
        sbQte.gameObject.SetActive(false);

        // 状态重置
        bInQte = false;

        // 变量重置
        qteStartTime = 0.0f;
        qteTotalTime = 0.0f;
        qteSpeed = 0.0f;
        qteDirect = 1;
        qteTolerance = 0.0f;
        
        // 代理清空
        onQteFinishDelegate = null;
    }

    public void StartQteTask(float totalTime, float speed, float tolerance, OnQteFinished onQteFinish)
    {
        if (bInQte)
        {
            Debug.Log("sbQte task 禁止重入");
            return;
        }

        // 隐藏HUD
        UIController.HideHUD();

        SmoothCamera.DoSmooth();

        qteTolerance = tolerance;
        qteStartTime = Time.time * 1000;
        qteTotalTime = totalTime * 1000;
        qteSpeed = speed;
        onQteFinishDelegate = onQteFinish;
        
        
        sbQte.gameObject.SetActive(true);
        sbQte.value = 0;
        bInQte = true;
    }

    public void StopQteTask(QteResult qteResult)
    {
        SmoothCamera.DoSmooth(true);

        if (onQteFinishDelegate != null)
        {
            onQteFinishDelegate(qteResult);
        }
        
        resetQte();
    }
}
