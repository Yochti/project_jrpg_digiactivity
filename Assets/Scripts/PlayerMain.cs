using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMain : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealthPlayer = 2000;
    [SerializeField] public int currentHealthPlayer;
    [Header("Damage")]
    [SerializeField] private int damagePlayer;
    [Header("Mana")]
    [SerializeField] public int mana = 20;
    public Image ManaImage;
    private int damageToTake = 0;

    [Header("Critic")]
    [SerializeField] private int critRate = 20;
    [SerializeField] private int critDamage = 120;

    [Header("Heal")]
    private int healPlayer;
    public Image healthBarImage;

    [Header("Boss Reference")]
    public EnemyMain boss;

    public void QTE_Return(bool success, bool WasAnAttack)
    {
        if (WasAnAttack)
        {
            damagePlayer = Random.Range(250, 401);
            // Ajouter la logique de critique
            if (Random.Range(1, 101) <= critRate)
            {
                damagePlayer = Mathf.RoundToInt(damagePlayer * (critDamage / 100f));
            }

            if (success)
            {
                boss.TakeDamage((int)(damagePlayer * 1.5));
            }
            else
            {
                boss.TakeDamage(damagePlayer);
            }

            mana += 5;
            if (mana > 20)
                mana = 20;
            if (boss.currentHealthEnemy <= 0)
            {
                // victoire
            }
            else
            {
                // continuer le jeu
            }
        }
        else
        {
            if (success)
            {
                currentHealthPlayer -= (int)(damageToTake * 0.66);
            }
            else
            {
                currentHealthPlayer -= damageToTake;
            }

            maxHealthPlayer -= damageToTake * 10 / 100;
            healthBarImage.fillAmount = (float)currentHealthPlayer / maxHealthPlayer;
            if (currentHealthPlayer <= 0)
            {
                // stopper le jeu
            }
            else
            {
                // passer à la phase suivante
            }
        }
    }

    private void Start()
    {
        currentHealthPlayer = maxHealthPlayer;
        MegaDamageToEnemy();
    }
    private void Update()
    {
        healthBarImage.fillAmount = (float)currentHealthPlayer / maxHealthPlayer;
        ManaImage.fillAmount = (float)mana / 20;
    }
    public void damageToEnemy()
    {
        battleSystem.Instance.qte.LaunchQTE(this, true, 1.5f, 0.5f, 0.4f);
    }

    public void TakeDamage(int damage)
    {
        damageToTake = damage;
        battleSystem.Instance.qte.LaunchQTE(this, false, 1.5f, 0.5f, 0.4f);
    }

    public void MegaDamageToEnemy()
    {
        int manaCost = 10;
        bool hasCrit;
        if (mana - manaCost >= 0)
        {
            mana -= manaCost;
            damagePlayer = Random.Range(800, 1001);
            if (Random.Range(1, 101) <= critRate)
            {
                hasCrit = true;
                print(hasCrit);
                damagePlayer += Mathf.RoundToInt(damagePlayer * (critDamage / 100f));
            }
            else
            {
                hasCrit = false;
                print(hasCrit);
            }
            boss.TakeDamage(damagePlayer);
            print(damagePlayer);
        }
    }

    public void IncreaseCritRateAndDamage()
    {
        int manaCost = 5;
        if (mana - manaCost >= 0)
        {
            mana -= manaCost;
            critDamage += 30;
            critRate += 30;
        }
    }

    public void Heal()
    {
        int manaCost = 5;
        if (mana - manaCost >= 0)
        {
            mana -= manaCost;
            healPlayer = Random.Range(150, 501);
            currentHealthPlayer += healPlayer;
            if (currentHealthPlayer > maxHealthPlayer)
                currentHealthPlayer = maxHealthPlayer;
        }
    }
}
