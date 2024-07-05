using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{
    public static Character Instance;
    //角色个人信息
    public string Character_Name;
    public string Character_Description;
    public int Character_ID;
    //角色等级（影响成长曲线）
    public int Character_Level;
    //角色最大生命值与当前生命值
    public float MaxHealth;
    public float CurrentHealth;
    //角色最大法力与当前法力
    public float MaxMagic;
    public float CurrentMagic;
    //角色战斗属性（可能应用于战斗公式中）
    public float Attack;
    public float Defend;
    //行动条
    public float actionSpeed;
    public float maxAction;
    public float currentAction;
    private void OnEnable()
    {
        PoolClass.State.StateAdded += OnStateAdded;
        PoolClass.State.StateRemoved += OnStateRemoved;
    }

    private void OnDisable()
    {
        PoolClass.State.StateAdded -= OnStateAdded;
        PoolClass.State.StateRemoved -= OnStateRemoved;
    }

    private void OnStateAdded(PoolClass.State state)
    {
        // 实例化并添加状态UI
        // TODO: 实现状态UI的创建逻辑
    }

    private void OnStateRemoved(PoolClass.State state)
    {
        // 移除状态UI
        // TODO: 实现状态UI的销毁逻辑
    }
    protected virtual void Awake()
    {
        Instance = this;
    }
    protected virtual void Start()
    {
        
    }
    void Update()
    {
        
    }

    // 枚举角色的不同动作或状态
    public enum Actiontext
    {
        Idle,
        Attack,
        Defend,
        TakeDamage,
        Die
    }

    // 虚方法：获取每个状态对应的语音文本，子类可以重写此方法
    protected virtual Dictionary<Actiontext, string> GetActionTexts()
    {
        return new Dictionary<Actiontext, string>()
        {
            { Actiontext.Idle, "I'm ready and waiting." },
            { Actiontext.Attack, "Take this!" },
            { Actiontext.Defend, "Shield up!" },
            { Actiontext.TakeDamage, "Ouch!" },
            { Actiontext.Die, "I'll be back..." }
        };
    }

    // 方法：根据当前状态播放相应的语音文本
    public void PlayActionText(Actiontext action)
    {
        var actionTexts = GetActionTexts();
        if (actionTexts.ContainsKey(action))
        {
            string text = actionTexts[action];
            // 这里你可以调用播放语音的方法，例如：
            PlayVoiceClip(text);
        }
    }

    // 模拟播放语音的方法，你需要根据你的项目需求实现这个方法
    private void PlayVoiceClip(string text)
    {
        Debug.Log("Playing voice clip: " + text);
        // 在这里播放对应的语音文件
    }
    public virtual float ChangeHealth(float damage_num) 
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth+damage_num,0,MaxHealth);
        if (CurrentHealth <= 0) 
        {
            //角色死亡，播放死亡动画和语音
        }
        return CurrentHealth;
        
       
    } 
    public virtual float ChangeMagic(float magic_cost) {
        CurrentMagic = Mathf.Clamp(CurrentMagic + magic_cost, 0, MaxMagic);
        if (CurrentMagic <= 0)
        {
            //魔力亏空，不能使用技能
        }
        return CurrentMagic;
    }
    public bool TakeDamege(float atk, float def)
    {
        float dmg = atk - def;//造成伤害和伤害计算公式需要保持相同
        ChangeHealth(-dmg);
        if (CurrentHealth < 0)
            return true; 
        else
            return false;
    }
}

