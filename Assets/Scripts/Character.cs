using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{

    //��ɫ�������ֵ�뵱ǰ����ֵ
    public float MaxHealth;
    public float CurrentHealth;
    //��ɫ��������뵱ǰ����
    public float MaxToughness;
<<<<<<< HEAD
    public float CurrentMagic;
=======
    public float CurrentToughness;
    //��ɫս������
    
    public enum AttackType
    {
        Attack,
        Prop,
        Defense,
    }

    public int turn;

>>>>>>> 615a4e9e6f2e2940685789199062fbde24ecebe2
    //�ж���
    public float actionSpeed = 100f;

    //public int turn;



<<<<<<< HEAD
    //��ɫ������Ϣ
    //private string Character_Name;
    //private string Character_Description;
    //private int Character_ID;
    //��ɫ�ȼ���Ӱ��ɳ����ߣ�
    //private int Character_Level;

=======
    }
    public void TakeDamage()
    {
        //����ս����ʽ����˺�
        Debug.Log(Character_Name+"��������" );
    }
>>>>>>> 615a4e9e6f2e2940685789199062fbde24ecebe2
}

