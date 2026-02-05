using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;

    void Start()
    {
        LoadScore();
        UpdateUI();
    }

    // + điểm
    public void AddScore()
    {
        score += 10;
        UpdateUI();
    }

    // Lưu score
    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
        Debug.Log("Score Saved: " + score);
    }

    // Load score
    public void LoadScore()
    {
        score = PlayerPrefs.GetInt("Score", 0);
    }

    // Reset score
    public void ResetScore()
    {
        score = 0;
        PlayerPrefs.DeleteKey("Score");
        UpdateUI();
        Debug.Log("Score Reset");
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
    }
}
