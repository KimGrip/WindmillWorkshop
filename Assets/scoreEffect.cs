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
    private Transform glassJarPosition;
    private Transform scorePosition;

    // Use this for initialization
    void Start ()
    {


        PS = GetComponent<ParticleSystem>();
        //PS.enableEmission = true;
        PS.Stop();


        inGameUI = GameObject.Find("Canvas").GetComponent<scr_IngameUI>();
        oldScore = inGameUI.GetScoreText();

        glassJarPosition = GameObject.FindGameObjectWithTag("win").GetComponent<Transform>();
        scorePosition = this.transform;

        scorePosition.transform.position = new Vector3(glassJarPosition.transform.position.x, glassJarPosition.transform.position.y, glassJarPosition.transform.position.z);
        //   Debug.Log(oldScore);
        //   Debug.Log(inGameUI.GetScoreText());

        //    scaleUpwards = true;
        //    transform.localScale = new Vector3(minMaxButtonScale.x, minMaxButtonScale.x, minMaxButtonScale.x);
    }

    void RunParticles()
    {
        if (PS.isPlaying)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(minMaxButtonScale.y, minMaxButtonScale.y, transform.localScale.z), buttonMorphSpeed);
        }
        else
        {
            PS.Play();
            timeToStop = 0;
            //PS.enableEmission = true;
            // Debug.Log("Particlesystem on");
            // ScaleMedal(transform);
        }
    }

    void CheckScore()
    {
        if (oldScore != inGameUI.GetScoreText())
        {
            RunParticles();
        //    Debug.Log(oldScore);
        //    Debug.Log("this is ingame score:" + inGameUI.GetScoreText());
            timeToStop = 0;
            //timeToStop = 0;
        }

    }

    // Update is called once per frame
    void Update ()
    {
        CheckScore();
        oldScore = inGameUI.GetScoreText();
        //ScaleMedal(transform);
      //  Debug.Log("checkScore run");

        if (!PS.isPlaying)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(minMaxButtonScale.x, minMaxButtonScale.x, transform.localScale.z), buttonMorphSpeed);
        }

        if (timeToStop > 0.3f)
        {
            PS.Stop();
            timeToStop = 0;
            //    Debug.Log("Particle system is off");
        }
        else
        {
            timeToStop = timeToStop + Time.deltaTime;
        }
        
        scorePosition.transform.position = new Vector3(glassJarPosition.transform.position.x, glassJarPosition.transform.position.y, glassJarPosition.transform.position.z);
//        glassJarPosition.position = new Vector3(Camera.main.transform.position.x + 10f, Camera.main.transform.position.y, transform.position.z);
    }
}