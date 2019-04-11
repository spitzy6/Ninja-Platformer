using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement m_playerMovement;
    public Vector3 leftOffset;
    public Vector3 rightOffset;
    public float smoothTime;
    private Vector3 offset;
    private Vector3 velocity;

    void Start ()
    {
        offset = transform.position - player.transform.position;
        velocity = new Vector3 (0, 0, 0);
    }

    void Update ()
    {
        if (m_playerMovement.dir.Equals ("left"))
        {
            transform.position = Vector3.SmoothDamp (transform.position, player.transform.position + offset + leftOffset, ref velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp (transform.position, player.transform.position + offset + rightOffset, ref velocity, smoothTime);
        }
    }
}