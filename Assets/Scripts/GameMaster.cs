using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }
    public PlayerData playerData { get; private set; }
    public GameObject playerInstance;
    private int lives;
    [SerializeField] TMP_Text UI_lifeCount;
    [SerializeField] Image healthBar;
    [SerializeField] Image sheildBar;
    [SerializeField] UI_SkillTree skillTree;
    public Image UI_EnergySteal;
    public GameObject UI_ActivateEnergySteal;
    public Transform UI_PrimaryFire;
    public Transform UI_SecondaryFire;
    public Transform UI_Shield;
    public GameObject UI_ActivateSecondaryFire;

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
        UI_lifeCount.text = "Lives:" + lives.ToString();
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
        
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            skillTree.TogglePause();
        }
    }

    void UpdateUI()
    {
        if (playerInstance != null)
        {
            healthBar.fillAmount = playerInstance.GetComponent<DamageHandler>().GetHealthNormalized();
            if(UI_Shield.gameObject.activeSelf)
            {
                sheildBar.fillAmount = playerInstance.GetComponent<DamageHandler>().GetShieldNormalized();
            }
        }
        else
        {
            healthBar.fillAmount = 0;
            if (UI_Shield.gameObject.activeSelf)
            {
                sheildBar.fillAmount = 0;
            }
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
