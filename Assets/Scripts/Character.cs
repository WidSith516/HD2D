using System;
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
    //角色最大韧性与当前韧性
    public float MaxToughness;
    public float CurrentToughness;
    //角色战斗属性
    
    public enum AttackType
    {
        Attack,
        Prop,
        Defense,
    }

    public int turn;

    //行动条
    public float actionSpeed = 100f;

    private void Update()
    {



    }
    public void TakeDamage()
    {
        //引入战斗公式造成伤害
        Debug.Log(Character_Name+"被击中了" );
    }
}

