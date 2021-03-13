using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour 
{
    public int MaxHealth = 100;
    int health;
    public Color damageColor;
    public SpriteRenderer sprite;

    void Start()
    {
        health = MaxHealth;
    }
    public virtual void TakeDamage(int damageAmount){
        health = Mathf.Clamp(health-damageAmount,0,MaxHealth);

        OnHealthChanged(health);
     
        if(health <= 0){
            Death();
            return;
        }
        if(sprite!=null){
            sprite.color = damageColor;
            Invoke(nameof(ResetSpriteColor),0.5f);
        }
        
    }

    public virtual void Heal(int healAmount){
        health = Mathf.Clamp(health+healAmount,0,MaxHealth);
        OnHealthChanged(health);
    }

    public virtual void Death(){
        Destroy(gameObject);
    }

    public virtual void OnHealthChanged(int health){
        
    }

    void ResetSpriteColor(){
        sprite.color = Color.white;
    }
}
