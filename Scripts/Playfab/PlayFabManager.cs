using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Collections.Generic;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance;

    [Header("Paneles")]
    public GameObject loginPanel;
    public GameObject registerPanel; // Asegúrate de arrastrar el panel de registro aquí
    public GameObject menuPrincipalPanel;
    public GameObject panelLeaderboard;

    [Header("Inputs Login")]
    public TMP_InputField logEmail;
    public TMP_InputField logPassword;

    [Header("Inputs Registro")]
    public TMP_InputField regEmail;
    public TMP_InputField regPassword;
    public TMP_InputField regUsername;

    private void Awake() { instance = this; }

    // --- LÓGICA DE REGISTRO ---
    public void RegisterButton()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = regEmail.text.Trim(),
            Password = regPassword.text.Trim(),
            Username = regUsername.text.Trim(),
            DisplayName = regUsername.text.Trim()
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Usuario registrado con éxito");
        OpenLogin(); // Regresamos al login para que entre
    }

    // --- LÓGICA DE LOGIN ---
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
        loginPanel.SetActive(false);
        if (registerPanel != null) registerPanel.SetActive(false);
        menuPrincipalPanel.SetActive(true);
    }

    // --- NAVEGACIÓN ENTRE PANELES ---
    public void OpenRegister()
    {
        loginPanel.SetActive(false);
        if (registerPanel != null) registerPanel.SetActive(true);
    }

    public void OpenLogin()
    {
        if (registerPanel != null) registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    // --- MARCADORES ---
    public void AbrirMarcadores()
    {
        if (panelLeaderboard != null) panelLeaderboard.SetActive(true);
    }

    public void CerrarMarcadores()
    {
        if (panelLeaderboard != null) panelLeaderboard.SetActive(false);
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

    void OnError(PlayFabError error) { Debug.LogError(error.GenerateErrorReport()); }
}