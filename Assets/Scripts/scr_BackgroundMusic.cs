using UnityEngine;
using System.Collections;

public class scr_BackgroundMusic : MonoBehaviour 
{
    public AudioSource Source;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
    static bool AudioBegin = false;
    void Awake()
    {

        if (!AudioBegin)
        {
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
            Source.Play();
        }
    }
    public void SetBackGroundMusic(AudioClip clip)
    {
        Source.clip = clip;
    }
    void Update()
    {
        if(!Source.isPlaying)
        {
            Source.Play();
        }
    }
}
