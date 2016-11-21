//using UnityEngine;
//using System.Collections;

//public class scr_particleTransformer : MonoBehaviour
//{
//    private GameObject objPooler;
//    private GameObject InputCheck;

//    public GameObject InputObject;
//    public float convertionRate;  // how many is required for 1 output;
//    public GameObject OutputObject;
//    private int StoredObjects;

//    // Use this for initialization
//    void Start () 
//    {
//        objPooler = GameObject.FindGameObjectWithTag("pooler");
//    }
	
//    // Update is called once per frame
//    void Update () 
//    {
//        //if storedObjects <= convertion rate.

//        if(StoredObjects != 0)
//        {
//            Debug.Log(StoredObjects / convertionRate);
//            if(StoredObjects / convertionRate == 1)
//            {
//                SpawnConvertedItems();
//            }
//        }
//    }
//    void SpawnConvertedItems()  //Needs to be dynamic
//    {
//      //Ugly code
//        if (OutputObject.name == objPooler.GetComponent<scr_obp>().GetParticle_2().name)
//        {
//            GameObject obj = objPooler.GetComponent<scr_obp>().GetParticle_2();
//            obj.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
//            obj.SetActive(true);
//        }
//        if (OutputObject.name == objPooler.GetComponent<scr_obp>().GetParticle_1().name)
//        {
//            GameObject obj = objPooler.GetComponent<scr_obp>().GetParticle_1();
//            obj.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
//            obj.SetActive(true);
//        }
//        StoredObjects = 0;
//    }
//    void OnCollisionEnter2D(Collision2D colli)
//    {
//        if (colli.gameObject.name == InputObject.name)
//        {
//            colli.gameObject.SetActive(false);
//            StoredObjects++;
//        }
        
//    }
//}