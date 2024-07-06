using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFSM : MonoBehaviour
{
    public enum State { WaitForRound, Round, Round_A, Round_B,Round_D,Null}

    [Header("����")]
    public State currentState;  //��ǰִ��״̬

    [Header("ʱ��")]
    public float PlayerDeltaTime;
    public float TypeSelectDeltaTime;

    [Header("QTEʱ��")]
    public float TypeSelectMaxTime = 5f;
    public float QTEMaxTime = 5f;


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

    [Header("CanvasPanel")]
    public Image Round_Player_Progress; 
    public GameObject Type_Panel;       //ABD���
    public GameObject QTE_Panel;       //QTE���
    public Scrollbar QTE_Scrollbar;
    public Image Enemy_health_value;
    public Image Enemy_Toughness_value;

    [Header("����")]
    public GameObject Enemy;

    [Header("�Զ���ȡ��ֵ")]
    public Transform enemyOriginTransform;
    public Vector3 playerOriginPos;
    public Vector3 enemyClosePos;

    //��϶�����
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
            State.Round_B => UpdateRound_BState(),
            State.Round_D => UpdateRound_DState(),
            _ => currentState
        };
    }

    //private void ChangeState(State nextState)  //�л�״̬����
    //{
    //    if (currentState != nextState)
    //    {
    //        stateLock = true;
    //        currentState = nextState;
    //    }
    //}

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
            QTE_Panel.SetActive(false);
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
            Time.timeScale = 0.1f;
        }
        //һֱִ��
        TypeSelectDeltaTime += Time.unscaledDeltaTime;

        //ҳ����ʾ
        //Type_Panel.SetActive(true);  //��Type���
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
            //����5s��ѡ��A
            //return State.Round_A;
            return ChangeState(State.Round_A);
        }
        
        return State.Round; //ά��Round״̬

    }

    private State UpdateRound_AState()  //Round_A״̬
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
            //���Ŷ���
            gameObject.GetComponent<Animator>().Play("cx1skill2");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Enemy_health_value.fillAmount = Enemy_health_value.fillAmount - 0.5f;  //�ػ���Ѫ
                Debug.Log("QTEA Success���ػ�Boss��Ѫ");
            }
            else if(TypeSelectDeltaTime > TypeSelectMaxTime)
            {
                Enemy_health_value.fillAmount = Enemy_health_value.fillAmount - 0.1f;  //���û��Ѫ
                Debug.Log("QTEA Failed�����Boss��0.1fѪ");
            }
            transform.position = playerOriginPos;
            TypeSelectDeltaTime = 0;
            PlayerDeltaTime = 0;
            return ChangeState(State.WaitForRound);
        }

        return State.Round_A;
    }

    private State UpdateRound_BState()  //Round_A״̬
    {
        if (stateLock)
        {
            stateLock = false;
            Debug.Log("Round_DState State");
        }
        return State.Round_B;
    }

    private State UpdateRound_DState()  //Round_A״̬
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
            //���Ŷ���
            gameObject.GetComponent<Animator>().Play("cx1skill2");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.5f;  //�ػ�����
                Debug.Log("QTEA Success���ػ�Boss����");
            }
            else if (TypeSelectDeltaTime > TypeSelectMaxTime)
            {
                Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.1f;  //���0.1f��
                Debug.Log("QTEA Failed�����Boss��0.1f����");
            }
            transform.position = playerOriginPos;
            TypeSelectDeltaTime = 0;
            PlayerDeltaTime = 0;
            return ChangeState(State.WaitForRound);
        }


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Time.timeScale = 1f;
        //    //���Ŷ���
        //    gameObject.GetComponent<Animator>().Play("cx1skill2");
        //    Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.5f;  //�ػ���Ѫ
        //    Debug.Log("QTEA Success���ػ�Boss����");
        //    transform.position = playerOriginPos;
        //    TypeSelectDeltaTime = 0;
        //    PlayerDeltaTime = 0;
        //    return ChangeState(State.WaitForRound);
        //}
        //if (TypeSelectDeltaTime > TypeSelectMaxTime)
        //{
        //    Time.timeScale = 1f;
        //    //���Ŷ���
        //    gameObject.GetComponent<Animator>().Play("cx1skill2");
        //    Debug.Log("QTEA Failed�����Boss��0.1f����");
        //    Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.1f;  //���û��Ѫ
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
