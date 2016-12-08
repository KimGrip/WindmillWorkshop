using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameObjectType
{
    P1, P2, P3, none
};
public class scr_obp : MonoBehaviour
{
    static public scr_obp current;
    private scr_IngameSoundManager ISM;

    public GameObject particle_1;
    List<GameObject> l_particle_1 = new List<GameObject>();
    public int m_particle_1_storage;

    public GameObject particle_2;
    List<GameObject> l_particle_2 = new List<GameObject>();
    public int m_particle_2_storage;

    public GameObject particle_3;
    List<GameObject> l_particle_3 = new List<GameObject>();
    public int m_particle_3_storage;

    public List<GameObject> l_activeParticles = new List<GameObject>();
    private GameObject lowestYParticle;

    private float m_timer;
    [Space(20)]
    public List<Sprite> m_GrainSprites;
    private float particleMass;
    void Awake()
    {
        current = this;
        PutIntoList(m_particle_1_storage, l_particle_1, particle_1, "p1");
        PutIntoList(m_particle_2_storage, l_particle_2, particle_2, "p2");
        PutIntoList(m_particle_3_storage, l_particle_3, particle_3, "p3");
        ISM = GameObject.Find("GameManager").GetComponent<scr_IngameSoundManager>();
    }
    void Update()
    {
        //function, check Type in list, if not correct item, check next list
        // check = name or int type of object
        // switch 
        // sCANT HAVE THIS FUCKING HERE., NO NONONONONON
        // 50% cpu power.
        if(m_timer > 1)
        {
            if (CheckForGrainSound() && !ISM.IsSourceIsPlayingSound(4))
            {
                ISM.PlayGraindSounds();
                m_timer = 0;
            }
        }
        m_timer = m_timer + Time.deltaTime;
    }
    bool CheckForGrainSound()
    {
        int neededForGrainSound = 0;
        for(int i = 0; i < l_particle_1.Count; i++)
        {
            if(l_particle_1[i].activeInHierarchy == true)
            {
                neededForGrainSound += 1;
            }
        }
        for (int i = 0; i < l_particle_2.Count; i++)
        {
            if (l_particle_2[i].activeInHierarchy == true)
            {
                neededForGrainSound += 1;
            }
        }
        if(neededForGrainSound > 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Rigidbody2D GetParticleRB(GameObject obj)
    {
        return obj.GetComponent<Rigidbody2D>();
    }
    void PutIntoList(int storageSpace, List<GameObject> objectList, GameObject gameobject, string name)
    {
        for (int i = 0; i < storageSpace; i++)
        {
            GameObject m_object = Instantiate(gameobject);
            m_object.transform.parent = transform;
            m_object.SetActive(false);
            m_object.name = name;
            objectList.Add(m_object);
        }
    }
    GameObject GetGameObjectFromList(List<GameObject> m_list, bool GetActiveItems )
    {
        for (int i = 0; i < m_list.Count; i++)
        {
            if (m_list[i].activeInHierarchy == GetActiveItems)
            {
                GameObject obj = m_list[i];
                return obj;
            }
        }
        return null;
    }
    public void SetParticleMass(float mass)
    {
        particleMass = mass;
    }
   public GameObject GetGameObjectFromType(GameObjectType type)
    {
        if(type == GameObjectType.P1)
        {
            GameObject obj = GetGameObjectFromList(l_particle_1, false);
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.mass = particleMass;
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.sprite = m_GrainSprites[Random.Range(0, m_GrainSprites.Count)];

            return obj;
        }
        if (type == GameObjectType.P2)
        {
            return GetGameObjectFromList(l_particle_2, false);
        }
        if (type == GameObjectType.P3)
        {
            return GetGameObjectFromList(l_particle_3, false);
        }
        return null;
    }
    bool CompareParticleVelocity(Vector2 velocity, float maxValue)
    {
        if(velocity.x > maxValue)
        {
            return true;
        }
        else if(velocity.y > maxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public List<GameObject> GetIdleActiveParticles()
    {
        l_activeParticles.Clear();
        for (int i = 0; i < l_particle_1.Count; i++ )
        {
            if(l_particle_1[i].activeInHierarchy == true && CompareParticleVelocity(l_particle_1[i].GetComponent<Rigidbody2D>().velocity, 0.2f))
            {
                l_activeParticles.Add(l_particle_1[i]);
            }
        }
        for (int y = 0; y < l_particle_2.Count; y++)
        {
            if (l_particle_2[y].activeInHierarchy == true && CompareParticleVelocity(l_particle_2[y].GetComponent<Rigidbody2D>().velocity, 0.2f))
            {
                l_activeParticles.Add(l_particle_2[y]);
            }
        }
        return l_activeParticles; 
    }
    public float GetLowestActiveYParticle()
    {
        lowestYParticle = l_particle_1[0];
        for (int i = 0; i < l_particle_1.Count; i++)
        {
            if (l_particle_1[i].activeInHierarchy == true && l_particle_1[i].transform.position.y < lowestYParticle.transform.position.y)
            {
                lowestYParticle = l_particle_1[i];
            }
        }
        for (int y = 0; y < l_particle_2.Count; y++)
        {
            if (l_particle_2[y].activeInHierarchy == true && l_particle_2[y].transform.position.y < lowestYParticle.transform.position.y)
            {
                lowestYParticle = l_particle_2[y];
            }
        }
        return lowestYParticle.transform.position.y;
    }
}