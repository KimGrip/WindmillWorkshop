using UnityEngine;
using System.Collections;

public class scr_DestructibleBlock : MonoBehaviour 
{
    public int HP;
    public int crumbledAmount;
	void Start () 
    {
	
	}
	// Update is called once per frame
	void Update () 
    {
	    if(HP <= 0)
        {
            SpawnParticles();
            DestroyBlock();
        }
	}
    void SpawnParticles()
    {
        for(int i = 0; i < crumbledAmount; i++)
        {
           GameObject obj = scr_obp.current.GetGameObjectFromType(GameObjectType.P3);
           obj.transform.position = transform.position;
           obj.SetActive(true);
        }
    }
    void DestroyBlock()
    {
        gameObject.SetActive(false);
    }
    void OnCollisionEnter2D(Collision2D colli)
    {
        if(colli.gameObject.tag =="bag")
        {
            DestroyBlock();
        }
        else if(colli.gameObject.tag == "particle")
        {
            HP -= 1;
        }
    }
}
