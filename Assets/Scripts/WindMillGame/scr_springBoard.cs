using UnityEngine;
using System.Collections;

public class scr_springBoard : MonoBehaviour 
{

    private float startScale;
    public float maxScaleOffset;
    private bool scaleUpwards;
    public float scaleSpeed;
    private bool morph;
    public float bounceSpeed;
	// Use this for initialization
	void Start () 
    {
        startScale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(morph)
        {
            MorphSpring();
        }
	}
    void MorphSpring()
    {
        if (!scaleUpwards)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(transform.localScale.x, maxScaleOffset, transform.localScale.z), scaleSpeed);
            if (transform.localScale.y <= maxScaleOffset)
            {
                scaleUpwards = true;
            }
        }
        else if (scaleUpwards)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(startScale, startScale, transform.localScale.z), scaleSpeed);
            if (transform.localScale.y >= startScale)
            {
                morph = false;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D colli)
    {
        if(colli.gameObject.tag == "bag")
        {
            morph = true;
            scaleUpwards = false;
            Rigidbody2D rb = colli.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = transform.up * bounceSpeed ;

        }
    }
}
