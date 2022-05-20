using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    public PlayerMovement PlayerScript;
    [SerializeField] private GameObject Creature;
    private GameObject CreatureClone;
    private float timer = 0;
    private float playerX;
    private float playerZ;
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
        Debug.Log(timer);
        timer += Time.deltaTime;
        if (timer >= spawnTime -1)
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
        CreatureClone = Instantiate(Creature, new Vector3(PlayerScript.playerX + Random.Range(-30, 30), 2.46f, PlayerScript.playerZ + Random.Range(-30, 30)), PlayerScript.transform.rotation);
    }
    void SetRandomSpawnTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }
}