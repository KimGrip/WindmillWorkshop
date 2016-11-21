using UnityEngine;
using System.Collections;
using Medvedya.SpriteDeformerTools;
using System.Collections.Generic;

public class EXAMPLE_createSpriteDeformer : MonoBehaviour
{

    public Sprite sprite;
    public Material material;
    SpriteDeformerStatic mySprite;
    private List<SpritePoint> l_Point = new List<SpritePoint>();
    private List<Vector2> l_Offset = new List<Vector2>();
    private Rigidbody2D rb;
    public Vector2 hitpoint;
    private float distance;
    private Vector2 relativeVel = new Vector2();
    public float resetSpeed;
    public float MaxOffset;
    void Start () 
    {
        rb = GetComponent<Rigidbody2D>();
        hitpoint = Vector2.zero;
        

        mySprite = gameObject.AddComponent<SpriteDeformerStatic>();
        mySprite.sprite = sprite;
        mySprite.material = material;
        mySprite.SetRectanglePoints();

        ////Top Left
        //l_Point.Add(new SpritePoint(1.0f, 1.0f));
        //l_Point.Add(new SpritePoint(0.8f, 0.9f));
        //l_Point.Add(new SpritePoint(0.9f, 0.8f));

        ////Top right
        //l_Point.Add(new SpritePoint(0.2f, 0.9f));
        //l_Point.Add(new SpritePoint(0.1f, 0.8f));
        //l_Point.Add(new SpritePoint(0.0f, 1.0f));


        ////bottom right
        //l_Point.Add(new SpritePoint(0.1f, 0.2f));
        //l_Point.Add(new SpritePoint(0.2f, 0.1f));
        //l_Point.Add(new SpritePoint(0.0f, 0.0f));


        ////bottom left
        //l_Point.Add(new SpritePoint(0.9f, 0.2f));
        //l_Point.Add(new SpritePoint(0.8f, 0.1f));
        //l_Point.Add(new SpritePoint(1.0f, 0.0f));

        l_Point.Add(new SpritePoint(0.25f, 0.25f));
        l_Point.Add(new SpritePoint(0.45f, 0.45f));
        l_Point.Add(new SpritePoint(0.65f, 0.65f));
        l_Point.Add(new SpritePoint(0.85f, 0.85f));
        l_Point.Add(new SpritePoint(0.25f, 0.25f));








        for (int i = 0; i < l_Point.Count; i++ )
        {
            mySprite.AddPoint(l_Point[i]);
            l_Offset.Add(l_Point[i].offset2d);

            Debug.Log("adding");
        }
        //mySprite.AddPoint(new SpritePoint(0.95f, 0.95f));
        //mySprite.AddPoint(new SpritePoint(0.05f, 0.05f));
        //mySprite.AddPoint(new SpritePoint(0.95f, 0.05f));
        //mySprite.AddPoint(new SpritePoint(0.05f, 0.95f));
        //mySprite.AddPoint(new SpritePoint(0.5f, 0.5f));



        Bounds b = mySprite.bounds;
        foreach (var item in mySprite.points)
        {
            b.Encapsulate((Vector3)mySprite.SpritePositionToLocal(item.spritePosition));
        }
        mySprite.bounds = b;
        mySprite.UpdateMeshImmediate();
	}
    public void SetHitPoint(Vector2 point,Vector2 p_relativVel)
    {
        hitpoint = point;
        relativeVel = new Vector2(p_relativVel.x,p_relativVel.y);
    }
    void Update()
    {
        //for (int i = 0; i < l_Point.Count; i++)
        //{
        //    l_Point[i].offset2d =
        //       new Vector2(Mathf.Cos(Time.time) * .15f,
        //           Mathf.Sin(Time.time) * 0.15f);
        //}
        //for (int i = l_Point.Count / 2; i < l_Point.Count; i++)
        //{
        //    l_Point[i].offset2d =
        //       new Vector2(Mathf.Cos(Time.time) * -.15f,
        //           Mathf.Sin(Time.time) * -0.15f);
        //}

        for (int i = 0; i < l_Point.Count; i++ )
        {
            //Sprite position is the original offset that is declared in Start.
            //distance seems to work.
            distance = Vector2.Distance(l_Point[i].spritePosition + new Vector2(transform.position.x, transform.position.y), hitpoint);
            l_Point[i].offset2d = Vector2.MoveTowards(l_Point[i].offset2d, l_Point[i].offset2d + relativeVel + l_Offset[i], Time.deltaTime * distance);


            //Clamp is working.
             l_Point[i].offset2d = new Vector2(
                 Mathf.Clamp(l_Point[i].offset2d.x, l_Offset[i].x - MaxOffset, l_Offset[i].x + MaxOffset),
                 Mathf.Clamp(l_Point[i].offset2d.y, l_Offset[i].y - MaxOffset, l_Offset[i].y + MaxOffset));

             //Move the bag towards its neutralState.
             //l_Point[i].offset2d = Vector2.MoveTowards(l_Point[i].offset2d, l_Offset[i], resetSpeed * Time.deltaTime);
        }

        // l_Point[1].offset2d = new Vector2(Mathf.Cos(Time.time) * .15f, Mathf.Sin(Time.time) * 0.15f);
        mySprite.dirty_offset = true;
        
        
    }
}
