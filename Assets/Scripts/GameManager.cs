using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum BattleStage
    {
        Start, // ս����ʼ
        PlayerTurn, // �ҷ��غ�
        EnemyTurn, // �з��غ�
        Win, // ʤ��
        Lose, // ʧ��
        Wait // �ȴ��غ�
    }

    public BattleStage state;

    // ��������������Һ͵��˵�Ԥ����
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // �����������������Һ͵��˽ű�
    public Player player;
    public Enemy enemy;

    // ����һ���ı����ͱ��������ս���е�����
    public Text dialogText;
    public UImanager playerHUD;
    public UImanager enemyHUD;

    // ����ҽ��о��ߵ����鼰���
    public Transform[] battleChoosePos;
    public GameObject ChoosePanel;
    public GameObject ChooseAction;

    int turn = 1; // �غ��ڿ��ж�����

    private PoolClass.StatePool statePool;
    private PoolClass.SkillPool skillPool;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialogText.text = "ս����";
        state = BattleStage.Start;
        statePool = new PoolClass.StatePool();
        skillPool = new PoolClass.SkillPool();
        StartCoroutine(SetupBattle());
    }

    void Update()
    {
        if (state == BattleStage.PlayerTurn && turn == 0)
        {
            player.currentAction += Time.deltaTime * player.actionSpeed;
            playerHUD.UpdateAct(player.currentAction);
            enemy.currentAction += Time.deltaTime * enemy.actionSpeed;
            enemyHUD.UpdateAct(enemy.currentAction);
            if (player.currentAction >= player.maxAction)
            {
                state = BattleStage.PlayerTurn;
                player.currentAction = 0;
                turn = 1;
                PlayerTurn();
            }
            else if (enemy.currentAction >= enemy.maxAction)
            {
                state = BattleStage.EnemyTurn;
                enemy.currentAction = 0;
                turn = 1;
                StartCoroutine(EnemyTurn());
            }
        }
        if (state == BattleStage.PlayerTurn && turn > 0)
        {
            BattleChoose();
        }
    }

    private IEnumerator SetupBattle()
    {
        enemy = enemyPrefab.GetComponent<Enemy>();
        player = playerPrefab.GetComponent<Player>();
        Debug.Log("ս����ʼ����������");
        dialogText.text = enemy.Character_Description;
        playerHUD.InitHUD(player);
        enemyHUD.InitHUD(enemy);
        yield return new WaitForSeconds(1f);

        if (player.actionSpeed > enemy.actionSpeed)
        {
            state = BattleStage.PlayerTurn;
            PlayerTurn();
        }
        else
        {
            state = BattleStage.EnemyTurn;
        }
    }

    private void PlayerTurn()
    {
        dialogText.text = "������" + player.Character_Name + "�Ļغϣ�";
        ChoosePanel.SetActive(true);
    }

    private IEnumerator EnemyTurn()
    {
        dialogText.text = "������" + enemy.Character_Name + "�Ļغϣ�";
        yield return new WaitForSeconds(1f);
        bool isDefeated = player.TakeDamege(enemy.Attack, player.Defend);
        float dmg = Attackmathmetic(enemy.Attack, player.Defend);
        dialogText.text = enemy.Character_Name + "��" + player.Character_Name + "�����" + dmg + "���˺�!";
        playerHUD.UpdateHp(player.CurrentHealth);
        if (isDefeated)
        {
            state = BattleStage.Lose;
        }
        else
        {
            ChoosePanel?.SetActive(false);
            state = BattleStage.Wait;
            StartCoroutine(Wait());
        }
    }

    private void BattleChoose()
    {
        int i = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            i = i - 1;
            ChooseAction.transform.position = battleChoosePos[i].position;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            i = i + 1;
        }
        if (i == -1)
        {
            i = 3;
            ChooseAction.transform.position = battleChoosePos[i].position;
        }
        if (i == battleChoosePos.Length)
        {
            i = 0;
            ChooseAction.transform.position = battleChoosePos[i].position;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            turn -= 1;
            switch (i)
            {
                case 0:
                    StartCoroutine(PlayerAttack());
                    break;
                case 1:
                    // ��Ӽ���ʹ���߼�
                    UseSkill();
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator PlayerAttack()
    {
        dialogText.text = player.Character_Name + "������ͨ����";
        bool isDefeated = enemy.TakeDamege(player.Attack, enemy.Defend);
        yield return new WaitForSeconds(1f);
        float dmg = player.Attack - enemy.Defend;
        dialogText.text = player.Character_Name + "��" + enemy.Character_Name + "�����" + dmg + "���˺�!";
        enemyHUD.UpdateHp(enemy.CurrentHealth);
        if (isDefeated)
        {
            state = BattleStage.Win;
        }
        else
        {
            ChoosePanel?.SetActive(false);
            state = BattleStage.Wait;
            StartCoroutine(Wait());
        }
    }

    private void UseSkill()
    {
        // ʾ������ʹ���߼�
        PoolClass.BuffSkill skill = new PoolClass.BuffSkill("Buff", player, 10);
        skillPool.AddSkill(skill);
        skillPool.UseSkill(skill);
    }

    private int Attackmathmetic(float a, float b)
    {
        return (int)(a - b);
    }

    private void BattleEnd()
    {
        // ս�������߼�
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        if (state == BattleStage.Wait)
        {
            if (enemy.CurrentHealth <= 0)
            {
                state = BattleStage.Win;
                BattleEnd();
            }
            else if (player.CurrentHealth <= 0)
            {
                state = BattleStage.Lose;
                BattleEnd();
            }
            else
            {
                state = BattleStage.PlayerTurn;
            }
        }
    }
}

