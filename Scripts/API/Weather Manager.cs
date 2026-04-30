using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class WeatherManager : MonoBehaviour
{
    [Header("Configuracion de Luz")]
    [SerializeField] private Light sunLight;

    [Header("Ajustes Visuales de Clima")]
    public Color colorSoleado = new Color(1f, 0.9f, 0.7f);
    public Color colorFrio = new Color(0.6f, 0.75f, 0.9f);
    public float intensidadSol = 1.5f;
    public float intensidadNubes = 0.6f;

    [Header("Datos del Clima Actual")]
    public WeatherData currentWeatherData;

    private string[] ciudades = {
        "Monterrey", "Tokyo", "London", "New York", "Paris",
        "Seoul", "Madrid", "Cairo", "Sydney", "Berlin"
    };

    void Start()
    {
        if (sunLight == null)
        {
            GameObject lightObj = GameObject.Find("Directional Light");
            if (lightObj != null) sunLight = lightObj.GetComponent<Light>();
        }

        string ciudadElegida = ciudades[UnityEngine.Random.Range(0, ciudades.Length)];
        StartCoroutine(GetWeather(ciudadElegida));
    }

    IEnumerator GetWeather(string city)
    {
        string url = $"https://wttr.in/{city}?format=j1";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error de conexion: " + webRequest.error);
                SetDefaultWeather();
            }
            else
            {
                ParseJson(webRequest.downloadHandler.text, city);
            }
        }
    }

    private void ParseJson(string json, string cityName)
    {
        try
        {
            var data = JSON.Parse(json);
            currentWeatherData.city = cityName;
            currentWeatherData.temp = data["current_condition"][0]["temp_C"].AsFloat;
            currentWeatherData.desc = data["current_condition"][0]["weatherDesc"][0]["value"].Value;

            ApplyWeatherEffects();
            Debug.Log($"CIUDAD: {currentWeatherData.city} | TEMP: {currentWeatherData.temp}°C | CLIMA: {currentWeatherData.desc}");
        }
        catch (Exception e)
        {
            Debug.LogError("Error procesando JSON: " + e.Message);
            SetDefaultWeather();
        }
    }

    private void ApplyWeatherEffects()
    {
        if (sunLight == null) return;

        string desc = currentWeatherData.desc.ToLower();

        if (currentWeatherData.temp < 15 || desc.Contains("cloud") || desc.Contains("rain") || desc.Contains("snow") || desc.Contains("mist"))
        {
            sunLight.color = colorFrio;
            sunLight.intensity = intensidadNubes;

           
            RenderSettings.fog = true;
            RenderSettings.fogColor = colorFrio;
            RenderSettings.fogDensity = 0.02f;

           
            RenderSettings.ambientLight = colorFrio;
        }
        else
        {
            sunLight.color = colorSoleado;
            sunLight.intensity = intensidadSol;

            
            RenderSettings.fog = false;

            
            RenderSettings.ambientLight = colorSoleado;
        }
    }

    private void SetDefaultWeather()
    {
        currentWeatherData.city = "Desconocido";
        currentWeatherData.temp = 20f;
        currentWeatherData.desc = "Clear";
        ApplyWeatherEffects();
    }
}

[Serializable]
public struct WeatherData
{
    public string city;
    public float temp;
    public string desc;
}