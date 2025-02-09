using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource[] musicas;
    public static MusicManager instance;
    public AudioSource currentMusica;
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        if (currentMusica == null) TrocarMusicaPara(musicas[0]);
        if (currentMusica != null && currentMusica != musicas[0])
        {
            TrocarMusicaPara(musicas[0]);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName[0])
        {
            case '1':
                if (currentMusica == null) TrocarMusicaPara(musicas[1]);
                if (currentMusica != null && currentMusica != musicas[1])
                {
                    TrocarMusicaPara(musicas[1]);
                }
                break;
            default:
                switch (sceneName)
                {
                    case "Boss 1":
                        if (currentMusica == null) TrocarMusicaPara(musicas[2]);
                        if (currentMusica != null && currentMusica != musicas[2])
                        {
                            TrocarMusicaPara(musicas[2]);
                        }
                        break;
                }
                break;
        }

    }

    void TrocarMusicaPara(AudioSource _musica)
    {
        foreach (AudioSource musica in musicas)
        {
            if (musica == _musica)
            {
                musica.Play();
                currentMusica = musica;
            }
            else
            {
                musica.Stop();
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
