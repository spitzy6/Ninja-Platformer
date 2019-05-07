using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    public PlayerMovement m_playerMovement;
    private Light light;
    private Color go;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        go = new Color (0.382f, 1.0f, 0.382f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerMovement.isTouchingVerticalWall && m_playerMovement.canJump)
        {
            light.color = go;
        } else
        {
            if (!light.color.Equals(new Color()))
            {
                light.color = new Color(light.color.r - 0.03f, light.color.g - 0.1f, light.color.b - 0.03f, 0f);
            }
            // light.color = new Color();
        }
    }
}
