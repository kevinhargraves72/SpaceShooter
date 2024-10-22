using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecondaryFire : MonoBehaviour
{
    //UI
    Image missleCoolDownUI;
    TMP_Text missleCountText;
    
    [SerializeField] Transform boxCastPoint;
    [SerializeField] GameObject misslePrefab;
    [SerializeField] GameObject missleSpawnPoints;
    [SerializeField] GameObject missleLaunchFire;
    [SerializeField] LayerMask enemyLayer;
    Transform[] launchPoints;
    LineRenderer lineRenderer;
    //RaycastStuff
    RaycastHit2D[] hits;
    List<Transform> targets;
    Vector2 boxSize = new Vector2(10, 1);

    PlayerStats playerStats;

    float cooldownTimer = 0;

    float Atk;
    //Targetingstuff
    bool targeting = false;
    bool visualManagerActive = false;
    Queue<IEnumerator> targetVisualQueue = new Queue<IEnumerator>();
    
    float missleCount;
    float shotDelay = 0.1f;

    private void Start()
    {
        playerStats = gameObject.GetComponent<Player>().GetPlayerStats();
        missleCount = playerStats.MaxMissleCount;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        missleCoolDownUI = gameObject.GetComponent<Player>().UIManager.UI_SecondaryFire.Find("UI_MissleCoolDown").GetComponent<Image>();
        missleCountText = gameObject.GetComponent<Player>().UIManager.UI_SecondaryFire.Find("UI_MissleCountText").GetComponent<TMP_Text>();
        targets = new List<Transform>();
    }

    void Update()
    {
        UpdateUI();
        if (cooldownTimer > 0 && targeting == false) //give the player missles every missleCoolDown seconds
        {
            
            cooldownTimer -= Time.deltaTime;
            
        }
        if(cooldownTimer <= 0 && missleCount < playerStats.MaxMissleCount)
        {
            missleCount++;
            if(missleCount < playerStats.MaxMissleCount)
                cooldownTimer = playerStats.MissleCoolDown;
        }

        if(targeting == true) //call targetMissle if ActivateLaunchers() sets targeting to true
        {
            TargetMissles();
        }
        
    }

    public void ActivateLaunchers(float atk)//Activated by PlayerShooting fire2 keyDown
    {
        if(missleCount > 0)  //If there are missles set attack, new target list and targeting bool                                           
        {//Might have to also check if targeting is false
            Atk = atk * playerStats.AtkMultiplyer;
            targeting = true;
            targets = new List<Transform>();
        }
    }
    public void ReleaseTargeting()//Activates by PlayerShooting fire2 keyUp or by TargetMissles() if target.count == missleCount (there are as many targets as missles ready)
    {//If there are targets and targeting is true add cooldown for each missle used, launch missles then set missle count to 0
        if(targets.Count > 0 && targeting == true)
        {
            LaunchMissles(targets, missleCount);
            missleCount = 0;
            cooldownTimer = playerStats.MissleCoolDown;
            targeting = false;
        }
        else
        {
            targeting = false;
        }
        //Will always happen after an activates (only place)
    }

    void TargetMissles()//Activated in update when targeting is set to true by ActivateLanchers()/PlayerShooting.keydown 
    {//While tageting is true keep boxcasting and if you get a target that isnt in the list add it
     //If the list has as many targets as there are missles is breaks and targeting is set to false by ReleaseTargeting()
        hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, transform.up, enemyLayer);

        if(hits.Length > 0)//Only does stuff if a target is hit, if target.count never equals missles count release has to stop targeting
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (targets.Count < missleCount)
                {
                    if (!targets.Contains(hit.transform) && hit.collider.gameObject.name != gameObject.name)
                    {
                        targets.Add(hit.transform);
                        targetVisualQueue.Enqueue(DrawTargetVisual(hit.transform.position));
                        
                        //add to coroutine queue
                    }
                }
                else
                {
                    
                    break;
                }
            }
            if(!visualManagerActive &&  targets.Count > 0)
            {
                visualManagerActive = true;
                StartCoroutine(TargetVisualManager());
            }

            if (targets.Count == missleCount)
            {
                ReleaseTargeting();
            }

        }
        
    }

    IEnumerator TargetVisualManager()
    {
        while(visualManagerActive)
        {
            while (targetVisualQueue.Count > 0)
            {
                
                yield return StartCoroutine(targetVisualQueue.Dequeue());
            }
            visualManagerActive = false;
            yield return null;
        }
    }

    IEnumerator DrawTargetVisual(Vector2 target)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, boxCastPoint.position);
        lineRenderer.SetPosition(1, target);
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;

    }

    void LaunchMissles(List<Transform> Targets, float numMissles)
    {
        launchPoints = missleSpawnPoints.GetComponentsInChildren<Transform>();
        Debug.Log(launchPoints);
        int launchIndex = 1;
        int targetIndex = 0;
        shotDelay = 0.1f;
        while (numMissles > 0)
        {
            //check for bool if coRoutine is running, set bool back to false after wait.2seconds in coRoutin
            StartCoroutine(LaunchMissle(Targets[targetIndex], Targets, launchPoints[launchIndex]));
            numMissles--;
            launchIndex++;
            targetIndex++;
            if(launchIndex == launchPoints.Length)
            {
                launchIndex = 1;
            }
            if(targetIndex == Targets.Count)
            {
                targetIndex = 0;
            }
            shotDelay += 0.2f;
        }

    }
    void UpdateUI()
    {
        missleCoolDownUI.fillAmount = cooldownTimer / playerStats.MissleCoolDown;
        missleCountText.text = "x" + missleCount.ToString();
    }
    IEnumerator LaunchMissle(Transform startingTarget,  List<Transform> Targets, Transform launchPoint)
    {
        yield return new WaitForSeconds(shotDelay);//Wait .2 seconds launching missle
        GameObject launchFire = Instantiate(missleLaunchFire, launchPoint.position, launchPoint.rotation);
        launchFire.GetComponent<Animator>().SetTrigger("LaunchMissle");
        launchFire.GetComponent<MissleLaunchFire>().launchPoint = launchPoint;
        Destroy(launchFire, 0.5f);
        GameObject missle = Instantiate(misslePrefab, launchPoint.position, launchPoint.rotation);
        missle.GetComponent<Missle>().SetMissle(startingTarget, Targets);
        missle.GetComponent<Missle>().SetAtk(Atk);
    }

}
