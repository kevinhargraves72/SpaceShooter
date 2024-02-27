using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }
    public PlayerData playerData { get; private set; }
    private int lives;
    [SerializeField] TMP_Text UI_lifeCount;
    [SerializeField] Image healthBar;
    [SerializeField] UI_SkillTree skillTree;

    float respawnTimer;
    [SerializeField] GameObject playerPrefab;
    private GameObject playerInstance;

    private void Awake()
    {
        if(Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        DontDestroyOnLoad(gameObject);
        playerData = new PlayerData();
        lives = playerData.numLives;
        
    }
    void Start()
    {
        SpawnPlayer();
        UI_lifeCount.text = "Lives:"+ lives.ToString();
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
        if(playerInstance != null)
        {
            healthBar.fillAmount = playerInstance.GetComponent<DamageHandler>().GetHealthNormalized();
        }
        else
        {
            healthBar.fillAmount = 0;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            skillTree.TogglePause();
        }
    }
    void SpawnPlayer()
    {
        lives--;
        UI_lifeCount.text = "Lives:" + lives.ToString();
        respawnTimer = 1;
        playerInstance = Instantiate(playerPrefab, transform.position, transform.rotation);
    }
}
