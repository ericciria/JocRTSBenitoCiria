using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectLife : MonoBehaviour
{
    [SerializeField] float health = 1;
    public float maxHealth = 100;
    private Image healthBarImage;

    private void Start()
    {
        //healthBarImage = FindObjectOfType<Image>();
        healthBarImage = GetComponentInChildren<Image>();
        if (healthBarImage != null)
        {
            SetHealthBarValue(health / maxHealth);
            healthBarImage.gameObject.SetActive(false);
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        //Debug.Log(health);
        if (health <= 0)
        {
            // He posat que destrueixi el parent per com he fet la torreta, que requeria d'un transform separat per tornar
            // a la posició original, i he hagut de posar-li parents a totes les unitats
            Destroy(transform.parent.gameObject);
        }
        if (healthBarImage!=null) {
            healthBarImage.gameObject.SetActive(true);
            SetHealthBarValue(health/maxHealth);
        }
    }
    public void heal(int healAmount)
    {
        health += healAmount;
        if (health >= maxHealth)
        {
            health = maxHealth;
            if (healthBarImage != null)
            {
                SetHealthBarValue(1);
                healthBarImage.gameObject.SetActive(false);
            }
        }
        else
        {
            if (healthBarImage != null)
            {
                SetHealthBarValue(health / maxHealth);
                healthBarImage.gameObject.SetActive(true);
            }
        }
    }

    public void setHealth(float amount)
    {
        health = amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
            if (healthBarImage != null)
            {
                SetHealthBarValue(1);
                healthBarImage.gameObject.SetActive(false);
            }
        }
        else
        {
            if (healthBarImage != null)
            {
                SetHealthBarValue(health / maxHealth);
                healthBarImage.gameObject.SetActive(true);
            }
        }
    }
    public void setMaxHealth(float amount)
    {
        maxHealth = amount;
    }
    public float getHealth()
    {
        return health;
    }

    public void SetHealthBarValue(float value)
    {
        healthBarImage.fillAmount = value;
        if (healthBarImage.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (healthBarImage.fillAmount < 0.5f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    public void SetHealthBarColor(Color healthColor)
    {
        healthBarImage.color = healthColor;
    }
}
