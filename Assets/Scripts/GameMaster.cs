using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }
    
    public PlayerData playerData { get; private set; }
    public GameObject playerInstance;
    [NonSerialized] public int lives;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        DontDestroyOnLoad(gameObject);
        playerData = new PlayerData();
        lives = playerData.numLives;

    }

}
