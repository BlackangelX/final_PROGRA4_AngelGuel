using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text scoreText;

    private float score;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        score += Time.deltaTime * 10f;

        scoreText.text = "Score: " + Mathf.RoundToInt(score);
    }

    public int GetScore()
    {
        return Mathf.RoundToInt(score);
    }
}