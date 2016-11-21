using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class scr_CameraScript : MonoBehaviour
{
    private GameObject m_Camera;
    private Transform m_CameraTF;
    private GameObject bag;
    private scr_obp pooler;
    private scr_bagMovement BM;
    public bool m_followBag;
    public bool m_lockHorizontalMovement;
    [Space(20)]
    public float OrtographicStartSize;
    public Vector2 MaxMinOrtographicSize;
    public float OrotgraphicChangeSpeed;
    /// <summary>
    /// Starts by going from the highest in list to the lowest in the list, so from element 0 -> 10
    /// </summary>
    public List<GameObject> m_CameraMovmentPoints;
    public float m_CameraPanSpeed;
    public float m_MousePanSpeed;
    public float m_MouseWheelPower;
    public Vector2 m_ClampedScrollSize;
    private bool m_donePanning;
    private GameObject m_winBag;
    public float m_CameraAtWinBagOffset;
    private bool doOnce;
    private Vector3 m_mousePos;
    private BoxCollider2D m_CameraBounds;
    // Use this for initialization
    void Start()
    {
        doOnce = true;
        pooler = GameObject.FindGameObjectWithTag("pooler").GetComponent<scr_obp>();
        m_donePanning = false;
        m_Camera = Camera.main.gameObject;
        m_CameraTF = m_Camera.GetComponent<Transform>();
        m_CameraTF.position = new Vector3(m_CameraMovmentPoints[0].transform.position.x, m_CameraMovmentPoints[0].transform.position.y, m_CameraTF.position.z);
        m_CameraBounds = GameObject.Find("CameraBoundaries").GetComponent<BoxCollider2D>();
        m_winBag = GameObject.FindGameObjectWithTag("win");
    }
    // Camera dynamicZoom;
    // Update is called once per frame
    //camera boundaries,
    //camera shake
    //if press space, skip camera first pan.

    void Update()
    {
        bag = GameObject.FindGameObjectWithTag("bag");
        if (bag != null)
        {
            BM = bag.GetComponent<scr_bagMovement>();
        }
        if (Input.GetMouseButton(1))
        {
            CameraRightClickPan();
            LimitRightClickPan();
            m_donePanning = false;
            m_CameraMovmentPoints.Clear();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            doOnce = true;
        }
        if (m_donePanning)
        {
            if (m_followBag && bag != null)
            {
                FollowBag(m_lockHorizontalMovement);
                AdjustOrtographicSizeToFitParticles();
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float value = Input.GetAxis("Mouse ScrollWheel");
                CameraMouseWheelScroll(value);
            }
        }
        else
        {
            JumpToLastMovementPoint();
            OnStartCameraPan();
        }
        //Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, OrtographicStartSize, OrotgraphicChangeSpeed);
    }
    void CameraMouseWheelScroll(float p_value)
    {
        Camera.main.orthographicSize -= p_value * m_MouseWheelPower;
        LimitScrollWheelOffset();
    }
    public void SetFollowBag(bool follow)
    {
        m_followBag = follow;
    }
    public bool GetDonePaning()
    {
        return m_donePanning;
    }
    public void MoveTowardsWinBag()
    {
        m_donePanning = false;
        m_CameraMovmentPoints.Add(m_winBag);
    }
    void AdjustOrtographicSizeToFitParticles()
    {
        //float targetValue;
        //targetValue = Vector2.Distance(pooler.GetLowestActiveYParticle().transform.position, bag.transform.position);
        //OrtographicStartSize = Mathf.Lerp(OrtographicStartSize, targetValue, OrotgraphicChangeSpeed);
        //OrtographicStartSize = Mathf.Clamp(OrtographicStartSize, MaxMinOrtographicSize.x, MaxMinOrtographicSize.y);
    }
    void LimitScrollWheelOffset()
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, MaxMinOrtographicSize.x, MaxMinOrtographicSize.y);
    }
    void CameraRightClickPan()
    {
        if(doOnce)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = new Vector3(mousePos.x, mousePos.y, 6);
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            m_mousePos = objectPos;
            doOnce = false;
        }
        Vector2 direction;
        Vector3 p_mousePos = Input.mousePosition;
        p_mousePos = new Vector3(p_mousePos.x, p_mousePos.y, 6);
        Vector3 p_objectPos = Camera.main.ScreenToWorldPoint(p_mousePos);
       
        direction = p_objectPos - m_mousePos; //Isnt using Direction
        m_CameraTF.position = Vector3.Lerp(m_CameraTF.position, new Vector3(m_CameraTF.position.x - direction.x, m_CameraTF.position.y - direction.y,
            m_CameraTF.position.z), m_MousePanSpeed * Time.deltaTime * 2);
    }
    void LimitRightClickPan()
    {
        Vector2 x_minMAx = new Vector2(m_CameraBounds.bounds.extents.x, m_CameraBounds.bounds.extents.y); //returns the height and width to center.
        Vector2 boxPos = m_CameraBounds.transform.position;
                                       // smallest, bounds - pos
        Vector2 xLimit = new Vector2(-x_minMAx.x - -boxPos.x, x_minMAx.x + boxPos.x); // The max and min X pos for Camera.
        Vector2 yLimit = new Vector2(-x_minMAx.y - -boxPos.y, x_minMAx.y + boxPos.y); // The max and min Y pos for Camera.       
        m_CameraTF.position = new Vector3(Mathf.Clamp(m_CameraTF.position.x, xLimit.x, xLimit.y), Mathf.Clamp(m_CameraTF.position.y, yLimit.x, yLimit.y), m_CameraTF.position.z);

        m_CameraTF.position = new Vector3(Mathf.Clamp(m_CameraTF.position.x, xLimit.x, xLimit.y), //Clamps X pos
            Mathf.Clamp(m_CameraTF.position.y, yLimit.x, yLimit.y), m_CameraTF.position.z);   // Clamps Y Pos

 
        m_CameraTF.position = new Vector3(Mathf.Clamp(m_CameraTF.position.x, xLimit.x, xLimit.y), Mathf.Clamp(m_CameraTF.position.y, yLimit.x, yLimit.y), m_CameraTF.position.z);
        m_CameraTF.position = new Vector3(Mathf.Clamp(m_CameraTF.position.x, xLimit.x, xLimit.y), //Clamps X pos
            Mathf.Clamp(m_CameraTF.position.y, yLimit.x, yLimit.y), m_CameraTF.position.z);   // Clamps Y Pos
    }
    public void JumpToLastMovementPoint()
    {
        if (Input.GetMouseButtonUp(0))
        {
            int index = m_CameraMovmentPoints.Count;
            Vector3 pos = m_CameraMovmentPoints[index - 1].transform.position;
            m_CameraTF.position = new Vector3(pos.x, pos.y, m_CameraTF.position.z);
            m_CameraMovmentPoints.Clear();
            m_donePanning = true;
            BM.SetInputDelay(0.2f);
        }
    }
    void OnStartCameraPan()
    {
        if (m_CameraMovmentPoints.Count == 0)
        {
            m_donePanning = true;
            return;
        }
        Vector3 position = m_CameraMovmentPoints[0].transform.position;
        if (m_CameraMovmentPoints[0].gameObject == GameObject.FindGameObjectWithTag("win"))
        {
            position.y = position.y + m_CameraAtWinBagOffset;
        }
        m_CameraTF.position = Vector3.MoveTowards(m_CameraTF.position, new Vector3(position.x, position.y, m_CameraTF.position.z), m_CameraPanSpeed * Time.deltaTime);
        if (m_CameraMovmentPoints[0].gameObject == GameObject.FindGameObjectWithTag("win"))
        {
            OrtographicStartSize = 16;
            if (pooler.GetIdleActiveParticles().Count == 0 && position.y == m_CameraTF.position.y)
            {
                m_CameraMovmentPoints.RemoveAt(0);
            }
        }
        else if (m_CameraMovmentPoints[0].transform.position.x == m_CameraTF.position.x && m_CameraMovmentPoints[0].transform.position.y == m_CameraTF.position.y)
        {
            m_CameraMovmentPoints.RemoveAt(0);
        }

    }
    public void SetCameraXYPos(GameObject pos)
    {
        m_donePanning = false;
        m_CameraMovmentPoints.Add(pos);
    }
    public void SetCameraOrtographicSize(float value)
    {
        OrtographicStartSize = value;
    }
    public void SetCameraOrtoSizeQuick(float value)
    {
        OrtographicStartSize = value;
        Camera.main.orthographicSize = value;
    }
    public void SetCameraRestriction(bool lockHorizontal, bool followBag)
    {
        m_lockHorizontalMovement = lockHorizontal;
        m_followBag = followBag;
    }
    void FollowBag(bool lockHorizontal)
    {
        if (m_lockHorizontalMovement)
        {
            m_CameraTF.position = Vector3.MoveTowards(m_CameraTF.position, new Vector3(m_CameraTF.position.x, bag.transform.position.y, m_CameraTF.position.z), 0.25f);
        }
        else
        {
            m_CameraTF.position = Vector3.MoveTowards(m_CameraTF.position, new Vector3(bag.transform.position.x, bag.transform.position.y, m_CameraTF.position.z), 0.25f);

        }

    }
}