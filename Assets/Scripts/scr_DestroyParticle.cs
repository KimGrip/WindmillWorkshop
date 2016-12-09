using UnityEngine;
using System.Collections;

public class scr_DestroyParticle : MonoBehaviour 
{
    private ParticleSystem PS;
    private Transform emitPosition;
    
    // Use this for initialization
	void Start () 
    {
        emitPosition = transform.GetChild(0);
        Debug.Log("emitPos; " + emitPosition);
        PS = emitPosition.GetComponent<ParticleSystem>();
        Debug.Log("PS: " + PS);
        PS.Stop();
        //emitPosition = (GameObject)Instantiate(emitPosition, transform); 
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    void OnCollisionEnter2D(Collision2D colli)
    {
        if(colli.gameObject.tag == "particle")
        {
            emitPosition.transform.position = colli.gameObject.transform.position;
            PS.Play();
            colli.transform.parent = GameObject.FindGameObjectWithTag("pooler").transform;
            colli.gameObject.SetActive(false);
        }
        else
        {
            PS.Stop();
        }
    }
}
