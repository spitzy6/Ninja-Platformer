using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody m_rb;
    public float thrust;
    public float downthrust;
    public float drift;
    public float m_maxZVelocity;
    public float m_maxYVelocity;
    private string dir;
    private bool canJump;
    private bool isTouchingVerticalWall;
    public bool hasWon;
    public bool hasDied;
    public Vector3 gameGravity;

    void Start ()
    {
        Physics.gravity = gameGravity;
        m_rb = GetComponent<Rigidbody> ();
        dir = "left";
        hasWon = false;
        hasDied = false;
        canJump = true;
        isTouchingVerticalWall = false;
        gameObject.SetActive (true);
    }

    public void Win ()
    {
        Debug.Log ("you win" + hasWon);
        gameObject.SetActive (false);
    }

    public void Respawn ()
    {
        Debug.Log ("Respawn: " + hasDied);
        gameObject.SetActive (false);
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
            canJump = true;
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
            canJump = true;
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
        hasWon = col.gameObject.CompareTag ("Goal");
        hasDied = col.gameObject.CompareTag ("Death");
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
        if (!(hasWon || hasDied))
        {
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
            if (canJump && Input.GetKeyDown (KeyCode.Space))
            {
                Jump ();
            }
        }
        else if (hasDied)
        {
            Respawn ();
        }
        else
        {
            Win ();
        }
    }
}