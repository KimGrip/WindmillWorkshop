using UnityEngine;
using System.Collections;
public enum PotionType
{
    none, SuperBounce, Gigantic, Minature, Success, Presence, Iron, Score , Slow_Motion
};
public class scr_PotionEffects : MonoBehaviour 
{
    //Self scripts
    private scr_FileHandler FH;
    private scr_IngameSoundManager IS;
    private scr_GameManager GM;
    //Winbag
    private scr_winbagBehaviour WBH;
    
    //Bag scripts
    private spawnParticles SP;
    private scr_bagMovement BM;
    private GameObject bag;

    private PotionType[] m_potionType = new PotionType[9];


    private float m_bouncePower;
    private float m_ScaleMultiplier;
    private float m_goldMultiplier;
    private bool m_extraThrow;
    private float m_gravity;
    private float m_scoreEndMultiplier;
    private float m_drag;
    private float m_timeScale;
    //1.    SuperBounce Potion: 
    //-	Makes the bag increase in velocity for three bounces. Get 25% more velocity instead of slowing down for 3 bounces. (Make sure bag doesn’t clip out of level.)

    //2.	Gigantic Potion:
    //-	Grain Size (and hitbox) increased by 50%.

    //3.	Miniature Potion:
    //-	Grain size (and hitbox) reduced by 50%.

    //4.	Success Potion.
    //-	Any gold picked up is increased by 50%.

    //5.	Presence Potion.
    //-	Gives the player a chance to change the direction that the bag is travelling by clicking around the bag. 

    //6.	Iron Potion.
    //-	Grain falls incredibly quickly and doesn’t bounce.

    //7.	Score Potion.
    //-	Adds 5% of the scores total again.

    //8.	Slow Motion Potion.
    //-	Bag and grain move in slow-motion. (50% of normal speed.)

	void Start () 
    {
        FH = GetComponent<scr_FileHandler>();
        IS = GetComponent<scr_IngameSoundManager>();
        GM = GetComponent<scr_GameManager>();
        WBH = GameObject.FindGameObjectWithTag("win").GetComponent<scr_winbagBehaviour>();
        SP = GameObject.FindGameObjectWithTag("bag").GetComponent<spawnParticles>();
        BM = GameObject.FindGameObjectWithTag("bag").GetComponent<scr_bagMovement>();
        bag = GameObject.FindGameObjectWithTag("bag");


       //  none, SuperBounce, Gigantic, Minature, Success, Presence, Iron, Score , Slow_Motion
        m_potionType[0] = PotionType.none;
        m_potionType[1] = PotionType.SuperBounce;
        m_potionType[2] = PotionType.Gigantic;
        m_potionType[3] = PotionType.Minature;
        m_potionType[4] = PotionType.Success;
        m_potionType[5] = PotionType.Presence;
        m_potionType[6] = PotionType.Iron;
        m_potionType[7] = PotionType.Score;
        m_potionType[8] = PotionType.Slow_Motion;

       m_bouncePower = BM.bouncePower;
       m_ScaleMultiplier = bag.transform.localScale.x;
       m_goldMultiplier = 1 ;
       m_extraThrow = false;
       m_gravity = 1.0f;
       m_scoreEndMultiplier = 1.0f; ;
       m_timeScale = 1;
       m_drag = bag.GetComponent<Rigidbody2D>().drag;
	}
    public void SetBag(GameObject p_bag)
    {
        bag = p_bag;
    }
	public void SetSPPointer(spawnParticles sp)
    {
        SP = sp;
    }
    public void SetBMPointer(scr_bagMovement bm)
    {
        BM = bm;
    }
    PotionType GetPotionType()
    {
        int index =  FH.GetEquipedPotions(GM.GetRemainingBags());

        Debug.Log(m_potionType[index]);
        return m_potionType[index];
    }
	void Update () 
    {
        switch (GetPotionType())
        {
            // p_bouncePower, p_scaleMultiplier, p_goldMultiplier, p_extraThrow,  p_gravity, p_scoreEndMultiplier, p_drag,  p_timeScale)

            case PotionType.SuperBounce:
                SetVariables(1.25f, m_ScaleMultiplier, m_goldMultiplier, m_extraThrow, m_gravity, m_scoreEndMultiplier, m_drag, m_timeScale);
                break;
            case PotionType.none:
                SetVariables(m_bouncePower, m_ScaleMultiplier, m_goldMultiplier, m_extraThrow, m_gravity, m_scoreEndMultiplier, m_drag, m_timeScale);
                break;
            case PotionType.Gigantic:
                SetVariables(m_bouncePower, 1.0f, m_goldMultiplier, m_extraThrow, m_gravity, m_scoreEndMultiplier, m_drag, m_timeScale);
                break;
            case PotionType.Success:
                SetVariables(m_bouncePower, m_ScaleMultiplier, 1.50f, m_extraThrow, m_gravity, m_scoreEndMultiplier, m_drag, m_timeScale);
                break;
            case PotionType.Minature:
                SetVariables(m_bouncePower, 0.3f, m_goldMultiplier, m_extraThrow, m_gravity, m_scoreEndMultiplier, m_drag, m_timeScale);
                break;
            case PotionType.Slow_Motion:
                SetVariables(m_bouncePower, m_ScaleMultiplier, m_goldMultiplier, m_extraThrow, m_gravity, m_scoreEndMultiplier, m_drag, 0.5f);
                break;
            case PotionType.Score:
                SetVariables(m_bouncePower, m_ScaleMultiplier, m_goldMultiplier, m_extraThrow, m_gravity, 1.05f, m_drag, m_timeScale);
                break;
            case PotionType.Presence:
                SetVariables(m_bouncePower, m_ScaleMultiplier, m_goldMultiplier, true, m_gravity, m_scoreEndMultiplier, m_drag, m_timeScale);
                break;
            case PotionType.Iron:
                SetVariables(0.1f, m_ScaleMultiplier, m_goldMultiplier, false, 2.0f, m_scoreEndMultiplier, m_drag, m_timeScale);
                break;
        }
	}
    void SetVariables(float p_bouncePower, float p_scaleMultiplier, float p_goldMultiplier, bool p_extraThrow, float p_gravity, float p_scoreEndMultiplier, float p_drag, float p_timeScale)
    {
        m_bouncePower = p_bouncePower;
        m_ScaleMultiplier = p_scaleMultiplier;
        m_goldMultiplier = p_goldMultiplier;
        m_extraThrow = p_extraThrow;
        m_gravity = p_gravity;
        m_scoreEndMultiplier = p_scoreEndMultiplier;
        m_drag = p_drag;
        m_timeScale = p_timeScale;

        SetClassVariables();
    }
    void SetClassVariables()
    {
        // p_bouncePower, p_scaleMultiplier, p_goldMultiplier, p_extraThrow,  p_gravity, p_scoreEndMultiplier, p_drag,  p_timeScale)
        BM.SetBagBounciness(m_bouncePower);
        BM.SetScale(new Vector3(m_ScaleMultiplier, m_ScaleMultiplier, m_ScaleMultiplier));
        BM.SetExtraThrow(m_extraThrow);
        BM.GetRB().drag = m_drag;
        BM.GetRB().gravityScale = m_gravity;


        Time.timeScale = m_timeScale;
    }
}
