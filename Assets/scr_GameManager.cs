using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public enum BagType
{
    def, big, small
};
public enum EndGameState
{
    lose, bronze, silver, gold, master, none
}
public class scr_GameManager : MonoBehaviour
{
    private BagType m_BT;
    private EndGameState m_EGS;
    public Transform m_bagSpawnPos;
    public int m_bagAmount;
    public GameObject bag;
    public List<GameObject> endResult;
    public int MaxWinParticlesNeeded;
    private scr_CameraScript m_Camera;
    private scr_IngameSoundManager ISM;
//    private GameObject m_EndGameMenu;
    private bool m_showEndGameMenu;
    private bool m_DisplayOnce;
    public Transform MedalSpawnPosition;
    public List<int> m_scorePerParticle;
    public List<float> m_MedalRequiredPoints;
    private bool OpenSurveyOnce;
    private Vector3 m_oldCameraPos;

    public float buttonMorphSpeed;
    public Vector2 minMaxButtonScale;
    private bool scaleUpwards;


    // Use this for initialization
    void Awake()
    {
        //m_EndGameMenu = GameObject.Find("EndGameMenu");
        //m_EndGameMenu.SetActive(false);

        OpenSurveyOnce = true;
        m_BT = BagType.def;
        Time.timeScale = 1;
        m_Camera = Camera.main.GetComponent<scr_CameraScript>();
        m_EGS = EndGameState.none;
        ISM = GetComponent<scr_IngameSoundManager>();
    
        if (m_bagAmount > 0 && !GameObject.FindGameObjectWithTag("bag"))
        {
            Instantiate(bag, m_bagSpawnPos.position, Quaternion.identity);
            bag.SetActive(true);
            m_bagAmount -= 1;
        }
        m_showEndGameMenu = false;
        m_DisplayOnce = true;
    }
    void Start()
    {
        ISM.PlayStageStart();
    }
    public int GetMaxWinParticles()
    {
        return MaxWinParticlesNeeded;
    }
    public int GetRemainingBags()
    {
        return m_bagAmount;
    }
    public int GetMaxBagAmount()
    {
        return m_bagAmount + 1;
    }
    public int GetParticleScore(int index)
    {
        return m_scorePerParticle[index];
    }
    public EndGameState GetEndGameState()
    {
        return m_EGS;
    }
    public bool GetEndGameMenuState()
    {
        return m_showEndGameMenu;
    }
    public float GetMedalValue(int index)
    {
        return m_MedalRequiredPoints[index];
    }
    public void CheckEndGameConditions()
    {
        GameObject instantiedobject;
        if (m_EGS == EndGameState.lose)
        {
            SceneManager.LoadScene(Application.loadedLevel);
        }
        if (m_DisplayOnce)
        {
            ISM.PlayStageEnd();

            MedalSpawnPosition.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

            if (m_EGS == EndGameState.master)
            {
                instantiedobject = (GameObject)Instantiate(endResult[3], transform);
                instantiedobject.transform.position = MedalSpawnPosition.position;
                m_DisplayOnce = false;
            }
            else if (m_EGS == EndGameState.gold)
            {
                instantiedobject = (GameObject)Instantiate(endResult[2], transform);
                instantiedobject.transform.position = new Vector3(MedalSpawnPosition.position.x + 15, MedalSpawnPosition.position.y, MedalSpawnPosition.position.z);
                m_DisplayOnce = false;
            }
            else if (m_EGS == EndGameState.silver)
            {
                instantiedobject = (GameObject)Instantiate(endResult[1], transform);
                instantiedobject.transform.position = new Vector3(MedalSpawnPosition.position.x + 15, MedalSpawnPosition.position.y, MedalSpawnPosition.position.z);
                m_DisplayOnce = false;
            }
            else if (m_EGS == EndGameState.bronze)
            {
                instantiedobject = (GameObject)Instantiate(endResult[0], transform);
                instantiedobject.transform.position = new Vector3(MedalSpawnPosition.position.x + 15, MedalSpawnPosition.position.y, MedalSpawnPosition.position.z);
                m_DisplayOnce = false;
            }
        }
        m_showEndGameMenu = true;
    }

    void EndGameStateMenu()
    {
        if (GetMorphableButton() != null)
        {
            ScaleButton(GetMorphableButton());
        }
        if (Input.GetMouseButton(0))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics2D.Raycast(toMouse.origin, toMouse.direction, LayerMask.NameToLayer("button")).transform != null)
            {
                Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                Debug.Log(obj.name);
                if (obj.name == "QuitGame")
                {
                    Application.Quit();
                }
                else if (obj.name == "GoToMenu")
                {
                    SceneManager.LoadScene(0);
                }
                else if (obj.name == "NextLevel")
                {
                    SceneManager.LoadScene(Application.loadedLevel + 1);
                }
                else if (obj.name == "GoToSurvey" && OpenSurveyOnce)
                {
                    Application.OpenURL("https://docs.google.com/forms/d/1Z00IarFUP5H8czNhRzySRa-H9fMCDFif3JoYpJAHVdY/edit?usp=sharing");
                    OpenSurveyOnce = false;
                }
                else if (obj.name == "Overworld")
                {
                    SceneManager.LoadScene("OverWorldScreen");
                }
                ISM.PlayButtonClick();
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            ISM.PlayButtonDeclick();
        }
    }

    void ScaleButton(Transform obj)
    {


            if (!scaleUpwards)
        {
            obj.localScale = Vector3.MoveTowards(obj.localScale, new Vector3(0.7f, minMaxButtonScale.x, obj.localScale.z), buttonMorphSpeed);
            if (obj.localScale.y <= minMaxButtonScale.x)
            {
                scaleUpwards = true;
            }
        }
        else if (scaleUpwards)
        {
            obj.localScale = Vector3.MoveTowards(obj.localScale, new Vector3(minMaxButtonScale.y, minMaxButtonScale.y, obj.localScale.z), buttonMorphSpeed);
            if (obj.localScale.y >= minMaxButtonScale.y)
            {
                scaleUpwards = false;
            }
        }
    }

    Transform GetMorphableButton()
    {
        Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics2D.Raycast(toMouse.origin, toMouse.direction, LayerMask.NameToLayer("button")).transform)
        {
            Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
            Debug.Log(obj.name);
            return obj;
        }
        return null;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_bagAmount > 0 && !GameObject.FindGameObjectWithTag("bag"))
        {
            SpawnNewBag(BagType.def);
        }
        if (m_showEndGameMenu)
        {
            EndGameStateMenu();
        }
        MedalSpawnPosition.position = new Vector3(Camera.main.transform.position.x + 10f, Camera.main.transform.position.y, transform.position.z);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_showEndGameMenu)
            {
                m_showEndGameMenu = false;
                Time.timeScale = 1;
                m_Camera.transform.position = m_oldCameraPos;
            }
            else if (!m_showEndGameMenu)
            {
                m_showEndGameMenu = true;
                m_oldCameraPos = m_Camera.transform.position;
                Time.timeScale = 0;
            }
        }
    }
    public void SetEndGameState(EndGameState state)
    {
        m_EGS = state;
    }
    void SpawnNewBag(BagType type)
    {
        Instantiate(bag, m_bagSpawnPos.position, new Quaternion(0, 0, 0, 0));
        m_Camera.SetCameraXYPos(m_bagSpawnPos.gameObject);
        m_Camera.SetFollowBag(false);
        bag.SetActive(true);
        m_bagAmount -= 1;
    }
}