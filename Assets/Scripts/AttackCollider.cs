using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class AttackCollider : MonoBehaviour
{
    private Dictionary<string, PolygonCollider2D> attackColliders = new Dictionary<string, PolygonCollider2D>();

    void Start()
    {
        // Register attack colliders at the start
        foreach (Transform child in transform)
        {
            PolygonCollider2D collider = child.GetComponent<PolygonCollider2D>();
            if (collider != null)
            {
                string skillFrameKey = child.gameObject.name;
                RegisterAttackCollider(skillFrameKey, collider);
            }
        }
    }

    // Method to register a polygon collider for a specific skill and frame
    public void RegisterAttackCollider(string skillFrameKey, PolygonCollider2D collider)
    {
        if (!attackColliders.ContainsKey(skillFrameKey))
        {
            attackColliders.Add(skillFrameKey, collider);
            collider.enabled = false; // Disable the collider by default
        }
    }

    // Method to activate the collider for a specific skill and frame
    public void ActivateCollider(string skillFrameKey)
    {
        if (attackColliders.ContainsKey(skillFrameKey))
        {
            attackColliders[skillFrameKey].enabled = true;
        }
    }

    // Method to deactivate the collider for a specific skill and frame
    public void DeactivateCollider(string skillFrameKey)
    {
        if (attackColliders.ContainsKey(skillFrameKey))
        {
            attackColliders[skillFrameKey].enabled = false;
        }
    }

    // Called by Animation Event to trigger attack
    public void TriggerAttack(string skillFrameKey, LayerMask enemyLayer)
    {
        if (attackColliders.ContainsKey(skillFrameKey))
        {
            PolygonCollider2D attackCollider = attackColliders[skillFrameKey];

            // Get all colliders within the attack range
           Collider2D[] hitEnemies = Physics2D.OverlapCollider(attackCollider, new ContactFilter2D { layerMask = enemyLayer });

            // Process each hit enemy
            foreach (Collider2D enemy in hitEnemies)
            {
                // Assume enemy has a component that can take damage
                enemy.GetComponent<Enemy>().TakeDamage();
            }

            // Deactivate the collider after the attack
            DeactivateCollider(skillFrameKey);
        }
    }
}*/