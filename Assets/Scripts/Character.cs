using System;
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
    //��ɫ��������뵱ǰ����
    public float MaxToughness;
    public float CurrentToughness;
    //��ɫս������
    
    public enum AttackType
    {
        Attack,
        Prop,
        Defense,
    }

    public int turn;

    //�ж���
    public float actionSpeed = 100f;

    private void Update()
    {



    }
    public void TakeDamage()
    {
        //����ս����ʽ����˺�
        Debug.Log(Character_Name+"��������" );
    }
}

