using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    GameObject saveMenu;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        saveMenu = GameObject.Find("/SaveLoadProva");
        saveMenu.SetActive(false);

    }
    public void Menu()
    {
        saveMenu.SetActive(false);
        SceneManager.LoadScene("Menu");

    }
    public void Opcions()
    {
        
        SceneManager.LoadScene("Opcions");
    }
    public void Sortir()
    {
        Application.Quit();
    }

    public void Level1()
    {
        saveMenu.SetActive(true);
        /*SceneManager.LoadScene("Scene1");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
        SceneManager.LoadScene("ProvesCiria");
    }

    public void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 4)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(index + 1);
        }
    }

    public void ReloadScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
}
