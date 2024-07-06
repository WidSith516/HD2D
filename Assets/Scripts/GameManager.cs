using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Rendering.DebugUI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    //public enum BattleStage
    //{
    //Start, // ս����ʼ
    //    PlayerTurn, // �ҷ��غ�
    //    EnemyTurn, // �з��غ�
    //    Win, // ʤ��
    //    Lose, // ʧ��
    //    Wait // �ȴ��غ�
    //}

    //public BattleStage state;

    // ��������������Һ͵��˵�Ԥ����
    //public GameObject playerPrefab;
    //public GameObject enemyPrefab;

    // �����������������Һ͵��˽ű�
    //public Player player;
    //public Enemy enemy;

    // ����һ���ı����ͱ��������ս���е�����
    //public Text dialogText;
    //public UImanager playerHUD;
    //public UImanager enemyHUD;

    // ����ҽ��о��ߵ����鼰���
    //public Transform[] battleChoosePos;
    //public GameObject ChoosePanel;
    //public GameObject ChooseAction;

    //int turn = 1; // �غ��ڿ��ж�����

    public static float gameTime;

    public int level;
    public int Pround;
    public int Eround;

    public float playerActionSpeed;
    public float enemyActionSpeed;
    //public GameObject Player;
    //public GameObject Enemy;
    public GameObject QTE_Panel;
    //public GameObject QTE;
    public GameObject Type_Panel;

    public Image Round_Player_Progress;
    public Image Round_Enemy_Progress;
    //public Image QTE_Progress;
    public Scrollbar QTE_Scrollbar;

    //private bool PlayerRound;
    //private float PlayerDeltaTime;

    public float TypeSelectTime = 0.5f;
    public float TypeSelectDeltaTime = 0f;

    public float QTETime = 0.5f;
    public float QTEDeltaTime = 0f;

    public bool QTElock;
    //private PoolClass.StatePool statePool;
    //private PoolClass.SkillPool skillPool;
    //public Animator playerAnimator;

    //public Transform playerOriginTransform;
    public Transform enemyOriginTransform;
    public Vector3 playerOriginPos;
    public Vector3 enemyClosePos;

    public GameObject Player;
    public GameObject Enemy;
    public GameObject QTE_State_Text;

    public Image Enemy_health_value;
    public Image Enemy_Toughness_value;

    private void Awake()
    {
        //Instance = this;
        gameTime = 0;
        //PlayerDeltaTime = 0;
        //playerOriginTransform = Player.transform;
        playerOriginPos = Player.transform.position;
        enemyOriginTransform = Enemy.transform;
        enemyClosePos = Enemy.transform.position -  Vector3.right * 2f;;
    }

    void Update()
    {
        //******
        //gameTime += Time.deltaTime;
        //PlayerDeltaTime += Time.deltaTime;

        //if (!PlayerRound)
        //{
        //    if(PlayerDeltaTime > playerActionSpeed)
        //    {
        //        QTElock = true;
        //        PlayerRound = true;
        //    }
        //}
        //else
        //{
        //    if (QTElock)
        //    {
        //        QTElock = false;
        //        Pround++;
        //        //Player.transform.position += Vector3.right * 1f;
        //        QTE_Panel.SetActive(true);
        //        //�غ�������
        //        StartCoroutine(PlayerTurn());
        //        QTE_State_Text.SetActive(false);
        //    }
        //    else
        //    {
        //        PlayerDeltaTime = 0;
        //    }
        //}

        //if (!PlayerRound)
        //{
            //if(PlayerDeltaTime > playerActionSpeed)
            //{
                //QTElock = true;
                //PlayerRound = true;
            //}
        //}
        //else
        //{
        //    if (QTElock)
        //    {
        //        QTElock = false;
        //        Pround++;
        //        //transform.DOMove( Player.transform.position += Vector3.right * 0.8f,12);
        //        QTE_Panel.SetActive(true);
        //        //�غ�������
        //        StartCoroutine(PlayerTurn());
        //    }
        //    else
        //    {
        //        //PlayerDeltaTime = 0;
        //    }
        //}

        //if ((gameTime - enemyActionSpeed * Eround) / enemyActionSpeed > 1)
        //{
        //    Eround++;
            //Debug.Log(Eround);
            //transform.DOMove(Enemy.transform.position -= Vector3.right * 0.8f,12);
        //}

        //Round_Player_Progress.fillAmount = (gameTime - playerActionSpeed * Pround) / playerActionSpeed;
        //Round_Enemy_Progress.fillAmount = (gameTime - enemyActionSpeed * Pround) / enemyActionSpeed;


        //if (state == BattleStage.PlayerTurn && turn == 0)
        //{
        //    player.currentAction += Time.deltaTime * player.actionSpeed;
        //    playerHUD.UpdateAct(player.currentAction);
        //    enemy.currentAction += Time.deltaTime * enemy.actionSpeed;
        //    enemyHUD.UpdateAct(enemy.currentAction);
        //    if (player.currentAction >= player.maxAction)
        //    {
        //        state = BattleStage.PlayerTurn;
        //        player.currentAction = 0;
        //        turn = 1;
        //        PlayerTurn();
        //    }
        //    else if (enemy.currentAction >= enemy.maxAction)
        //    {
        //        state = BattleStage.EnemyTurn;
        //        enemy.currentAction = 0;
        //        turn = 1;
        //        StartCoroutine(EnemyTurn());
        //    }
        //}
        //if (state == BattleStage.PlayerTurn && turn > 0)
        //{
        //    BattleChoose();
        //}
    }

    IEnumerator  PlayerTurn()
    {
        while (true) {
            TypeSelectDeltaTime += Time.unscaledDeltaTime;
            Time.timeScale = 0.1f;
            Type_Panel.SetActive(true);
            if (Input.GetKeyDown(KeyCode.A) || TypeSelectDeltaTime > 5f)
            {
                Player.GetComponent<Animator>().Play("cx1skill2");
                Player.transform.position = enemyClosePos;
                StartCoroutine(QTEATurn());
                //Debug.Log("out");
                break;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Player.GetComponent<Animator>().Play("cx1skill2");
                Player.transform.position = enemyClosePos;
                StartCoroutine(QTEDTurn());
                Debug.Log("D out");
                break;
            }
            yield return null;
        }
        yield return null;
    }

    //�����غ�
    IEnumerator QTEATurn()
    {
        while (true)
        {
            QTEDeltaTime += Time.unscaledDeltaTime;
            QTE_Scrollbar.value = QTEDeltaTime / 5f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                QTE_State_Text.SetActive(true);
                QTE_State_Text.GetComponent<TMP_Text>().text = "Excellent";
                Time.timeScale = 1f;
                Debug.Log("QTEA Success���ػ�Boss��Ѫ");
                Enemy_health_value.fillAmount = Enemy_health_value.fillAmount - 0.5f;  //�ػ���Ѫ
                break;
            }
            if (QTEDeltaTime > 5f)
            {
                QTE_State_Text.SetActive(true);
                QTE_State_Text.GetComponent<TMP_Text>().text = "Failed";
                Time.timeScale = 1f;
                Debug.Log("QTEA Failed�����Boss��0.1fѪ");
                Enemy_health_value.fillAmount = Enemy_health_value.fillAmount- 0.1f;  //���û��Ѫ
                break;
            }
            yield return null;
        }
        yield return null;
        //�����¼�
        Time.timeScale = 1f;
        //PlayerRound = false;
        //PlayerDeltaTime = 0;
        QTEDeltaTime = 0;
        TypeSelectDeltaTime = 0;
        Player.transform.position = playerOriginPos;

        QTE_Scrollbar.value = 0;
    }

    //���ͻغ�
    IEnumerator QTEDTurn()
    {
        while (true)
        {
            QTEDeltaTime += Time.unscaledDeltaTime;
            QTE_Scrollbar.value = QTEDeltaTime / 5f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1f;
                Debug.Log("QTED Success���ػ�Boss����");
                Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.5f;  //�ػ���Ѫ
                break;
            }
            if (QTEDeltaTime > 5f)
            {
                Time.timeScale = 1f;
                Debug.Log("QTED Failed�����Boss��0.1f��");
                Enemy_Toughness_value.fillAmount = Enemy_Toughness_value.fillAmount - 0.1f;  //���û��Ѫ
                break;
            }
            yield return null;
        }
        yield return null;
        //�����¼�
        Time.timeScale = 1f;
        //PlayerRound = false;
        //PlayerDeltaTime = 0;
        QTEDeltaTime = 0;
        TypeSelectDeltaTime = 0;
        Player.transform.position = playerOriginPos;

        QTE_Scrollbar.value = 0;
    }



    //private IEnumerator SetupBattle()
    //{
    //    enemy = enemyPrefab.GetComponent<Enemy>();
    //    player = playerPrefab.GetComponent<Player>();
    //    Debug.Log("ս����ʼ����������");
    //    dialogText.text = enemy.Character_Description;
    //    playerHUD.InitHUD(player);
    //    enemyHUD.InitHUD(enemy);
    //    yield return new WaitForSeconds(1f);

    //    if (player.actionSpeed > enemy.actionSpeed)
    //    {
    //        state = BattleStage.PlayerTurn;
    //        PlayerTurn();
    //    }
    //    else
    //    {
    //        state = BattleStage.EnemyTurn;
    //    }
    //}

    //private void PlayerTurn()
    //{
    //    dialogText.text = "������" + player.Character_Name + "�Ļغϣ�";
    //    ChoosePanel.SetActive(true);
    //}

    //private IEnumerator EnemyTurn()
    //{
    //    dialogText.text = "������" + enemy.Character_Name + "�Ļغϣ�";
    //    yield return new WaitForSeconds(1f);
    //    bool isDefeated = player.TakeDamege(enemy.Attack, player.Defend);
    //    float dmg = Attackmathmetic(enemy.Attack, player.Defend);
    //    dialogText.text = enemy.Character_Name + "��" + player.Character_Name + "�����" + dmg + "���˺�!";
    //    playerHUD.UpdateHp(player.CurrentHealth);
    //    if (isDefeated)
    //    {
    //        state = BattleStage.Lose;
    //    }
    //    else
    //    {
    //        ChoosePanel?.SetActive(false);
    //        state = BattleStage.Wait;
    //        StartCoroutine(Wait());
    //    }
    //}

    //private void BattleChoose()
    //{
    //    int i = 0;
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        i = i - 1;
    //        ChooseAction.transform.position = battleChoosePos[i].position;
    //    }
    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        i = i + 1;
    //    }
    //    if (i == -1)
    //    {
    //        i = 3;
    //        ChooseAction.transform.position = battleChoosePos[i].position;
    //    }
    //    if (i == battleChoosePos.Length)
    //    {
    //        i = 0;
    //        ChooseAction.transform.position = battleChoosePos[i].position;
    //    }
    //    if (Input.GetKey(KeyCode.Z))
    //    {
    //        turn -= 1;
    //        switch (i)
    //        {
    //            case 0:
    //                StartCoroutine(PlayerAttack());
    //                break;
    //            case 1:
    //                // ��Ӽ���ʹ���߼�
    //                UseSkill();
    //                break;
    //            case 2:
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //}

    //private IEnumerator PlayerAttack()
    //{
    //    dialogText.text = player.Character_Name + "������ͨ����";
    //    bool isDefeated = enemy.TakeDamege(player.Attack, enemy.Defend);
    //    yield return new WaitForSeconds(1f);
    //    float dmg = player.Attack - enemy.Defend;
    //    dialogText.text = player.Character_Name + "��" + enemy.Character_Name + "�����" + dmg + "���˺�!";
    //    enemyHUD.UpdateHp(enemy.CurrentHealth);
    //    if (isDefeated)
    //    {
    //        state = BattleStage.Win;
    //    }
    //    else
    //    {
    //        ChoosePanel?.SetActive(false);
    //        state = BattleStage.Wait;
    //        StartCoroutine(Wait());
    //    }
    //}

    //private void UseSkill()
    //{
    //    // ʾ������ʹ���߼�
    //    PoolClass.BuffSkill skill = new PoolClass.BuffSkill("Buff", player, 10);
    //    skillPool.AddSkill(skill);
    //    skillPool.UseSkill(skill);
    //}

    //private int Attackmathmetic(float a, float b)
    //{
    //    return (int)(a - b);
    //}

    //private void BattleEnd()
    //{
    //    // ս�������߼�
    //}

    //private IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(1f);
    //    if (state == BattleStage.Wait)
    //    {
    //        if (enemy.CurrentHealth <= 0)
    //        {
    //            state = BattleStage.Win;
    //            BattleEnd();
    //        }
    //        else if (player.CurrentHealth <= 0)
    //        {
    //            state = BattleStage.Lose;
    //            BattleEnd();
    //        }
    //        else
    //        {
    //            state = BattleStage.PlayerTurn;
    //        }
    //    }
    //}



    //20240706
}

