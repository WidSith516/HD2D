using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    //public static UImanager instance;
    //public Text nameText;
    //public Text levelText;

    //public Slider hpSlider;
    //public Slider mpSlider;
    //public Slider actSlider;

    private void Awake()
    {
        //instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitHUD(Character character)
    {
        //nameText.text = character.Character_Name;
        //levelText.text="Lv."+character.Character_Level;
        //hpSlider.maxValue = character.MaxHealth;
        //hpSlider.value = character.CurrentHealth;
        ////mpSlider.maxValue = character.MaxMagic;
        //mpSlider.value = character.CurrentMagic;
    }
    //Update¸üÐÂ×´Ì¬¼¯
    public void UpdateHp(float hp)
    {
        //hpSlider.value = hp;
    }
    public void UpdateMp(float mp)
    {
        //hpSlider.value = mp;
    }
    public void UpdateAct(float act) 
    {
        //actSlider.value = act;
    }
}
