using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;
using System;
using System.IO;

public struct World
{
    public GameObject self;
    public int levelAmount;
    public List<Level> l_levels;
}
public struct Level
{
    public int maxScore;
    public int personalBest;
    public string completed;
    public GameObject self;
}
public class scr_overWorld : MonoBehaviour 
{
    private World Worlds;
    public int WorldAmount;
    public Text m_worldFont;
    public Canvas m_canvas;
    private List<World> l_worlds;
    private int[,] worldLevels;
    private string m_world;
    private GameObject m_selectedWorld;
    private GameObject m_WorldMenu;
    private bool m_DoOnce;
    private StreamReader reader;
    private List<Text> textContainer = new List<Text>();
    private List<GameObject> m_ButtonContainer = new List<GameObject>();
    public GameObject GoToLevelButton;
    private Vector3 goToLevelBtnPos;
    private scr_IngameSoundManager ISM;
    private GameObject m_EndGameMenu;
    private bool openSurveyonce;
    private bool showEndGameMenu;
    void Awake()
    {
        openSurveyonce = false;
        m_EndGameMenu = GameObject.Find("EndGameMenu");
        ISM = GetComponent<scr_IngameSoundManager>();
        m_EndGameMenu.SetActive(false);
        showEndGameMenu = false;
    }
    void EndGameStateMenu()
    {
        if(showEndGameMenu)
        {
            m_EndGameMenu.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            m_EndGameMenu.SetActive(true);
            Camera.main.orthographicSize = 17.08f;
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
                        ISM.PlayButtonClick();
                    }
                    else if (obj.name == "GoToMenu")
                    {
                        SceneManager.LoadScene(0);
                        ISM.PlayButtonClick();
                    }
                    else if (obj.name == "NextLevel")
                    {
                        SceneManager.LoadScene(Application.loadedLevel + 1);
                        ISM.PlayButtonClick();
                    }
                    else if (obj.name == "GoToSurvey" && openSurveyonce)
                    {
                        Application.OpenURL("https://docs.google.com/forms/d/1Z00IarFUP5H8czNhRzySRa-H9fMCDFif3JoYpJAHVdY/edit?usp=sharing");
                        openSurveyonce = false;
                    }
                    else if (obj.name == "Overworld")
                    {
                        SceneManager.LoadScene("OverWorldScreen");
                    }

                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                ISM.PlayButtonDeclick();
            }
        }
    }
	void Start () 
    {
        m_WorldMenu = GameObject.Find("WorldLevelMenu");
        m_WorldMenu.SetActive(false);
        m_DoOnce = true;
        goToLevelBtnPos = GoToLevelButton.transform.localPosition;
        l_worlds = new List<World>();
        for(int i = 0; i < WorldAmount; i++) // Loops through all the worlds in the game.
        {
            World p_world = new World();
            m_world = "world_" + i.ToString();
            p_world.self = GameObject.Find(m_world);
            p_world.l_levels = new List<Level>();
            p_world.levelAmount = p_world.self.transform.childCount;
            l_worlds.Add(p_world);
            

            reader = new StreamReader(Application.dataPath + "/world_" + i.ToString() + "_data.txt");
            Debug.Log(Application.dataPath + "/world_" + i.ToString() + "_data.txt");
            for (int y = 0; y < p_world.levelAmount; y++)  // loops through all the levels in each world
            {
                string line = reader.ReadLine();
                string[] variables = line.Split(':');
                Level p_level = new Level();

                //Completed, PersonalBest, MaxLvLScore.
                int OneOrZero;
                OneOrZero = Convert.ToInt32(variables[0]);
                string isCompleted;
                if(OneOrZero == 1)
                {
                   isCompleted = "Completed";
                }    
                else
                {
                    isCompleted = "Not Done";
                }
                   
                p_level.completed = isCompleted;
                p_level.personalBest = Convert.ToInt32(variables[1]);
                p_level.maxScore = Convert.ToInt32(variables[2]);
                string m_level = m_world + " " + y.ToString();
                p_world.l_levels.Add(p_level);
                //Get Saved progess HERE when assinging p_levels variables.
            }
        }
	}
    void SelectWorld()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics2D.Raycast(toMouse.origin, toMouse.direction).transform != null)
            {
                Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                for (int i = 0; i < WorldAmount; i++ )
                {
                    if (obj.name == "world_" + i.ToString())
                    {
                        m_selectedWorld = obj.gameObject;
                    }
                }
            }
        }
    }
    void DeDisplayWorldDetails()
    {
        for (int i = 0; i < textContainer.Count; i++ )
        {
            textContainer[i].gameObject.SetActive(false);
            DestroyObject(textContainer[i].gameObject);
            if(i +1  == textContainer.Count)
            {
                textContainer.Clear();
            }
        }
        for (int i = 0; i < m_ButtonContainer.Count; i++)
        {
            m_ButtonContainer[i].gameObject.SetActive(false);
            DestroyObject(m_ButtonContainer[i]);
            if (i + 1 == m_ButtonContainer.Count)
            {
                m_ButtonContainer.Clear();
            }
        }
        m_WorldMenu.SetActive(false);
        m_WorldMenu.GetComponent<SpriteRenderer>().sortingOrder = -1;

        m_selectedWorld = null;
    }
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (showEndGameMenu)
            {
                showEndGameMenu = false;
                m_EndGameMenu.SetActive(false);
                Camera.main.orthographicSize = 5;
            }
            else if (!showEndGameMenu)
            {
                showEndGameMenu = true;
                m_EndGameMenu.SetActive(true);
                DeDisplayWorldDetails();
            }
        }
        
        EndGameStateMenu();

        SelectWorld();
        if(m_selectedWorld)
        {
            DisplayWorldDetails(m_selectedWorld);
        }
	}
    void DisplayWorldDetails(GameObject world)
    {
        string worldName = world.name;
        string result = System.Text.RegularExpressions.Regex.Replace(worldName, @"\D", "");
        int index = Convert.ToInt32(result);
        m_WorldMenu.SetActive(true);
        m_WorldMenu.GetComponent<SpriteRenderer>().sortingOrder = 1;

       if(m_DoOnce )
       {
           for (int i = 0; i < l_worlds[index].l_levels.Count; i++)
           {
               Text LevelName = Instantiate<Text>(m_worldFont);
               LevelName.rectTransform.parent = m_canvas.transform;
               LevelName.rectTransform.position = new Vector3(660, 850 - 80 * i, 0);
               LevelName.font = m_worldFont.font;
               LevelName.text = "Level " + i.ToString();
               textContainer.Add(LevelName);

               Text completedStatus = Instantiate<Text>(m_worldFont);
               completedStatus.rectTransform.parent = m_canvas.transform;
               completedStatus.rectTransform.position = new Vector3(760, 850 - 80 * i, 0);
               completedStatus.font = m_worldFont.font;
               completedStatus.text = l_worlds[index].l_levels[i].completed.ToString();
               textContainer.Add(completedStatus);

               Text PersonalBest = Instantiate<Text>(m_worldFont);
               PersonalBest.rectTransform.parent = m_canvas.transform;
               PersonalBest.rectTransform.position = new Vector3(890, 850 - 80 * i, 0);
               PersonalBest.font = m_worldFont.font;
               PersonalBest.text = l_worlds[index].l_levels[i].personalBest.ToString();
               textContainer.Add(PersonalBest);

               Text MaxScore = Instantiate<Text>(m_worldFont);
               MaxScore.rectTransform.parent = m_canvas.transform;
               MaxScore.rectTransform.position = new Vector3(980, 850 - 80 * i, 0);
               MaxScore.font = m_worldFont.font;
               MaxScore.text = l_worlds[index].l_levels[i].maxScore.ToString();
               textContainer.Add(MaxScore);

               GameObject m_GoToLevel = Instantiate(GoToLevelButton);
               m_GoToLevel.transform.parent = m_WorldMenu.transform; 
               m_GoToLevel.transform.localPosition = new Vector3(goToLevelBtnPos.x, (goToLevelBtnPos.y- 4.7f) - 1.75f * i, 0);
               string m_gotoLevel = m_GoToLevel.name;
               m_gotoLevel = "GoToLevel_" + i.ToString();
               m_GoToLevel.name = m_gotoLevel;
               m_ButtonContainer.Add(m_GoToLevel);
           }
           m_DoOnce = false;
       }
       if (Input.GetMouseButtonDown(0))
       {
           Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
           if (Physics2D.Raycast(toMouse.origin, toMouse.direction).transform != null)
           {
               for(int i = 0; i < 10;i++)
               {
                   Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                   if(obj != null)
                   {
                       if (obj.name == "CloseWorldLevelMenu")
                       {
                           m_DoOnce = true;
                           DeDisplayWorldDetails();
                       }
                       else if (obj.name == "GoToLevel_" + i.ToString())
                       {
                           SceneManager.LoadScene(i);
                       }
                   }            
               }
           }
       }
    }
}
