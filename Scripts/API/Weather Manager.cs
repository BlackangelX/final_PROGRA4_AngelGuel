using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class WeatherManager : MonoBehaviour
{
    [Header("Configuracion de Luz")]
    [SerializeField] private Light sunLight;

    [Header("Datos del Clima Actual")]
    public WeatherData currentWeatherData;

    // Tu lista de 10 ciudades
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

       
        if (currentWeatherData.temp < 15 || currentWeatherData.desc.Contains("Cloud") || currentWeatherData.desc.Contains("Rain") || currentWeatherData.desc.Contains("Snow"))
        {
            sunLight.color = new Color(0.6f, 0.75f, 0.9f); 
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.75f, 0.75f, 0.75f);
        }
        else 
        {
            sunLight.color = new Color(1f, 0.9f, 0.7f); 
            sunLight.intensity = 1.3f;
            RenderSettings.fog = false;
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