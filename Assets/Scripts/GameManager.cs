using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instância estática para o Singleton
    public static GameManager instance;
    private PostProcessVolume postProcess;

    private GameObject[] players;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 1)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        postProcess = FindObjectOfType<PostProcessVolume>();
        if(postProcess == null) return;
        if(PlayerPrefs.HasKey("PostProcess"))
        {
            if(PlayerPrefs.GetInt("PostProcess") == 1)
            {
                postProcess.gameObject.SetActive(true);
            }
            else
            {
                postProcess.gameObject.SetActive(false);
            }
        }
    }
}
