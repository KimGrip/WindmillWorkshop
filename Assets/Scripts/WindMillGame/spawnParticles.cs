using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum ParticleType
{
    normal, big, gunpowder
};
public class spawnParticles : MonoBehaviour 
{
    private scr_obp objectPooler;
    private scr_GameManager GM;
    private scr_bagMovement BM;
    private scr_IngameSoundManager ISM;
    public float distance;
    private int particleAmount;

    private GameObject particle;
    [Range(0.0f, 30.0f)]
    public float particleSpawnForce;
    public List<int> l_ParticlesPerBounce;
    public bool EnableRightClickSpawning;

    [Space(20)]
    public bool EnableLeftClickExplosion;
    public int bagExplosionFrameAmount;
    private bool m_exploding;
    private int m_frameCounter;
    private int index;
	void Awake () 
    {
        BM = this.GetComponent<scr_bagMovement>();
        m_frameCounter = 1;
        GM = GameObject.Find("GameManager").GetComponent<scr_GameManager>();
        ISM = GM.GetComponent<scr_IngameSoundManager>();

        particleAmount = GM.GetMaxWinParticles();
        objectPooler = GameObject.FindGameObjectWithTag("pooler").GetComponent<scr_obp>();
        m_exploding = false;
	}
	void Update () 
    {
        if(EnableRightClickSpawning)
        {
            SpawnParticlesFromBag();
        }
        if (Input.GetMouseButtonDown(0) && BM.GetThrowBagStatus() && !BM.GetExtraThrow())
        {
            m_exploding = true;
        }
        else if (Input.GetMouseButtonDown(0) && !BM.GetPresence() && BM.GetThrowBagStatus())
        {
            m_exploding = true;
        }
	}
    void FixedUpdate()
    {
        if (m_exploding)
        {
            ExplodeBag();
        }
    }
    public void ExplodeBag()
    {
        ISM.PlayBagBreak();
     //   int maxWinAmount = GM.GetMaxWinParticles() * BM.GetRemainingBounces() / BM.GetMaxBounces();
        int index = BM.GetMaxBounces() - BM.GetRemainingBounces();
        int maxWinAmount = GM.GetMaxWinParticles();
        for(int i = 0; i < index; i++)
        {
            maxWinAmount -=  l_ParticlesPerBounce[i];
        }

        int loopAmount = maxWinAmount / bagExplosionFrameAmount;
        SpawnParticlesAroundBag(loopAmount);

        if (m_frameCounter == bagExplosionFrameAmount)
        {
            BM.DestroyBag();
        }
        m_frameCounter += 1;
    }
    void SpawnParticlesAroundBag(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = RandomCircle(this.transform.position, 0.5f);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, this.transform.position - pos);
            GameObject obj = objectPooler.GetComponent<scr_obp>().GetGameObjectFromType(GameObjectType.P1);

            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            obj.transform.localScale = transform.localScale;
            obj.SetActive(true);

            Rigidbody2D rb = objectPooler.GetParticleRB(obj); //causes lagg, needs predeclared pointer somehow
            rb.velocity = ParticleSpawnVel(this.transform.position, obj.transform.position, transform.GetComponent<Rigidbody2D>().velocity);
        }
    }
    void SpawnParticlesFromBag()
    {
        if (Input.GetMouseButton(1))
        {
            GameObject obj = objectPooler.GetComponent<scr_obp>().GetGameObjectFromType(GameObjectType.P1);
            obj.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
          //  obj.transform.localScale = new Vector3(1, 1, 1);
            obj.SetActive(true);
        }
    }
    public int GetparticleAmount()
    {
        return particleAmount;
    }
    public void SpawnParticles(ParticleType type)
    {
        particleAmount = particleAmount - l_ParticlesPerBounce[index];
        for (int i = 0; i < l_ParticlesPerBounce[index]; i++)
        {
            Vector3 pos = RandomCircle(this.transform.position, 0.5f);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, this.transform.position - pos);
            GameObject obj = objectPooler.GetComponent<scr_obp>().GetGameObjectFromType(GameObjectType.P1);

            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            obj.SetActive(true);

            Rigidbody2D rb = objectPooler.GetParticleRB(obj); //causes lagg, needs predeclared pointer somehow
            rb.velocity = ParticleSpawnVel(this.transform.position, obj.transform.position, transform.GetComponent<Rigidbody2D>().velocity);
        }
        index++;

    }
    Vector2 ParticleSpawnVel(Vector2 targetPos, Vector2 selfPos, Vector2 extraVel)
    {
        Vector2 direction;
        direction = selfPos - targetPos;
        Vector2 force = direction * particleSpawnForce;
        force = force * Random.Range(0.5f, 1.5f)+ extraVel;
        return force;
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
}
