using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    public Text scoreText;
    void Awake()
    {
        score = 0;
        scoreText.text = "Score: " + score;
    }
    void Update()
    {
        scoreText.text = "Score: " + score;
    }
    
}
