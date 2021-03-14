using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : HealthSystem
{
    public Image healthBar;

    public override void OnHealthChanged(int health)
    {
        healthBar.fillAmount = (float)health/(float)MaxHealth;
    }

    public override void Death()
    {
        base.Death();
        LevelManager.Instance.GameOver();
    }
}
