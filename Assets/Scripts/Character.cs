using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{

    //角色最大生命值与当前生命值
    public float MaxHealth;
    public float CurrentHealth;
    //角色最大韧性与当前韧性
    public float MaxToughness;
<<<<<<< HEAD
    public float CurrentMagic;
=======
    public float CurrentToughness;
    //角色战斗属性
    
    public enum AttackType
    {
        Attack,
        Prop,
        Defense,
    }

    public int turn;

>>>>>>> 615a4e9e6f2e2940685789199062fbde24ecebe2
    //行动条
    public float actionSpeed = 100f;

    //public int turn;



<<<<<<< HEAD
    //角色个人信息
    //private string Character_Name;
    //private string Character_Description;
    //private int Character_ID;
    //角色等级（影响成长曲线）
    //private int Character_Level;

=======
    }
    public void TakeDamage()
    {
        //引入战斗公式造成伤害
        Debug.Log(Character_Name+"被击中了" );
    }
>>>>>>> 615a4e9e6f2e2940685789199062fbde24ecebe2
}

