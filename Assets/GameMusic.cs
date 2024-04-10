using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
    private static GameMusicManager _instance;

    public static GameMusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameMusicManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
