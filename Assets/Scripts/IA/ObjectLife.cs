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
            if (GetComponentInParent<Building>() != null)
            {
                if (GetComponentInParent<Building>().team == 1)
                {
                    CameraController player = GameObject.Find("/Camera").GetComponent<CameraController>();
                    player.buildings.Remove(transform.parent.gameObject);
                    player.electricitat -= GetComponentInParent<Building>().energy;
                    if (GetComponentInParent<Building>().name.Equals("Base"))
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
                    }
                    if (player.electricitat < 0)
                    {
                        GameObject.Find("/Canvas/Slider/Background").GetComponent<Image>().color = Color.red;
                    }
                    else if (player.electricitat <= 10)
                    {
                        GameObject.Find("/Canvas/Slider/Background").GetComponent<Image>().color = Color.yellow;
                    }
                    else
                    {
                        GameObject.Find("/Canvas/Slider/Background").GetComponent<Image>().color = Color.green;
                    }
                }
                else
                {
                    GameObject.Find("/AIGeneral").GetComponent<AIGeneral>().buildings.Remove(GetComponentInParent<Building>());
                    GameObject.Find("/AIGeneral").GetComponent<AIGeneral>().energia -= GetComponentInParent<Building>().energy;
                    if (GetComponentInParent<Building>().name.Equals("Base"))
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("GameWin");
                    }
                }
            }
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
        if (healthBarImage.fillAmount < 0.25f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (healthBarImage.fillAmount < 0.75f)
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
