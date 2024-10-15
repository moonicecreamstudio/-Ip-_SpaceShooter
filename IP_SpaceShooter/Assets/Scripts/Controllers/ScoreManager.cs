using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Slider player1Health;
    public GameObject player1HealthUI;
    public GameObject player1Start;

    public Slider player2Health;
    public GameObject player2HealthUI;
    public GameObject player2Start;
    private float timer;
    private float timer2;

    public void Start()
    {
        player1HealthUI.SetActive(false);
        player2HealthUI.SetActive(false);
    }

    public void Update()
    {
        if (player1Health.value <= 0)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                player1HealthUI.SetActive(false);
                player1Start.SetActive(true);
                timer = 0;
            }
        }

        if (player2Health.value <= 0)
        {
            timer2 += Time.deltaTime;
            if (timer2 > 2)
            {
                player2HealthUI.SetActive(false);
                player2Start.SetActive(true);
                timer2 = 0;
            }
        }
    }

    // Player 1
    public void HidePlayer1Start()
    {
        player1Start.SetActive(false);
    }

    public void ShowPlayer1Health()
    {
        player1HealthUI.SetActive(true);
    }

    public void Player1TakeDamage(float damage)
    {
        player1Health.value -= damage;
    }
    public void Player1ResetHealth()
    {
        player1Health.value = 3;
    }

    // Player 2

    public void HidePlayer2Start()
    {
        player2Start.SetActive(false);
    }

    public void ShowPlayer2Health()
    {
        player2HealthUI.SetActive(true);
    }

    public void Player2TakeDamage(float damage)
    {
        player2Health.value -= damage;
    }
    public void Player2ResetHealth()
    {
        player2Health.value = 3;
    }
}
