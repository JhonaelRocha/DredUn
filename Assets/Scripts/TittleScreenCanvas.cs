using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TittleScreenCanvas : MonoBehaviour
{
    public GameObject gameStartButton;
    GameObject[] players;
    public Sprite gamePad, keyborad;
    public GameObject[] playerPanels;
    public PostProcessVolume postProcess;

    public Toggle togglePostProcess;
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 0)
        {
            gameStartButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameStartButton.GetComponent<Button>().interactable = false;
        }

        

        for(int i = 0; i < players.Length; i++)
        {
            if(players[i] != null)
            {
                playerPanels[i].SetActive(true);
                Color[] colors = players[i].GetComponent<Player>().playersColors;
                playerPanels[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = colors[i];
            }
        }

    }

    public void GameStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Start()
    {
        if(!PlayerPrefs.HasKey("PostProcess")) PlayerPrefs.SetInt("PostProcess", 1);
        postProcess.gameObject.SetActive(PlayerPrefs.GetInt("PostProcess") == 1? true : false);
        togglePostProcess.isOn = PlayerPrefs.GetInt("PostProcess") == 1? true : false;
    }


    public void TogglePostProcess()
    {
        postProcess.gameObject.SetActive(togglePostProcess.isOn);
        PlayerPrefs.SetInt("PostProcess", togglePostProcess.isOn? 1 : 0);
    }
}
