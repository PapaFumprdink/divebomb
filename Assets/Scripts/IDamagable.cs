using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    event DamageAction DamageEvent;
    event DamageAction DeathEvent;

    void Damage(float damage, GameObject damager, Vector2 point, Vector2 direction);
    void Kill(float damage, GameObject killer, Vector2 point, Vector2 direction);
}

public delegate void DamageAction(float damage, GameObject damager, Vector2 point, Vector2 direction);
