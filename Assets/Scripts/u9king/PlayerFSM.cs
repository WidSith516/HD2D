using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFSM : MonoBehaviour
{
    public enum State { WaitForRound, Round, Round_A, Round_B,Round_D,Null}

    [Header("参数")]
    public State currentState;  //当前执行状态

    [Header("时间")]
    public float PlayerDeltaTime;
    public float TypeSelectDeltaTime;

    [Header("QTE时间")]
    public float TypeSelectMaxTime = 5f;
    public float QTEMaxTime = 5f;


    [Header("属性")]
    //角色最大生命值与当前生命值
    public float MaxHealth;
    public float CurrentHealth;
    //角色最大韧性与当前韧性
    public float MaxToughness;
    public float CurrentMagic;
    //行动条
    public float actionSpeed = 2f;  //2秒

    private bool stateLock = false; //状态锁

    [Header("CanvasPanel")]
    public Image Round_Player_Progress; 
    public GameObject Type_Panel;       //ABD面板
    public GameObject QTE_Panel;       //QTE面板
    public Scrollbar QTE_Scrollbar;
    public Image Enemy_health_value;
    public Image Enemy_Toughness_value;

    [Header("敌人")]
    public GameObject Enemy;

    [Header("自动获取数值")]
    public Transform enemyOriginTransform;
    public Vector3 playerOriginPos;
    public Vector3 enemyClosePos;

    //打断动画表
    private HashSet<State> BreakStates = new HashSet<State> {
        State.Round_A,
        State.Round_B,
        State.Round_D,
    };

    private void Awake()
    {
        enemyOriginTransform = Enemy.transform;
        enemyClosePos = Enemy.transform.position - Vector3.right * 2f;
        playerOriginPos = transform.position;
        PlayerDeltaTime = 0;
    }

    private void Start()
    {
        // 初始化状态，此处为初始化闲置状态
        currentState = State.Null;
    }

    private void Update()
    {
        CheckState();
        //注册并轮询状态，初始为闲置Idle状态
        currentState = currentState switch
        {
            State.WaitForRound => UpdateWaitForRoundState(),
            State.Round => UpdateRoundState(),
            State.Round_A => UpdateRound_AState(),
            State.Round_B => UpdateRound_BState(),
            State.Round_D => UpdateRound_DState(),
            _ => currentState
        };
    }

    //private void ChangeState(State nextState)  //切换状态函数
    //{
    //    if (currentState != nextState)
    //    {
    //        stateLock = true;
    //        currentState = nextState;
    //    }
    //}

    private State ChangeState(State nextState)  //切换状态函数
    {
        if (currentState != nextState)
        {
            stateLock = true;
            currentState = nextState;
        }
        return currentState;
    }



    private State UpdateWaitForRoundState()  //WaitForRound状态
    {
        if (stateLock)
        {
            Type_Panel.SetActive(false);
            QTE_Panel.SetActive(false);
        }
        return State.WaitForRound;
    }

    private State UpdateRoundState()  //Round状态
    {
        if (stateLock)
        {
            stateLock = false;
            Type_Panel.SetActive(true);
            Debug.Log("Round State");
            Time.timeScale = 0.1f;
        }
        //一直执行
        TypeSelectDeltaTime += Time.unscaledDeltaTime;

        //页面显示
        //Type_Panel.SetActive(true);  //打开Type面板
        if (Input.GetKeyDown(KeyCode.A))
        {
            TypeSelectDeltaTime = 0;
            return ChangeState(State.Round_A);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TypeSelectDeltaTime = 0;
            return ChangeState(State.Round_D);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            return State.Round_B;
        }
        if(TypeSelectDeltaTime > TypeSelectMaxTime)
        {
            //大于5s就选择A
            //return State.Round_A;
            return ChangeState(State.Round_A);
        }
        
        return State.Round; //维持Round状态

    }

    private State UpdateRound_AState()  //Round_A状态
    {
        TypeSelectDeltaTime += Time.unscaledDeltaTime;
        QTE_Scrollbar.value = TypeSelectDeltaTime/ QTEMaxTime;
        if (stateLock)
        {
            Time.timeScale = 0.1f;
            stateLock = false;
            QTE_Panel.SetActive(true);
            transform.position = enemyClosePos;
            Debug.Log("Round_AState State");
        }

        if(Input.GetKeyDown(KeyCode.Space) || TypeSelectDeltaTime > TypeSelectMaxTime)
        {
            Time.timeScale = 1f;
            //播放动画
            gameObject.GetComponent<Animator>().Play("cx1skill2");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Enemy_health_value.fillAmount = Enemy_health_value.fillAmount - 0.5f;  //重击半血
                Debug.Log("QTEA Success，重击Boss半血");
            }
            else if(TypeSelectDeltaTime > TypeSelectMaxTime)
            {
                Enemy_health_value.fillAmount = Enemy_health_value.fillAmount - 0.1f;  //轻击没掉血
                Debug.Log("QTEA Failed，轻击Boss掉0.1f血");
            }
            transform.position = playerOriginPos;
            TypeSelectDeltaTime = 0;
            PlayerDeltaTime = 0;
            return ChangeState(State.WaitForRound);
        }

        return State.Round_A;
    }

    private State UpdateRound_BState()  //Round_A状态
    {
        if (stateLock)
        {
            stateLock = false;
            Debug.Log("Round_DState State");
        }
        return State.Round_B;
    }

    private State UpdateRound_DState()  //Round_A状态
    {
        TypeSelectDeltaTime += Time.unscaledDeltaTime;
        QTE_Scrollbar.value = TypeSelectDeltaTime / QTEMaxTime;
        if (stateLock)
        {
            Time.timeScale = 0.1f;
            stateLock = false;
            QTE_Panel.SetActive(true);
            transform.position = enemyClosePos;
            Debug.Log("Round_DState State");
        }
        if (Input.GetKeyDown(KeyCode.Space) || TypeSelectDeltaTime > TypeSelectMaxTime)
        {
            Time.timeScale = 1f;
            //播放动画
            gameObject.GetComponent<Animator>().Play("cx1skill2");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.5f;  //重击半韧
                Debug.Log("QTEA Success，重击Boss半韧");
            }
            else if (TypeSelectDeltaTime > TypeSelectMaxTime)
            {
                Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.1f;  //轻击0.1f韧
                Debug.Log("QTEA Failed，轻击Boss掉0.1f韧性");
            }
            transform.position = playerOriginPos;
            TypeSelectDeltaTime = 0;
            PlayerDeltaTime = 0;
            return ChangeState(State.WaitForRound);
        }


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Time.timeScale = 1f;
        //    //播放动画
        //    gameObject.GetComponent<Animator>().Play("cx1skill2");
        //    Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.5f;  //重击半血
        //    Debug.Log("QTEA Success，重击Boss韧性");
        //    transform.position = playerOriginPos;
        //    TypeSelectDeltaTime = 0;
        //    PlayerDeltaTime = 0;
        //    return ChangeState(State.WaitForRound);
        //}
        //if (TypeSelectDeltaTime > TypeSelectMaxTime)
        //{
        //    Time.timeScale = 1f;
        //    //播放动画
        //    gameObject.GetComponent<Animator>().Play("cx1skill2");
        //    Debug.Log("QTEA Failed，轻击Boss掉0.1f韧性");
        //    Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.1f;  //轻击没掉血
        //    transform.position = playerOriginPos;
        //    TypeSelectDeltaTime = 0;
        //    PlayerDeltaTime = 0;
        //    return ChangeState(State.WaitForRound);
        //}
        return State.Round_D;
    }

    private void CheckState()
    {
        PlayerDeltaTime += Time.deltaTime;
        Round_Player_Progress.fillAmount = PlayerDeltaTime / actionSpeed;
        if (BreakStates.Contains(currentState))
            return;

        if (PlayerDeltaTime < actionSpeed)
        {
            ChangeState(State.WaitForRound);
        }
        else
        {
            ChangeState(State.Round);
        }
    }
}
