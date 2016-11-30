using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scr_cannonBehaviour : MonoBehaviour
{
    private scr_obp pooler;
    private Transform cannonAim;
    public int collectedParticles;
    private scr_IngameSoundManager ISM;

    private List<GameObject> collectedGameObjects;
   
    public int neededShootAmount;
    public float cannonPower;
    public float cannonShootCooldown;
    private float m_cannonCooldownCounter;
    private bool shootBag;
    
	// Use this for initialization
	void Start () 
    {
        ISM = GameObject.Find("GameManager").GetComponent<scr_IngameSoundManager>() ;
        collectedGameObjects = new List<GameObject>();
        pooler = GameObject.FindGameObjectWithTag("pooler").GetComponent<scr_obp>();
        cannonAim = transform.GetChild(0);
	}
	void Update () 
    {
        m_cannonCooldownCounter += Time.deltaTime;
        if (m_cannonCooldownCounter > cannonShootCooldown)
        {
            if (collectedParticles >= neededShootAmount || shootBag)
            {
                FireCannon();
                ISM.PlayCannonSounds(0, false);
            }
            m_cannonCooldownCounter = 0;
        }
	}
    
    Vector2 ParticleSpawnVel(Vector2 targetPos, Vector2 selfPos)
    {
        Vector2 direction;
        direction = targetPos - selfPos;
        Vector2 force = direction * cannonPower;
        force = force * Random.Range(0.5f, 1.5f);
        return force;
    }
    Vector2 ParticleSpawnPosition(Vector2 targetPos, Vector2 selfPos)
    {
        Vector2 direction;
        Vector2 position;
        direction = targetPos - selfPos; //direction towards CannonAim
        position = new Vector2(transform.position.x + direction.x, transform.position.y + direction.y);
        return position;
    }
    void FireCannon() //needs to shoot them dynamically
    {
       for(int i =0 ; i < collectedGameObjects.Count; i++)
       {
           if(collectedGameObjects[i].name == "p1")
           {
               GameObject obj = pooler.GetGameObjectFromType(GameObjectType.P1);
               obj.SetActive(true);
               obj.transform.position = ParticleSpawnPosition(cannonAim.position, transform.position);
               obj.GetComponent<Rigidbody2D>().velocity = ParticleSpawnVel(cannonAim.position, transform.position);
           }
           if (collectedGameObjects[i].name == "p2")
           {
               GameObject obj = pooler.GetGameObjectFromType(GameObjectType.P2);
               obj.SetActive(true);
               obj.transform.position = ParticleSpawnPosition(cannonAim.position, transform.position);
               obj.GetComponent<Rigidbody2D>().velocity = ParticleSpawnVel(cannonAim.position, transform.position);
           }
           if (collectedGameObjects[i].name == "p3")
           {
               GameObject obj = pooler.GetGameObjectFromType(GameObjectType.P3);
               obj.SetActive(true);
               obj.transform.position = ParticleSpawnPosition(cannonAim.position, transform.position);
               obj.GetComponent<Rigidbody2D>().velocity = ParticleSpawnVel(cannonAim.position, transform.position);
           }
           if(collectedGameObjects[i].tag == "bag")
           {
               collectedGameObjects[i].transform.position = ParticleSpawnPosition(cannonAim.position, transform.position);
               collectedGameObjects[i].GetComponent<Rigidbody2D>().velocity = ParticleSpawnVel(cannonAim.position, transform.position);
           }
       }
       collectedGameObjects.Clear();
       collectedParticles = 0;
    }

    void OnCollisionEnter2D(Collision2D colli)
    {
        if (colli.gameObject.tag == "particle" )
        {
            collectedGameObjects.Add(colli.gameObject);
            colli.gameObject.SetActive(false);
            collectedParticles += 1;
            ISM.PlayCannonSounds(1, true);
        }
        else if( colli.gameObject.tag == "bag")
        {
            collectedGameObjects.Add(colli.gameObject);
            ISM.PlayCannonSounds(1, true);
            shootBag = true;
        }
    }
}