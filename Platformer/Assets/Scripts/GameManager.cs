using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMovement m_playerMovement;
    // Update is called once per frame
    void Update()
    { 
        if(m_playerMovement.hasDied)
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
        }
    }
}
