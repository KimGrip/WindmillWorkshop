using UnityEngine;
using System.Collections;




public class medalEffects : MonoBehaviour
{

    private ParticleSystem PS;
    private bool scaleUpwards;
    public Vector3 minMaxButtonScale;
    public float buttonMorphSpeed;
    public float startBeforeDoneTime;
    private scr_CameraScript CS;
    private Camera mainCam;

    // Use this for initialization
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
        PS.Stop();
        scaleUpwards = true;
        transform.localScale = new Vector3(minMaxButtonScale.x, minMaxButtonScale.x, minMaxButtonScale.x);
        CS = Camera.main.GetComponent<scr_CameraScript>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }


    //void ScaleMedal(Transform obj)
    //{
    //    if (scaleUpwards)
    //    {
    //        obj.localScale = Vector3.MoveTowards(obj.localScale, new Vector3(minMaxButtonScale.y, minMaxButtonScale.y, obj.localScale.y), buttonMorphSpeed);
    //        Debug.Log("scaleUpwards biggest size");
    //        if (obj.localScale.y >= minMaxButtonScale.y)
    //            {
    //                scaleUpwards = false;
    //            }
    //        else if (obj.localScale.y >= minMaxButtonScale.y - startBeforeDoneTime)
    //            {
    //                PS.Play();
    //                Debug.Log("Particlesystem on");
    //            }

    //    }
    //    else if (obj.localScale.y >= minMaxButtonScale.y)
    //    {
    //        obj.localScale = Vector3.MoveTowards(obj.localScale, new Vector3(minMaxButtonScale.z, minMaxButtonScale.z, obj.localScale.z), buttonMorphSpeed);
    //        Debug.Log("scaleUpwards back to normal");
    //    }
    //}

    void ScaleMedalTwo()
    {
        if (scaleUpwards)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(minMaxButtonScale.y, minMaxButtonScale.y, transform.localScale.y), buttonMorphSpeed);
            Debug.Log("ScaleUp");
            if (transform.localScale.y >= minMaxButtonScale.y)
            {
                scaleUpwards = false;
                CS.ShakeCamera(0.5f, 0.2f);

            }
            else if (transform.localScale.y >= minMaxButtonScale.y - startBeforeDoneTime)
            {
                PS.Play();

            }
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(minMaxButtonScale.z, minMaxButtonScale.z, transform.localScale.z), buttonMorphSpeed);
            Debug.Log("ScaleDown");
            PS.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ScaleMedal(transform);
        ScaleMedalTwo();

    }



}
