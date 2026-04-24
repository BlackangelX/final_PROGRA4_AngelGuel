using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject panelGameOver;
    public GameObject hudJuego;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // ESTA ES LA FUNCIÓN QUE CORRIGE TU ERROR
    public void AddScore(int points)
    {
        score += points;
        if (scoreText != null)
        {
            scoreText.text = "cajas " + score;
        }
    }

    public void ClickEnJugar()
    {
        if (PlayFabManager.instance != null) PlayFabManager.instance.menuPrincipalPanel.SetActive(false);
        if (hudJuego != null) hudJuego.SetActive(true);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        if (panelGameOver != null) panelGameOver.SetActive(true);
        if (PlayFabManager.instance != null) PlayFabManager.instance.SendLeaderboard(score);
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RegresarAlMenu()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (hudJuego != null) hudJuego.SetActive(false);
        if (PlayFabManager.instance != null) PlayFabManager.instance.menuPrincipalPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void VerMarcadoresDesdeMuerte()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (PlayFabManager.instance != null) PlayFabManager.instance.AbrirMarcadores();
    }
}