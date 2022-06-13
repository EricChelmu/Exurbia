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
        timer += Time.deltaTime;
        if (timer >= spawnTime - 0.1)
        {
            Destroy(CreatureClone);
        }
        if (timer >= spawnTime)
        {
            SpawnCreature();
            SetRandomSpawnTime();
        }
    }
    void SpawnCreature()
    {
        timer = 0;
        CreatureClone = Instantiate(Creature, new Vector3(PlayerScript.playerX + Random.Range(-20, 20), PlayerScript.transform.position.y + 3, PlayerScript.playerZ + Random.Range(-20, 20)), PlayerScript.transform.rotation);
    }
    void SetRandomSpawnTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }
}
