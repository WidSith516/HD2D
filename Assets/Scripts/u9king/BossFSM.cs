using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossFSM : MonoBehaviour
{
    public enum State { WaitForRound, Round, Round_A, Round_D, Null }

    [Header("����")]
    public State currentState;  //��ǰִ��״̬

    [Header("ʱ��")]
    public float EnemyDeltaTime;
    public float waitForAnimationTime;


    [Header("����")]
    //��ɫ�������ֵ�뵱ǰ����ֵ
    public float MaxHealth;
    public float CurrentHealth;
    //��ɫ��������뵱ǰ����
    public float MaxToughness;
    public float CurrentMagic;
    //�ж���
    public float actionSpeed = 2f;  //2��

    private bool stateLock = false; //״̬��
    private Animator animator;

    [Header("CanvasPanel")]
    public Image Round_Enemy_Progress;
    public GameObject Type_Panel;       //ABD���
    public Image Player_health_value;
    public Image Player_Toughness_value;

    [Header("���")]
    public GameObject Player;

    [Header("�Զ���ȡ��ֵ")]
    public Transform playerOriginTransform;
    public Vector3 emenyOriginPos;
    public Vector3 playerClosePos;

    //��϶�����
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
        // ��ʼ��״̬���˴�Ϊ��ʼ������״̬
        currentState = State.Null;
    }

    private void Update()
    {
        CheckState();
        //ע�Ტ��ѯ״̬����ʼΪ����Idle״̬
        currentState = currentState switch
        {
            State.WaitForRound => UpdateWaitForRoundState(),
            State.Round => UpdateRoundState(),
            State.Round_A => UpdateRound_AState(),
            State.Round_D => UpdateRound_DState(),
            _ => currentState
        };
    }

    private State ChangeState(State nextState)  //�л�״̬����
    {
        if (currentState != nextState)
        {
            stateLock = true;
            currentState = nextState;
        }
        return currentState;
    }



    private State UpdateWaitForRoundState()  //WaitForRound״̬
    {
        if (stateLock)
        {
            Type_Panel.SetActive(false);
        }
        return State.WaitForRound;
    }

    private State UpdateRoundState()  //Round״̬
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
        return State.Round; //ά��Round״̬

    }

    private State UpdateRound_AState()  //Round_A״̬
    {
        EnemyDeltaTime = 0;
        if (stateLock)
        {
            stateLock = false;
            transform.position = playerClosePos;
            Debug.Log("Round_AState State");
            //����Boss��������
            gameObject.GetComponent<Animator>().Play("LCB_fsd_skill3");
            Player_health_value.fillAmount = Player_health_value.fillAmount - 0.5f;  //�ػ���Ѫ
            Debug.Log("Boss Success���ػ�Player��Ѫ");
            //�غϽ���
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


    private State UpdateRound_DState()  //Round_A״̬
    {
        if (stateLock)
        {
            stateLock = false;
            transform.position = playerClosePos;
            Debug.Log("Round_DState State");
            //����Boss��������
            //gameObject.GetComponent<Animator>().Play("LCB_fsd_skill3");
            Player_Toughness_value.fillAmount = Player_Toughness_value.fillAmount - 0.5f;  //�ػ�����
            Debug.Log("Boss Success���ػ�Player����");
            //�غϽ���
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
