using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{
    public static Character Instance;
    //��ɫ������Ϣ
    public string Character_Name;
    public string Character_Description;
    public int Character_ID;
    //��ɫ�ȼ���Ӱ��ɳ����ߣ�
    public int Character_Level;
    //��ɫ�������ֵ�뵱ǰ����ֵ
    public float MaxHealth;
    public float CurrentHealth;
    //��ɫ������뵱ǰ����
    public float MaxMagic;
    public float CurrentMagic;
    //��ɫս�����ԣ�����Ӧ����ս����ʽ�У�
    public float Attack;
    public float Defend;
    //�ж���
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
        // ʵ���������״̬UI
        // TODO: ʵ��״̬UI�Ĵ����߼�
    }

    private void OnStateRemoved(PoolClass.State state)
    {
        // �Ƴ�״̬UI
        // TODO: ʵ��״̬UI�������߼�
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

    // ö�ٽ�ɫ�Ĳ�ͬ������״̬
    public enum Actiontext
    {
        Idle,
        Attack,
        Defend,
        TakeDamage,
        Die
    }

    // �鷽������ȡÿ��״̬��Ӧ�������ı������������д�˷���
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

    // ���������ݵ�ǰ״̬������Ӧ�������ı�
    public void PlayActionText(Actiontext action)
    {
        var actionTexts = GetActionTexts();
        if (actionTexts.ContainsKey(action))
        {
            string text = actionTexts[action];
            // ��������Ե��ò��������ķ��������磺
            PlayVoiceClip(text);
        }
    }

    // ģ�ⲥ�������ķ���������Ҫ���������Ŀ����ʵ���������
    private void PlayVoiceClip(string text)
    {
        Debug.Log("Playing voice clip: " + text);
        // �����ﲥ�Ŷ�Ӧ�������ļ�
    }
    public virtual float ChangeHealth(float damage_num) 
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth+damage_num,0,MaxHealth);
        if (CurrentHealth <= 0) 
        {
            //��ɫ������������������������
        }
        return CurrentHealth;
        
       
    } 
    public virtual float ChangeMagic(float magic_cost) {
        CurrentMagic = Mathf.Clamp(CurrentMagic + magic_cost, 0, MaxMagic);
        if (CurrentMagic <= 0)
        {
            //ħ�����գ�����ʹ�ü���
        }
        return CurrentMagic;
    }
    public bool TakeDamege(float atk, float def)
    {
        float dmg = atk - def;//����˺����˺����㹫ʽ��Ҫ������ͬ
        ChangeHealth(-dmg);
        if (CurrentHealth < 0)
            return true; 
        else
            return false;
    }
}

