using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scoreEffect : MonoBehaviour {

    private ParticleSystem PS;
    private bool scaleUpwards;
    public Vector2 minMaxButtonScale;
    public float buttonMorphSpeed;
    public float startBeforeDoneTime;
    private scr_IngameUI inGameUI;
    private int oldScore;
    private float timeToStop;
    
    // Use this for initialization
    void Start ()
    {


        PS = GetComponent<ParticleSystem>();
        //PS.enableEmission = true;
        PS.Stop();


        inGameUI = GameObject.Find("Canvas").GetComponent<scr_IngameUI>();
        oldScore = inGameUI.GetScoreText();
        Debug.Log(oldScore);
        Debug.Log(inGameUI.GetScoreText());

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

               
                
                //PS.enableEmission = true;
                
                //PS.Play();
                
            }

        else if(!scaleUpwards)
            {
                obj.localScale = Vector3.MoveTowards(obj.localScale, new Vector3(minMaxButtonScale.y, minMaxButtonScale.y, obj.localScale.z), buttonMorphSpeed);
                if (obj.localScale.y >= minMaxButtonScale.y)
                {
                    scaleUpwards = true;

                }
            }
        }
    }

    void RunParticles()
    {
        if (PS.isPlaying)
        {
            //if (timeToStop > 200)
            //{
            //    PS.Stop();
            //    timeToStop = 0;
            //    Debug.Log("Particle system is off");
            //}
            //else
            //{
            //    timeToStop = timeToStop + 1;
            //}
        }
        else
        {
            PS.Play();
            timeToStop = 0;
            //PS.enableEmission = true;
            Debug.Log("Particlesystem on");
        }
    }

    void CheckScore()
    {
        if (oldScore != inGameUI.GetScoreText())
        {
            RunParticles();
            ScaleMedal(transform);
            Debug.Log(oldScore);
            Debug.Log("this is ingame score:" + inGameUI.GetScoreText());
            timeToStop = 0;
            //timeToStop = 0;
        }
        //else if (timeToStop < 22)
        //{
        //    timeToStop = timeToStop + 1;
        //    RunParticles();
        //}
    }

    // Update is called once per frame
    void Update ()
    {
        CheckScore();
        oldScore = inGameUI.GetScoreText();
        //ScaleMedal(transform);
        Debug.Log("checkScore run");

        if (timeToStop >    0.5f)
        {
            
            PS.Stop();
            timeToStop = 0;
            Debug.Log("Particle system is off");
        }
        else
        {
            timeToStop = timeToStop + Time.deltaTime;
        }
    }
}