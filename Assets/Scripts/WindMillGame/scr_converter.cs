using UnityEngine;
using System.Collections;

public class scr_converter : MonoBehaviour
{
    private GameObject objPooler;
    private GameObject InputCheck;


    public GameObject InputObject;
    public float convertionRate;  // how many is required for 1 output;
    public GameObject OutputObject;
    private float storedObjects;

    private float spawnMultiplier;

    public float convertionTimePerParticle;
    private float m_convertionTimer;
    private ParticleSystem PS;
    private float stopPSTimer = 1.0f;

	// Use this for initialization
	void Start () 
    {
        objPooler = GameObject.FindGameObjectWithTag("pooler");
        PS = GetComponent<ParticleSystem>();
        PS.Stop();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(m_convertionTimer >= convertionTimePerParticle)
        {
            if (storedObjects >= convertionRate)
            {
                spawnMultiplier = storedObjects / convertionRate;
                if(spawnMultiplier >= 1.0f / convertionRate)
                {
                    spawnMultiplier = 1.0f / convertionRate;
                    PS.Play();
                }
                SpawnConvertedItems();
            }
            m_convertionTimer = 0;
        }
        m_convertionTimer = m_convertionTimer + Time.deltaTime;

        if (PS.isPlaying)
        {
            if (stopPSTimer >= 0.5f)
            {
                PS.Stop();
                stopPSTimer = 0f;
            }
            stopPSTimer = stopPSTimer + Time.deltaTime;
        }
        
	}

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
    void SpawnConvertedItems()  //Needs to be dynamic
    {
        for (int i = 0; i < spawnMultiplier; i++)
        {
            Vector3 pos = new Vector3((this.transform.position.x - 0.4f )+ 0.1f * i, this.transform.position.y - 0.8f * transform.lossyScale.y, 0);
            if (OutputObject.name == objPooler.GetComponent<scr_obp>().GetGameObjectFromType(GameObjectType.P1).name)
            {
                GameObject obj = objPooler.GetComponent<scr_obp>().GetGameObjectFromType(GameObjectType.P1);
                obj.transform.position = pos;
                obj.SetActive(true);
            }
            if (OutputObject.name == objPooler.GetComponent<scr_obp>().GetGameObjectFromType(GameObjectType.P2).name)
            {
                GameObject obj = objPooler.GetComponent<scr_obp>().GetGameObjectFromType(GameObjectType.P2);
                obj.transform.position = pos;
                obj.SetActive(true);
            }
            storedObjects -= 1;
        }
    }

    void OnCollisionEnter2D(Collision2D colli)
    {
        if (colli.gameObject.name == InputObject.name)
        {
            colli.gameObject.SetActive(false);
            storedObjects++;

        }
        if (colli.gameObject.tag == "particle" && colli.gameObject.name != "p2")
        {
            colli.gameObject.SetActive(false);

        }
        if (colli.gameObject.tag == "particle" && colli.gameObject.name == "p2")
        {
            colli.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f * transform.lossyScale.y, colli.gameObject.transform.position.z);
        }
    }
}