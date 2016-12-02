using UnityEngine;
using System.Collections;

public class scr_DestroyParticle : MonoBehaviour 
{
    
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    void OnCollisionEnter2D(Collision2D colli)
    {
        if(colli.gameObject.tag == "particle")
        {
            colli.transform.parent = GameObject.FindGameObjectWithTag("pooler").transform;
            colli.gameObject.SetActive(false);
        }
    }
}
