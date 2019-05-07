using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CoinBehavior : MonoBehaviour
{
    public bool hasTouchedCoin = false;
    private Rigidbody rb;
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start ()
    {
        hasTouchedCoin = false;
        rb = GetComponent<Rigidbody> ();
        initialPos = transform.position;
    }

    public void OnCollisionEnter (Collision col)
    {
        //hasTouchedCoin = col.gameObject.name.Equals ("Player");
        hasTouchedCoin = true;
    }

    void Update ()
    {
        if (rb.velocity.x > 0)
        {
            rb.velocity = new Vector3 (rb.velocity.x * -1, rb.velocity.y, rb.velocity.z);
        }
        if (hasTouchedCoin)
        {
            rb.velocity = new Vector3 (1.05f * rb.velocity.x, 1.05f * rb.velocity.y, 1.05f * rb.velocity.z);
            if (Vector3.Magnitude (transform.position - initialPos) > 1000)
            {
                GameObject.Destroy (this);
            }
        }
    }
}