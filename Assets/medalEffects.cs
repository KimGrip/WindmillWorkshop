using UnityEngine;
using System.Collections;




public class medalEffects : MonoBehaviour {

    private ParticleSystem PS;
    private bool scaleUpwards;
    public Vector2 minMaxButtonScale;
    public float buttonMorphSpeed;
    public float startBeforeDoneTime;

    // Use this for initialization
    void Start ()
    {
        PS = GetComponent<ParticleSystem>();
        PS.enableEmission = false;
        //PS.Stop();
        scaleUpwards = true;

        transform.localScale = new Vector3(minMaxButtonScale.x, minMaxButtonScale.x, minMaxButtonScale.x);
    }


    void ScaleMedal(Transform obj)
    {
       if (scaleUpwards)
        {
            obj.localScale = Vector3.MoveTowards(obj.localScale, new Vector3(minMaxButtonScale.y, minMaxButtonScale.y, obj.localScale.z), buttonMorphSpeed);
            if (obj.localScale.y >= minMaxButtonScale.y)
            {
                scaleUpwards = false;
               
            }
            if (obj.localScale.y >= minMaxButtonScale.y - startBeforeDoneTime)
            {
                PS.enableEmission = true;
                //PS.Play();
                Debug.Log("Particlesystem on");
            }
          
        }
    }

   

    // Update is called once per frame
    void Update ()
    {
	    ScaleMedal(transform);

     
    }

        

}
