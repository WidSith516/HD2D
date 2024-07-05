using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolClass : MonoBehaviour
{
    // 状态类定义
    public abstract class State
    {
        public string StateName;
        public int Duration;
        protected Character Target;

        public State(string name, int duration, Character target)
        {
            StateName = name;
            Duration = duration;
            Target = target;
            OnStateAdded();
        }

        public virtual void ApplyEffect() { }

        public virtual void RemoveEffect()
        {
            OnStateRemoved();
        }

        // 状态添加和移除的事件
        protected virtual void OnStateAdded()
        {
            StateAdded?.Invoke(this);
        }

        protected virtual void OnStateRemoved()
        {
            StateRemoved?.Invoke(this);
        }

        // 事件
        public static event System.Action<State> StateAdded;
        public static event System.Action<State> StateRemoved;
    }

    // 具体的状态实现
    public class PoisonState : State
    {
        public float DamagePerTurn;

        public PoisonState(int duration, Character target, float damagePerTurn)
            : base("Poison", duration, target)
        {
            DamagePerTurn = damagePerTurn;
        }

        public override void ApplyEffect()
        {
            Target.CurrentHealth -= DamagePerTurn;
        }
    }

    // 状态池管理
    public class StatePool
    {
        private List<State> activeStates = new List<State>();

        public void AddState(State state)
        {
            activeStates.Add(state);
        }

        public void RemoveState(State state)
        {
            state.RemoveEffect();
            activeStates.Remove(state);
        }

        public void UpdateStates()
        {
            for (int i = activeStates.Count - 1; i >= 0; i--)
            {
                State state = activeStates[i];
                state.ApplyEffect();
                state.Duration--;
                if (state.Duration <= 0)
                {
                    RemoveState(state);
                }
            }
        }
    }

    // 对象池管理
    public class ObjectPool<T> where T : new()
    {
        private List<T> availableObjects = new List<T>();
        private List<T> inUseObjects = new List<T>();

        public T GetObject()
        {
            if (availableObjects.Count > 0)
            {
                T obj = availableObjects[0];
                availableObjects.RemoveAt(0);
                inUseObjects.Add(obj);
                return obj;
            }
            else
            {
                T newObj = new T();
                inUseObjects.Add(newObj);
                return newObj;
            }
        }

        public void ReleaseObject(T obj)
        {
            inUseObjects.Remove(obj);
            availableObjects.Add(obj);
        }
    }

    // 技能类定义
    public abstract class Skill
    {
        public string SkillName;
        protected Character Owner;

        public Skill(string name, Character owner)
        {
            SkillName = name;
            Owner = owner;
        }

        public virtual void Activate() { }
    }

    // 具体的技能实现
    public class BuffSkill : Skill
    {
        public float BuffAmount;

        public BuffSkill(string name, Character owner, float buffAmount)
            : base(name, owner)
        {
            BuffAmount = buffAmount;
        }

        public override void Activate()
        {
            //Owner.Attack += BuffAmount;
        }
    }

    public class DebuffSkill : Skill
    {
        public float DebuffAmount;
        public Character Target;

        public DebuffSkill(string name, Character owner, Character target, float debuffAmount)
            : base(name, owner)
        {
            DebuffAmount = debuffAmount;
            Target = target;
        }

        public override void Activate()
        {
            //Target.Defend -= DebuffAmount;
        }
    }

    public class DotSkill : Skill
    {
        public float DamagePerTurn;
        public int Duration;
        public Character Target;

        public DotSkill(string name, Character owner, Character target, float damagePerTurn, int duration)
            : base(name, owner)
        {
            DamagePerTurn = damagePerTurn;
            Duration = duration;
            Target = target;
        }

        public override void Activate()
        {
            StatePool statePool = new StatePool();
            statePool.AddState(new PoisonState(Duration, Target, DamagePerTurn));
        }
    }

    // 技能池管理
    public class SkillPool
    {
        private List<Skill> availableSkills = new List<Skill>();

        public void AddSkill(Skill skill)
        {
            availableSkills.Add(skill);
        }

        public void UseSkill(Skill skill)
        {
            skill.Activate();
        }
    }
}
