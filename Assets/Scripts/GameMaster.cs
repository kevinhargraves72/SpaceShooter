using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }
    
    [SerializeField] CameraFollow followCam;
    [SerializeField] float[] levelBounds;

    public PlayerData playerData { get; private set; }
    public GameObject playerInstance;
    [NonSerialized] public int lives;

    public UI_Manager UIManager;

    float respawnTimer;
    [SerializeField] GameObject playerPrefab;


    private void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        DontDestroyOnLoad(gameObject);
        playerData = new PlayerData();
        lives = playerData.numLives;

    }
    void Start()
    {
        SpawnPlayer();
        SetLevelBounds(levelBounds);
        followCam.myTarget = playerInstance.transform;
    }
    void Update()
    {
        if (playerInstance == null && lives > 0)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                SpawnPlayer();
            }
        }
        
    }
    void SpawnPlayer()
    {
        lives--;
        UIManager.UpdateLivesUI();
        respawnTimer = 1;
        playerInstance = Instantiate(playerPrefab, transform.position, transform.rotation);
    }
    
    void SetLevelBounds(float[] bounds)
    {
        followCam.setBounds(bounds[0], bounds[1], bounds[2], bounds[3]);
    }
}
