using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    protected override Dictionary<Actiontext, string> GetActionTexts()
    {
        return new Dictionary<Actiontext, string>()
        {
            { Actiontext.Idle, "Player is ready." },
            { Actiontext.Attack, "Player attacks!" },
            { Actiontext.Defend, "Player defends!" },
            { Actiontext.TakeDamage, "Player takes damage!" },
            { Actiontext.Die, "Player has fallen!" }
        };
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
