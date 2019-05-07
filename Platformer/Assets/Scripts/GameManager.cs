using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMovement m_playerMovement;
    public GameObject scoreText;
    public GameObject timerText;
    public List<GameObject> coins;
    public Timer timer;
    public float score;

    void Start ()
    {
        score = 0;
        timer = new Timer (GetComponent<AudioSource> ().clip.length);
        coins = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Coin"));
    }

    public void Win ()
    {
        Debug.Log ("Win");
        m_playerMovement.gameObject.SetActive (false);
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }

    public void Respawn ()
    {
        Debug.Log ("Respawn");
        m_playerMovement.gameObject.SetActive (false);
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }

    void Update ()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i].GetComponent<CoinBehavior> ().hasTouchedCoin)
            {
                coins.RemoveAt (i);
                score += 100;
            }
        }
        if (timer.GetTimeLeft () < Time.deltaTime)
        {
            Respawn ();
        }
        timer.Update();
        scoreText.GetComponent<Text> ().text = "Score: " + Mathf.Round (score);
        timerText.GetComponent<Text> ().text = "Timer: " + timer.ToString ();
    }
}