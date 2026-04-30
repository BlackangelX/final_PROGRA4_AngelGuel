using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Collections.Generic;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance;

    [Header("UI Paneles")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject menuPrincipalPanel;
    public GameObject panelLeaderboard;

    [Header("Inputs")]
    public TMP_InputField logEmail;
    public TMP_InputField logPassword;
    public TMP_InputField regEmail;
    public TMP_InputField regPassword;
    public TMP_InputField regUsername;

    [Header("Leaderboard UI")]
    public GameObject filaPrefab;
    public Transform contenedorFilas;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0;
        if (loginPanel != null) loginPanel.SetActive(true);
    }

    // --- REGISTRO Y LOGIN ---
    public void RegisterButton()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = regEmail.text.Trim(),
            Password = regPassword.text.Trim(),
            Username = regUsername.text.Trim(),
            DisplayName = regUsername.text.Trim()
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, r => OpenLogin(), OnError);
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = logEmail.text.Trim(),
            Password = logPassword.text.Trim()
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login correcto.");
        if (loginPanel != null) loginPanel.SetActive(false);
        if (menuPrincipalPanel != null) menuPrincipalPanel.SetActive(true);
    }

    // --- LEADERBOARD LOGIC ---
    public void AbrirMarcadores()
    {
        if (panelLeaderboard != null) panelLeaderboard.SetActive(true);
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Puntaje",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in contenedorFilas) Destroy(item.gameObject);

        foreach (var item in result.Leaderboard)
        {
            GameObject nuevaFila = Instantiate(filaPrefab, contenedorFilas);
            TMP_Text[] textos = nuevaFila.GetComponentsInChildren<TMP_Text>();
            textos[0].text = (item.Position + 1) + ". " + item.DisplayName;
            textos[1].text = item.StatValue.ToString();
        }
    }

  
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate { StatisticName = "Puntaje", Value = score }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, r => Debug.Log("Puntos enviados"), OnError);
    }

    public void CerrarMarcadores() => panelLeaderboard.SetActive(false);
    public void OpenRegister() { loginPanel.SetActive(false); registerPanel.SetActive(true); }
    public void OpenLogin() { registerPanel.SetActive(false); loginPanel.SetActive(true); }
    void OnError(PlayFabError error) => Debug.LogError(error.GenerateErrorReport());
}