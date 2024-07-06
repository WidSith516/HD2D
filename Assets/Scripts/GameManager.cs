using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


 
    //public enum BattleStage
    //{
    //    Start, // 战斗开始
    //    PlayerTurn, // 我方回合
    //    EnemyTurn, // 敌方回合
    //    Win, // 胜利
    //    Lose, // 失败
    //    Wait // 等待回合
    //}

    //public BattleStage state;

    // 创建两对象存放玩家和敌人的预制体
    //public GameObject playerPrefab;
    //public GameObject enemyPrefab;

    // 创建两个类来存放玩家和敌人脚本
    //public Player player;
    //public Enemy enemy;

    // 创建一个文本类型变量来存放战斗中的文字
    public Text dialogText;
    public UImanager playerHUD;
    public UImanager enemyHUD;

    // 供玩家进行决策的数组及面板
    public Transform[] battleChoosePos;
    public GameObject ChoosePanel;
    public GameObject ChooseAction;

    //int turn = 1; // 回合内可行动次数

    public static float gameTime;

    public int level;
    public int Pround;
    public int Eround;

    public float playerActionSpeed;
    public float enemyActionSpeed;
    public GameObject Player;
    public GameObject Enemy;
    public GameObject QTE_Panel;
    public GameObject QTE;

    public Image Round_Player_Progress;
    public Image Round_Enemy_Progress;
    public Image QTE_Progress;

    private bool PlayerRound;
    private float PlayerDeltaTime;

    public float TypeSelectTime = 0.5f;
    public float TypeSelectDeltaTime = 0f;

    public float QTETime = 0.5f;
    public float QTEDeltaTime = 0f;

    public bool QTElock;
    //private PoolClass.StatePool statePool;
    //private PoolClass.SkillPool skillPool;

    private void Awake()
    {
        //Instance = this;
        gameTime = 0;
        PlayerDeltaTime = 0;
    }

    //void Start()
    //{
    //    dialogText.text = "战斗！";
    //    state = BattleStage.Start;
    //    statePool = new PoolClass.StatePool();
    //    skillPool = new PoolClass.SkillPool();
    //    StartCoroutine(SetupBattle());
    //}

    void Update()
    {
        gameTime += Time.deltaTime;
        PlayerDeltaTime += Time.deltaTime;

        //if ((gameTime - playerActionSpeed * Pround) / playerActionSpeed > 1)
        //{
        //    Pround++;
        //    //Debug.Log(Pround);
        //    Player.transform.position += Vector3.right * 1f;
        //}

        if (!PlayerRound)
        {
            if(PlayerDeltaTime > playerActionSpeed)
            {
                QTElock = true;
                PlayerRound = true;
            }
        }
        else
        {
            if (QTElock)
            {
                QTElock = false;
                Pround++;
                transform.DOMove( Player.transform.position += Vector3.right * 0.8f,12);
                QTE_Panel.SetActive(true);
                //回合内任务
                StartCoroutine(PlayerTurn());
            }
            else
            {
                PlayerDeltaTime = 0;
            }
        }



        if ((gameTime - enemyActionSpeed * Eround) / enemyActionSpeed > 1)
        {
            Eround++;
            //Debug.Log(Eround);
            transform.DOMove(Enemy.transform.position -= Vector3.right * 0.8f,12);
        }

        Round_Player_Progress.fillAmount = (gameTime - playerActionSpeed * Pround) / playerActionSpeed;
        Round_Enemy_Progress.fillAmount = (gameTime - enemyActionSpeed * Pround) / enemyActionSpeed;



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
            if (Input.GetKeyDown(KeyCode.A) || TypeSelectDeltaTime > 5f)
            {
                QTE.SetActive(true);
                StartCoroutine(QTETurn());
                //Time.timeScale = 1f;
                //Debug.Log("out");
                break;
            } 
            yield return null;
        }
        yield return null;
        ////重置事件
        //PlayerRound = false;
        //PlayerDeltaTime = 0;
    }

    IEnumerator QTETurn()
    {
        while (true)
        {
            QTEDeltaTime += Time.unscaledDeltaTime;
            QTE_Progress.fillAmount = QTEDeltaTime / 5f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1f;
                Debug.Log("QTE Success");
                break;
            }
            if (QTEDeltaTime > 5f)
            {
                Time.timeScale = 1f;
                Debug.Log("QTE Failed");
                break;
            }
            yield return null;
        }
        yield return null;
        //重置事件
        Time.timeScale = 1f;
        PlayerRound = false;
        PlayerDeltaTime = 0;

    }



        //private IEnumerator SetupBattle()
        //{
        //    enemy = enemyPrefab.GetComponent<Enemy>();
        //    player = playerPrefab.GetComponent<Player>();
        //    Debug.Log("战斗开始方法加载中");
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
        //    dialogText.text = "现在是" + player.Character_Name + "的回合！";
        //    ChoosePanel.SetActive(true);
        //}

        //private IEnumerator EnemyTurn()
        //{
        //    dialogText.text = "现在是" + enemy.Character_Name + "的回合！";
        //    yield return new WaitForSeconds(1f);
        //    bool isDefeated = player.TakeDamege(enemy.Attack, player.Defend);
        //    float dmg = Attackmathmetic(enemy.Attack, player.Defend);
        //    dialogText.text = enemy.Character_Name + "对" + player.Character_Name + "造成了" + dmg + "点伤害!";
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
        //                // 添加技能使用逻辑
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
        //    dialogText.text = player.Character_Name + "正在普通攻击";
        //    bool isDefeated = enemy.TakeDamege(player.Attack, enemy.Defend);
        //    yield return new WaitForSeconds(1f);
        //    float dmg = player.Attack - enemy.Defend;
        //    dialogText.text = player.Character_Name + "对" + enemy.Character_Name + "造成了" + dmg + "点伤害!";
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
        //    // 示例技能使用逻辑
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
        //    // 战斗结束逻辑
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



    }

