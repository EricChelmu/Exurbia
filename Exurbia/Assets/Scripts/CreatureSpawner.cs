using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public PlayerMovement PlayerScript;
    [SerializeField] private GameObject Creature;
    [SerializeField] private GameObject Player;
    private GameObject CreatureClone;
    private float timer = 0;
    private float safeTimer = 0;
    private float minTime = 5;
    private float maxTime = 10;
    private float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        SetRandomSpawnTime();
        timer = minTime;
    }

    // Update is called once per frame
    void Update()
    {
        //15 second window when game starts before creature spawns
        safeTimer += Time.deltaTime;
        //If the 15 seconds pass, creature spawns
        if (safeTimer >= 15)
        {
            //Count the time in seconds
            timer += Time.deltaTime;
            //Delete the creature before a new one is spawned
            if (timer >= spawnTime - 0.1)
            {
                Destroy(CreatureClone);
            }
            //If the time is bigger than the spawn time(random number between 5-10 seconds) spawn creature
            if (timer >= spawnTime)
            {
                SpawnCreature();
                SetRandomSpawnTime();
            }
        }
        
    }
    void SpawnCreature()
    {
        timer = 0;
        //Spawn creature based on player rotation
        CreatureClone = Instantiate(Creature, new Vector3(PlayerScript.playerX + Random.Range(-15, 15), PlayerScript.transform.position.y + 3, PlayerScript.playerZ + Random.Range(-15, 15)), PlayerScript.transform.rotation);
    }
    void SetRandomSpawnTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }
}
