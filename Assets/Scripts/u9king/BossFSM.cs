using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossFSM : MonoBehaviour
{
    public enum State { WaitForRound, Round, Round_A, Round_D, Null }

    [Header("参数")]
    public State currentState;  //当前执行状态

    [Header("时间")]
    public float EnemyDeltaTime;
    public float waitForAnimationTime;


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
    private Animator animator;

    [Header("CanvasPanel")]
    public Image Round_Enemy_Progress;
    public GameObject Type_Panel;       //ABD面板
    public Image Player_health_value;
    public Image Player_Toughness_value;

    [Header("玩家")]
    public GameObject Player;

    [Header("自动获取数值")]
    public Transform playerOriginTransform;
    public Vector3 emenyOriginPos;
    public Vector3 playerClosePos;

    //打断动画表
    private HashSet<State> BreakStates = new HashSet<State> {
        State.Round_A,
        State.Round_D,
    };

    private void Awake()
    {
        playerOriginTransform = Player.transform;
        playerClosePos = Player.transform.position + Vector3.right * 2f;
        emenyOriginPos = transform.position;
        EnemyDeltaTime = 0;
        animator = GetComponent<Animator>();
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
            State.Round_D => UpdateRound_DState(),
            _ => currentState
        };
    }

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
            var actionNo = 1;
            //var actionNo = Random.Range(1, 3);
            switch (actionNo)
            {
                case 1:
                    Debug.Log("Emeny Action 1 A");
                    return ChangeState(State.Round_A);
                case 2:
                    Debug.Log("Emeny Action 2 D");
                    return ChangeState(State.Round_D);
                default:
                    Debug.Log("Emeny no ActionNo");
                    break;
            }
        }
        return State.Round; //维持Round状态

    }

    private State UpdateRound_AState()  //Round_A状态
    {
        EnemyDeltaTime = 0;
        if (stateLock)
        {
            stateLock = false;
            transform.position = playerClosePos;
            Debug.Log("Round_AState State");
            //播放Boss攻击动画
            gameObject.GetComponent<Animator>().Play("LCB_fsd_skill3");
            Player_health_value.fillAmount = Player_health_value.fillAmount - 0.5f;  //重击半血
            Debug.Log("Boss Success，重击Player半血");
            //回合结束
            //EnemyDeltaTime = 0;
        }

        waitForAnimationTime += Time.deltaTime;
        if (waitForAnimationTime > animator.GetCurrentAnimatorStateInfo(0).length)
        {
            transform.position = emenyOriginPos;
            waitForAnimationTime = 0;
            return ChangeState(State.WaitForRound);
        }

        return State.Round_A;
    }


    private State UpdateRound_DState()  //Round_A状态
    {
        if (stateLock)
        {
            stateLock = false;
            transform.position = playerClosePos;
            Debug.Log("Round_DState State");
            //播放Boss攻击动画
            //gameObject.GetComponent<Animator>().Play("LCB_fsd_skill3");
            Player_Toughness_value.fillAmount = Player_Toughness_value.fillAmount - 0.5f;  //重击半韧
            Debug.Log("Boss Success，重击Player半韧");
            //回合结束
            EnemyDeltaTime = 0;
            transform.position = emenyOriginPos;
            return ChangeState(State.WaitForRound);
        }
        return State.Round_D;
    }

    private void CheckState()
    {
        EnemyDeltaTime += Time.deltaTime;
        Round_Enemy_Progress.fillAmount = EnemyDeltaTime / actionSpeed;
        if (BreakStates.Contains(currentState))
            return;

        if (EnemyDeltaTime < actionSpeed)
        {
            ChangeState(State.WaitForRound);
        }
        else
        {
            ChangeState(State.Round);
        }
    }
}
