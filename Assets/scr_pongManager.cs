using UnityEngine;
using System.Collections;

public class scr_pongManager : MonoBehaviour {

    public GameObject paddle1;
    public GameObject paddle2;

    public GameObject goal_1;
    public GameObject goal_2;

    public GameObject ball;
    private Rigidbody2D blalRB;
    private Vector2 m_vel;
    public float m_speed;
	// Use this for initialization
	void Start () {
        m_vel = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        blalRB = ball.GetComponent<Rigidbody2D>();
        blalRB.velocity = m_vel;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Inputmanager();
        BallManager();
	 }
    void BallManager()
    {
        
    }
    void Inputmanager()
    {
        if(Input.GetKey(KeyCode.W))
        {
            paddle1.transform.position = new Vector3(paddle1.transform.position.x, paddle1.transform.position.y + m_speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            paddle1.transform.position = new Vector3(paddle1.transform.position.x, paddle1.transform.position.y - m_speed);

        }
        if (Input.GetKey(KeyCode.O))
        {
            paddle2.transform.position = new Vector3(paddle2.transform.position.x, paddle2.transform.position.y + m_speed);
        }
        if (Input.GetKey(KeyCode.K))
        {
            paddle2.transform.position = new Vector3(paddle2.transform.position.x, paddle2.transform.position.y - m_speed);
        }
    }
    void OnCollisionEnter2D(Collision2D colli)
    {
        if(colli.gameObject.tag == "ball")
        {
            blalRB.velocity = -blalRB.velocity;
        }
    }
}
