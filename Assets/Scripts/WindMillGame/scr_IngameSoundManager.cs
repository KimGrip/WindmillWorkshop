using UnityEngine;
using System.Collections;

public class scr_IngameSoundManager : MonoBehaviour
{
    private scr_BackgroundMusic MuiscManager;

    public AudioClip backGroundMusic;

    public AudioSource[] soundFXsources;

    public AudioClip[] bagHitSounds;
    public AudioClip[] collectParticles;

    public AudioClip[] cannonSounds;
    public AudioClip grainSounds;
    public AudioClip crowdCheer;
    [Space(20)]

    public AudioClip bagBreak;
    public AudioClip bagHit;
    public AudioClip bagShoot;
    [Space(20)]
    public AudioClip stageStart;
    public AudioClip stageEnd;
    [Space(20)]
    public AudioClip buttonClick;
    public AudioClip buttonDeclick;
    public AudioClip buttonHover;

    [Space(20)]
    public float lowPitchRange = 0.55f;
    public float highPitchRange = 1.55f;
    private float m_collectParticlePitch = 1.0f;

    private float m_Timer;
    private float m_CollectingTimer;
    private float m_cannonCollectingPitch;
    // Use this for initialization


    //AudioSource  5 = bagBreak, bagHit, bagShoot.
    //AudioSource  6 = stageStart, stageEnd.
    //AudioSource  7 = buttonclick,Declick,Hover


    void Awake()
    {
 
        MuiscManager = GameObject.Find("MusicManager").GetComponent<scr_BackgroundMusic>();
        MuiscManager.SetBackGroundMusic(backGroundMusic);
        m_collectParticlePitch = 1;
        m_cannonCollectingPitch = 1;
        //BGMusicSource.pitch = Random.Range(0.25f, 1.5f);

        for (int i = 0; i < soundFXsources.Length; i++)
        {
            soundFXsources[i] = gameObject.AddComponent<AudioSource>();
        }
    }
    public void PlayButtonClick()
    {
        soundFXsources[7].clip = buttonClick;
        soundFXsources[7].Play();
    }
    public void PlayButtonDeclick()
    {
        soundFXsources[7].clip = buttonDeclick;
        soundFXsources[7].Play();
    }
    public void PlayButtonHover()
    {
        soundFXsources[7].clip = buttonHover;
        soundFXsources[7].Play();
    }
    public void PlayStageStart()
    {
        soundFXsources[6].clip = stageStart;
        soundFXsources[6].Play();
    }
    public void PlayStageEnd()
    {
        soundFXsources[6].clip = stageEnd;
        soundFXsources[6].Play();
    }
    public void PlayBagShootSound()
    {
        soundFXsources[5].clip = bagShoot;
        soundFXsources[5].Play();
    }
    public void PlayBagHitSound()
    {
        soundFXsources[5].clip = bagHit;
        soundFXsources[5].Play();
    }
    public void PlayBagBreak()
    {
        soundFXsources[5].clip = bagBreak;
        soundFXsources[5].Play();
    }
    public void PlayCannonSounds(int index, bool increasePitch)
    {
        if (increasePitch && index == 1)
        {
            m_cannonCollectingPitch += 0.01f;
            soundFXsources[2].pitch = m_cannonCollectingPitch;
            soundFXsources[2].volume = 0.3f;
            soundFXsources[2].clip = cannonSounds[index];
            soundFXsources[2].Play();
        }
        else
        {
            soundFXsources[3].pitch = 1;
            soundFXsources[3].volume = 1.3f;
            soundFXsources[3].clip = cannonSounds[index];
            soundFXsources[3].Play();
            m_cannonCollectingPitch = 1;

        }
    }

    //}
    //public void SetBGMusicState(bool playSound)
    //{
    //    if (playSound)
    //    {
    //        BGMusicSource.UnPause();
    //    }
    //    else if (!playSound)
    //    {
    //        BGMusicSource.Pause();
    //    }
    //}
    public bool IsSourceIsPlayingSound(int index)
    {

        if (soundFXsources[index].isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void PlayGraindSounds()
    {
        soundFXsources[4].clip = grainSounds;
        soundFXsources[4].volume = 0.5f;
        soundFXsources[4].Play();
    }
    public void PlayCrowCheerSound()
    {
          
    }
    public void PlayBagHitSounds()
    {
        RandomizeSfx(soundFXsources[0], true, bagHitSounds);
    }
    public void PlayCollectParticleSounds()
    {
        if (m_CollectingTimer > 0.10f)
        {
            RandomizeSfx(soundFXsources[1], false, collectParticles);
            soundFXsources[1].volume = 0.65f;
            m_Timer = 0;
            m_CollectingTimer = 0;
        }
        soundFXsources[1].pitch = m_collectParticlePitch;
        m_collectParticlePitch += 0.0025f;
    }
    public void RandomizeSfx(AudioSource source, bool useRandomPitch, params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        if (useRandomPitch)
        {
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            source.pitch = randomPitch;
        }

        source.clip = clips[randomIndex];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        m_Timer = m_Timer + Time.deltaTime;
        if (m_Timer > 2)
        {
            m_collectParticlePitch = 1.0f;
        }
        m_CollectingTimer = m_CollectingTimer + Time.deltaTime;
    }
}