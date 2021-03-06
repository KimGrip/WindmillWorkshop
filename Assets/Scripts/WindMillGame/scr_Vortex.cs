﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scr_Vortex : MonoBehaviour 
{
    public GameObject portal_2;
    private scr_Vortex Vortex;
    private List<GameObject> unTrappableItems = new List<GameObject>();


    private List<GameObject> m_trappedItems;
    private List<Rigidbody2D> m_trappedRBs;
    public float PullInPower;
    public float PullInSpeed;
    public float MagnitudeDivider;
    private Transform center;
    public float m_eventHorizonDistance;
    public float m_RotationSpeed;
    public float m_deSizeSpeed;
    private CircleCollider2D m_CC;
    public float m_exitScale;
	void Start () 
    {
        m_CC = GetComponent<CircleCollider2D>();
        Vortex = portal_2.GetComponent<scr_Vortex>();

        center = this.transform;
        m_trappedItems = new List<GameObject>();
        m_trappedRBs = new List<Rigidbody2D>();
	}
    public void AddUntrappable(GameObject obj)
    {
        unTrappableItems.Add(obj);
    }
	void Update () 
    {
        MoveTrappedParticles();
        transform.Rotate(0, 0, m_RotationSpeed * Time.deltaTime);
	}
    void ExpelParticles(int y)  //Disaböle collsion between particles.
    {
        Vector2 directionToCenter = center.position - m_trappedItems[y].transform.position;
        m_trappedRBs[y].velocity = -directionToCenter * PullInPower;
        m_trappedRBs[y].gravityScale = 1;
        m_trappedItems[y].transform.SetParent(null);

   //     m_trappedItems[y].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        m_trappedItems[y].transform.position = portal_2.transform.position;
        m_trappedItems[y].transform.localScale = new Vector3(m_exitScale, m_exitScale, m_exitScale);

        Vortex.AddUntrappable(m_trappedItems[y]);

        m_trappedRBs.Remove(m_trappedRBs[y]);
        m_trappedItems.Remove(m_trappedItems[y]);
    }
    void ScaleParticles(float distance, int index)
    {
        Vector3 targetScale = new Vector3(0.1f,0.1f,0.1f) / 30;
        m_trappedItems[index].transform.localScale = Vector3.MoveTowards(m_trappedItems[index].transform.localScale, targetScale, Time.deltaTime * m_deSizeSpeed);
    }
    void MoveTrappedParticles()
    {
        for(int i = 0; i < m_trappedItems.Count; i++)
        {
            float distance = Vector2.Distance(center.position, m_trappedItems[i].transform.position);
            Vector2 directionToCenter = center.position - m_trappedItems[i].transform.position;
            ScaleParticles(distance, i);

            if (distance <= m_eventHorizonDistance)
            {
                ExpelParticles(i);
            }
            else
            {
                m_trappedRBs[i].velocity = Vector2.MoveTowards(m_trappedRBs[i].velocity, directionToCenter, PullInSpeed * Time.deltaTime);
                var fraction = (center.position - m_trappedItems[i].transform.position).magnitude / MagnitudeDivider;
                var maxGravity = 25.0f;
                float gravity;
                if (fraction < 0.0)
                {
                    gravity = maxGravity * (1.0f - fraction);
                    Vector2 vel = new Vector2();
                    m_trappedRBs[i].velocity = vel;
                }
            }

        }
    }
    void OnTriggerEnter2D(Collider2D colli)
    {

        if (colli.gameObject.tag == "particle" )
        {
            m_trappedItems.Add(colli.gameObject);
            Rigidbody2D rb = colli.gameObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            m_trappedRBs.Add(rb);
            colli.transform.SetParent(transform);
            Vortex.AddUntrappable(colli.gameObject);

            for(int i = 0; i < unTrappableItems.Count; i++)
            {
                if(colli.gameObject == unTrappableItems[i].gameObject)
                {

                    GameObject obj;
                    obj = colli.gameObject;
                    for (int y = 0; y < m_trappedItems.Count; y++)
                    {
                        if (m_trappedItems[y] == obj)
                        {
                            m_trappedItems.Remove(m_trappedItems[y]);
                            m_trappedRBs[y].gravityScale = 1;
                            m_trappedRBs.Remove(m_trappedRBs[y]);
                            colli.transform.SetParent(null);
                        }
                    }
                }
            }
        }
        if (colli.gameObject.tag == "bag")
        {
            m_trappedItems.Add(colli.gameObject);
            Rigidbody2D rb = colli.gameObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            m_trappedRBs.Add(rb);
            colli.transform.SetParent(transform);
            Vortex.AddUntrappable(colli.gameObject);

            for (int i = 0; i < unTrappableItems.Count; i++)
            {
                if (colli.gameObject == unTrappableItems[i].gameObject)
                {

                    GameObject obj;
                    obj = colli.gameObject;
                    for (int y = 0; y < m_trappedItems.Count; y++)
                    {
                        if (m_trappedItems[y] == obj)
                        {
                            m_trappedItems.Remove(m_trappedItems[y]);
                            m_trappedRBs[y].gravityScale = 1;
                            m_trappedRBs.Remove(m_trappedRBs[y]);
                            colli.transform.SetParent(null);
                        }
                    }
                }
            }
        }
        //if(colli.gameObject.tag == "bag")
        //{
        //    colli.transform.SetParent(transform);
        //    colli.GetComponent<Rigidbody2D>().velocity = colli.GetComponent<Rigidbody2D>().velocity / 2;
        //    colli.GetComponent<Rigidbody2D>().gravityScale = 0;
        //}
    }
    void OnTriggerExit2D(Collider2D colli)
    {
        if (colli.gameObject.tag == "particle")
        {
            GameObject obj;
            obj = colli.gameObject;
             for(int i = 0; i < m_trappedItems.Count; i++)
             {
                 if(m_trappedItems[i] == obj)
                 {
                     m_trappedItems.Remove(m_trappedItems[i]);
                     m_trappedRBs[i].gravityScale = 1;
                     m_trappedRBs.Remove(m_trappedRBs[i]);
                     colli.transform.SetParent(null);
                 }
             }
        }
        if(colli.gameObject.tag == "bag")
        {
            colli.transform.SetParent(null);
            colli.GetComponent<Rigidbody2D>().gravityScale = 1;
            float xpeliAnus = 3;
            Vector2 VingFåle = colli.gameObject.transform.position - transform.position;
            colli.GetComponent<Rigidbody2D>().AddForce(VingFåle * xpeliAnus, ForceMode2D.Force); 
        }
    }
}
