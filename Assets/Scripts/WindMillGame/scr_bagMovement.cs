﻿using UnityEngine;
using System.Collections;
public enum BagState
{
    idle, flying, other
};
public class scr_bagMovement : MonoBehaviour
{
    private GameObject bag;
    private GameObject aimingArrow;
    private Rigidbody2D bagRB;
    private spawnParticles SP;
    private PhysicsMaterial2D bagMaterial;
    private BagState BS;
    private scr_CameraScript CS;
    private scr_IngameSoundManager ISG;
    private scr_GameManager GM;
    private ParticleSystem PS;
 
    public int bounces;
    private int remainingBounces;
    public float bouncePower;
    private float throwPower;
    public float throwStrenght;

    public float m_fullThrowDistance;
    private Vector2 m_clampedStartingPosY;
    private Vector2 m_clampedStartingPosX;

    public float m_clampedYPosSize;
    public float m_clampedXPosSize;
    private Vector3 bagTempPos;
    public bool isThrown;

    private float m_inputDelay;
    private bool hasTakenPosInput;
    private bool m_morphBag;
    private float startScale;
    public float maxScaleOffset;
    private bool scaleUpwards;
    public float scaleSpeed;
    void Start()
    {
        startScale = transform.localScale.x;
        hasTakenPosInput = false;
        GM = GameObject.Find("GameManager").GetComponent<scr_GameManager>();
        CS = Camera.main.GetComponent<scr_CameraScript>();
        PS = GetComponent<ParticleSystem>();

        PS.enableEmission = false;

        aimingArrow = GameObject.FindGameObjectWithTag("aimarrow");
        aimingArrow.SetActive(false);
        BS = BagState.idle;
        bag = this.gameObject;
        bagRB = bag.GetComponent<Rigidbody2D>();
        bagRB.isKinematic = true;
        SP = gameObject.GetComponent<spawnParticles>();
        PS.enableEmission = false;

        m_clampedStartingPosY.x = bag.transform.position.y - m_clampedYPosSize; // min
        m_clampedStartingPosY.y = bag.transform.position.y + m_clampedYPosSize; // max
        m_clampedStartingPosX.x = bag.transform.position.x - m_clampedXPosSize;
        m_clampedStartingPosX.y = bag.transform.position.x + m_clampedXPosSize;
        bagMaterial = bag.GetComponent<BoxCollider2D>().sharedMaterial;
        bagMaterial.bounciness = bouncePower;
        remainingBounces = bounces;
        ISG = GameObject.Find("GameManager").GetComponent<scr_IngameSoundManager>();
    }
    public void SetInputDelay(float value)
    {
        m_inputDelay = value;
    }
    // Update is called once per frame
    void Update()
    {
        if(m_morphBag)
        {
            MorphBag();
        }
        if (CS.GetDonePaning() && GM.GetEndGameMenuState() == false && m_inputDelay <= 0)
        {
            InputManager();
        }
        if (BS == BagState.idle && throwPower == 0 && GM.GetEndGameMenuState() == false)
        {
            BagMovementIdleMovement();
        }
        else if (BS == BagState.flying)
        {
            BagFlyingState();
        }
        if (m_inputDelay > 0)
        {
            m_inputDelay = m_inputDelay - Time.deltaTime;
        }
    }
    void MorphBag()
    {
        if (!scaleUpwards)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.7f , maxScaleOffset, transform.localScale.z), scaleSpeed);
            if(transform.localScale.y <= maxScaleOffset)
            {
                scaleUpwards = true;
            }
        }
        else if(scaleUpwards)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(startScale, startScale, transform.localScale.z), scaleSpeed);
            if (transform.localScale.y >= startScale)
            {
                m_morphBag = false;
            }
        }
    }
    public void SetBagState(BagState state)
    {
        BS = state;
    }
    public float GetThrowPower()
    {
        return throwPower;
    }
    public int GetMaxBounces()
    {
        return bounces;
    }
    public int GetRemainingBounces()
    {
        return remainingBounces;
    }
    public void DestroyBag()
    {
        ISG.PlayBagBreak();
        bag.SetActive(false);
    }
    public void ThrowBag()
    {
        if(CS.GetDonePaning())
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = new Vector3(mousePos.x, mousePos.y, 1);
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 direction;
            PS.enableEmission = true;

            direction = objectPos - bagTempPos;
            direction.Normalize();
            BS = BagState.flying;
            DecideThrowStrenght(objectPos, bagTempPos);
            bagRB.gravityScale = 1;
            bagRB.velocity = direction * throwPower * throwStrenght;
        }
    }
    float DecideThrowStrenght(Vector2 target, Vector2 start)
    {
        throwPower = Vector2.Distance(target, start) / m_fullThrowDistance;
        throwPower = Mathf.Clamp01(throwPower);
        return throwPower;
    }
    void BagMovementIdleMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x, mousePos.y, 6);
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        float y = objectPos.y;
        float x = objectPos.x;
        y = Mathf.Clamp(y, m_clampedStartingPosY.x, m_clampedStartingPosY.y);
        x = Mathf.Clamp(x, m_clampedStartingPosX.x, m_clampedStartingPosX.y);
        bag.transform.position = new Vector3(x, y, objectPos.z);
    }
    void BagFlyingState()
    {
        bagRB.isKinematic = false;
        Vector3 mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x, mousePos.y, 1);
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction;


        direction = objectPos - bagTempPos;
        direction.Normalize();
        DecideThrowStrenght(objectPos, bagTempPos);
        if (remainingBounces <= 0)
        {
            ISG.PlayBagBreak();
            CS.MoveTowardsWinBag();
            bagTempPos = Vector3.zero;
        }
    }
    void InputManager()
    {
        if (isThrown == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                bagTempPos = bag.transform.position;
                bagRB.gravityScale = 0;
                aimingArrow.SetActive(true);
                hasTakenPosInput = true;
            }
            if (Input.GetMouseButton(0) && hasTakenPosInput == true)
            {
                bag.transform.position = bagTempPos;
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 5.23f;

                Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
                mousePos.x = mousePos.x - objectPos.x;
                mousePos.y = mousePos.y - objectPos.y;
                float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                BS = BagState.flying;
            }
            if (Input.GetMouseButtonUp(0) && hasTakenPosInput == true)
            {
                ThrowBag();
                aimingArrow.SetActive(false);
                bagRB.gravityScale = 1;
                isThrown = true;
                ISG.PlayBagShootSound();
                CS.SetCameraOrtographicSize(8.0f);
                CS.SetCameraRestriction(false, true);
                hasTakenPosInput = false;
            }
        }
    }
    public bool GetThrowBagStatus()
    {
        return isThrown;
    }
    void OnCollisionEnter2D(Collision2D colli)
    {
        if (colli.gameObject.tag == "wall" && isThrown)
        {
            SP.SpawnParticles(ParticleType.normal);
            remainingBounces -= 1;
            ISG.PlayBagHitSounds();
            m_morphBag = true;
            scaleUpwards = false;
        }
    }
}