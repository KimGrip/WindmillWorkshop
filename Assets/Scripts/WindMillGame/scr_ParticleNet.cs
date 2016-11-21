using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scr_ParticleNet : MonoBehaviour 
{
    private GameObject netCore;
    public GameObject netAnchor;
    private List<GameObject> m_anchorPoints = new List<GameObject>();

	void Start () 
    {
        netCore = this.gameObject;
        for (int i = 0; i < 1; i++ )
        {
          for(int j = 0; j < 6; j++)
          {
              GameObject obj = (GameObject)Instantiate(netAnchor, this.transform);
              HingeJoint2D objHJ;
              obj.transform.position = new Vector3(transform.position.x  + 1.5f * i, transform.position.y - 1.0f * j, transform.position.z);
              obj.AddComponent<HingeJoint2D>();
              objHJ = obj.GetComponent<HingeJoint2D>();
              objHJ.enableCollision = true;
              if(i== 0 && j < 1)
              {
                  objHJ.connectedAnchor.Set(netCore.transform.position.x, netCore.transform.position.y);
              }
              else
              {
                  Debug.Log("creatingAnchorRB");
                  objHJ.connectedBody = m_anchorPoints[i + j].GetComponent<Rigidbody2D>();
              }
              m_anchorPoints.Add(obj);
          }

        }
	}

	void Update () 
    {
        Debug.Log(m_anchorPoints.Count);
	}
}
