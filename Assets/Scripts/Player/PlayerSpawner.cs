using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] CameraFollow followCam;
    public UI_Manager UIManager;
    [SerializeField] float[] levelBounds; //-35, 35, -25, 25

    public GameObject playerInstance;

    float respawnTimer;
    void Start()
    {
        SpawnPlayer();
        SetLevelBounds(levelBounds);
    }

    void Update()
    {
        if(playerInstance == null && GameMaster.Instance.lives > 0)
        {
            respawnTimer -= Time.deltaTime;
            if(respawnTimer <= 0)
            {
                SpawnPlayer();
            }

        }
    }

    void SpawnPlayer()
    {
        GameMaster.Instance.lives--;
        UIManager.UpdateLivesUI();
        respawnTimer = 1;
        playerInstance = Instantiate(playerPrefab, transform.position, transform.rotation);
        playerInstance.GetComponent<Player>().SetUI_Manager(UIManager);
        GameMaster.Instance.playerInstance = playerInstance;
    }

    void SetLevelBounds(float[] bounds)
    {
        followCam.setBounds(bounds[0], bounds[1], bounds[2], bounds[3]);
    }

}
