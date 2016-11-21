using UnityEngine;
using System.Collections;

public class scr_particleHatchet : MonoBehaviour {
    private Rigidbody2D m_rb;
    public int breakAmount;
    private int m_particlesPresent;
	// Use this for initialization
	void Start () 
    {
        m_rb = this.GetComponent<Rigidbody2D>();
	}
	// Update is called once per frame
	void Update () 
    {
	    if(breakAmount <= m_particlesPresent )
        {
            m_rb.isKinematic = false;
        }
	}
    void OnCollisionEnter2D(Collision2D colli)
    {
        if(colli.gameObject.tag == "particle")
        {
            m_particlesPresent += 1;
        }
    }
    void OnCollisionExit2D(Collision2D colli)
    {
        if (colli.gameObject.tag == "particle")
        {
            m_particlesPresent -= 1;
        }
    }
}
