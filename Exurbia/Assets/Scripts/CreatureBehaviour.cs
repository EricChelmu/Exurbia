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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 10f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            CreatureClone = Instantiate(Creature, new Vector3(PlayerScript.playerX + Random.Range(5, 15), 2.46f, PlayerScript.playerZ + Random.Range(5, 15)), PlayerScript.transform.rotation);
        }
    }
}
