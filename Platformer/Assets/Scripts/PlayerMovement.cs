using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameManager gameManager;
    Rigidbody m_rb;
    public float thrust;
    public float drift;
    public float m_maxZVelocity;
    public float m_maxYVelocity;
    public string dir;
    public bool canJump;
    public bool isTouchingVerticalWall;
    private bool isMoving;
    public Vector3 gameGravity;

    void Start ()
    {
        Physics.gravity = gameGravity;
        m_rb = GetComponent<Rigidbody> ();
        dir = "right";
        isMoving = false;
        canJump = true;
        isTouchingVerticalWall = false;
        gameObject.SetActive (true);
    }

    // When initially touching wall
    public void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.CompareTag ("HorizontalPlatforms"))
        {
            canJump = true;
        }
        if (col.gameObject.CompareTag ("VerticalPlatforms"))
        {
            isTouchingVerticalWall = true;
            canJump = !isMoving;
        }
    }

    // When holding into wall
    public void OnCollisionStay (Collision col)
    {
        if (col.gameObject.CompareTag ("HorizontalPlatforms"))
        {
            canJump = true;
        }
        if (col.gameObject.CompareTag ("VerticalPlatforms"))
        {
            canJump = !isMoving;
            isTouchingVerticalWall = true;
        }
    }

    // When leaving wall
    public void OnCollisionExit (Collision col)
    {
        canJump = false;
        isTouchingVerticalWall = false;
    }

    // When goal reached
    public void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.CompareTag ("Goal"))
        {
            gameManager.Win ();
        }
        if (col.gameObject.CompareTag ("Death"))
        {
            gameManager.Respawn ();
        }
    }

    public void Face ()
    {
        if (dir.Equals ("left"))
        {
            transform.Rotate (0, -180, 0, Space.World);
            dir = "right";
        }
        else
        {
            transform.Rotate (0, 180, 0, Space.World);
            dir = "left";
        }
    }

    public void WallJump ()
    {
        if (dir.Equals ("left"))
        {
            m_rb.AddForce (0, 0, 2 * drift, ForceMode.Impulse);
        }
        else
        {
            if (dir.Equals ("right"))
            {
                m_rb.AddForce (0, 0, -2 * drift, ForceMode.Impulse);
            }
        }
        Face ();
    }

    public void Jump ()
    {
        if (isTouchingVerticalWall)
        {
            WallJump ();
        }
        if (m_rb.velocity.y < m_maxYVelocity)
        {
            m_rb.AddForce (0, thrust, 0, ForceMode.Impulse);
        }
    }

    public void Move ()
    {
        if (Mathf.Abs (m_rb.velocity.z) < m_maxZVelocity)
        {
            if (dir.Equals ("left"))
            {
                m_rb.AddForce (0, 0, -1 * drift, ForceMode.Impulse);
            }
            else
            {
                if (dir.Equals ("right"))
                {
                    m_rb.AddForce (0, 0, drift, ForceMode.Impulse);
                }
            }
        }
        else //fast motion switch
        {
            if (dir.Equals ("left") && m_rb.velocity.z > 0)
            {
                m_rb.AddForce (0, 0, -1 * drift, ForceMode.Impulse);
            }
            else
            {
                if (dir.Equals ("right") && m_rb.velocity.z < 0)
                {
                    m_rb.AddForce (0, 0, drift, ForceMode.Impulse);
                }
            }
        }
    }

    void Update ()
    {
        if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow))
        {
            isMoving = true;
            if (Input.GetKey (KeyCode.LeftArrow))
            {
                if (dir != "left")
                {
                    Face ();
                }
                Move ();
            }
            if (Input.GetKey (KeyCode.RightArrow))
            {
                if (dir != "right")
                {
                    Face ();
                }
                Move ();
            }
        }
        else
        {
            isMoving = false;
        }
        if (canJump && Input.GetKeyDown (KeyCode.UpArrow))
        {
            Jump ();
        }
    }
}