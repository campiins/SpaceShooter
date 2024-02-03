using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;

    private int _levelToLoad;

    private Animator _anim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        _anim = GetComponent<Animator>();
    }

    private void FadeToScene(int buildIndex)
    {
        _levelToLoad = buildIndex;
        _anim.SetBool("FadeOut", true);
        Debug.Log("FadeOut animation triggered.");
    }

    public void OpenNextScene()
    {
        FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenMainMenu()
    {
        Debug.Log("Fading to menu...");
        FadeToScene(0);
    }

    public void RestartScene()
    {
        FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Se ejecuta desde evento de animación.
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
