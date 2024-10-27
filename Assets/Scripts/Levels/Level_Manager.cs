using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{
    [SerializeField] GameObject[] LevelPrefabs;
    int currentLevel;
    Quest[] currentLevelQuests;
    int numQuestsCompleted;

    public float _minX, _maxX, _minY, _maxY; // level bounds for current level

    private void Awake()
    {
        GameMaster.Instance.levelManager = this;
        SpawnLevel(0);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnLevel(int level)
    {
        currentLevel = level;
        SetLevelBounds(LevelPrefabs[currentLevel].GetComponent<LevelData>().levelBounds);
        
        // Instantiate level
        GameObject levelInstance = Instantiate(LevelPrefabs[currentLevel], transform.position, transform.rotation);
        
        // Subscribe to quest completed events
        currentLevelQuests = levelInstance.GetComponents<Quest>();
        foreach(Quest quest in currentLevelQuests)
        {
            quest.OnQuestCompleted += LevelManager_OnQuestCompleted;
        }
    }
    void SetLevelBounds(float[] bounds)
    {
        _minX = bounds[0];
        _maxX = bounds[1];
        _minY = bounds[2];
        _maxY = bounds[3];
    }

    public bool IsPointInLevelBounds(Vector3 point)
    {
        return (point.y > _minY && point.y < _maxY && point.x > _minX && point.x < _maxX);
    }

    private void LevelManager_OnQuestCompleted(object sender, System.EventArgs e)
    {
        numQuestsCompleted++;

        if (numQuestsCompleted == currentLevelQuests.Length)
        {
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        SceneManager.LoadScene("StartMenuScene");
    }

}
