using UnityEngine;
using UnityEngine.UI;


public delegate void OnBtnResetClicked();


public class UIController : MonoBehaviour
{
    // 重置按钮
    Button btnReset;
    OnBtnResetClicked onBtnResetClickedDelegate;

    // 敌我双方信息条
    Image playerHp;
    Image playerBlock;
    Image enemyHp;
    Image enemyBlock;
    Image playerRound;
    Image enemyRound;
    Image playerRoundFull;
    Image enemyRoundFull;

    // 关联QTE
    public QTEController QteController;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化各种条
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            switch (image.name)
            {
                case "playerHp": 
                    playerHp = image;
                    break;
                case "playerBlock":
                    playerBlock = image;
                    break;
                case "enemyHp":
                    enemyHp = image;
                    break;
                case "enemyBlock":
                    enemyBlock = image;
                    break;
                case "playerRound":
                    playerRound = image;
                    break;
                case "enemyRound":
                    enemyRound = image;
                    break;
                case "playerRoundFull":
                    playerRoundFull = image;
                    break;
                case "enemyRoundFull":
                    enemyRoundFull = image;
                    break;
                default:
                    Debug.LogWarning(string.Format("{0} no reference get", image.name));
                    break;
            }
        }

        // 发光条默认不显示
        playerRoundFull.gameObject.SetActive(false);
        enemyRoundFull.gameObject.SetActive(false);

        // 初始化重置按钮
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            switch (button.name)
            {
                case "btnReset":
                    btnReset = button;
                    break;
                default:
                    break;
            }
        }

        QteController = Camera.main.GetComponentInChildren<QTEController>();
        QteController.Init();

        // test function
        BindBtnResetClicked(OnBtnResetClickedCB);
    }

    // 总开关
    static public void HideHUD()
    {
        Debug.Log("HideHUD");
        GameObject canvas = Camera.main.transform.Find("CanvasHUD").gameObject;
        canvas.SetActive(false);
    }

    static public void ShowHUD()
    {
        Debug.Log("ShowHUD");
        GameObject canvas = Camera.main.transform.Find("CanvasHUD").gameObject;
        canvas.SetActive(true);
    }

    public void SetPlayerHp(float cur, float ttl)
    {
        playerHp.fillAmount = cur / ttl;
    }

    public void SetPlayerDef(float cur, float ttl)
    {
        playerBlock.fillAmount = cur / ttl;
    }

    public void SetPlayerRound(float cur, float ttl)
    {
        playerRound.fillAmount = cur / ttl;
    }

    public void SetPlayerRoundFull()
    {
        playerRoundFull.gameObject.SetActive(true);
    }

    public void SetEnemyHp(float cur, float ttl)
    {
        enemyHp.fillAmount = cur / ttl;
    }

    public void SetEnemyDef(float cur, float ttl)
    {
        enemyBlock.fillAmount = cur / ttl;
    }

    public void SetEnemyRound(float cur, float ttl)
    {
        enemyRound.fillAmount = cur / ttl;
    }

    public void SetEnemyRoundFull()
    {
        enemyRoundFull.gameObject.SetActive(true);
    }

    public void BindBtnResetClicked(OnBtnResetClicked onBtnResetClicked)
    {
        onBtnResetClickedDelegate = onBtnResetClicked;
        btnReset.onClick.AddListener(onBtnResetClickedCB);
    }

    void onBtnResetClickedCB()
    {
        if (onBtnResetClickedDelegate != null)
        {
            onBtnResetClickedDelegate();
        }
    }

    void Update()
    {
        
    }

    // test function
    void OnBtnResetClickedCB()
    {
        Debug.Log("模拟外部绑定按钮点击事件");
        // QteController.StartQteTask(5, 0.5f, 0.05f, OnQteFinishedCB);
    }

    // test function
    void OnQteFinishedCB(QteResult qteResult)
    {
        Debug.Log(string.Format("模拟外部QTE结束 {0}", qteResult));
    }
}
