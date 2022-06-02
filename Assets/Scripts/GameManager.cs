using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Environmental Elements")]
    public Light sun;
    public Transform water;
    public ParticleSystem rainParticle;
    public ParticleSystem snowParticle;
    public Material groundMaterial;
    public Gradient groundByTemperature;

    [Header("Humidity")]
    public float humidity = 50;
    public float dropletDensity = 1000;
    public float timeBetweenRainfalls = 30;
    public float rainDuration = 5;
    public Slider humiditySlider;
    public TextMeshProUGUI humidityPercentage;

    float timeSinceLastRainfall = 0;
    float timeSinceRainStarted = 0;
    bool isRaining = false;

    [Header("Temperature")]
    public float temperature = 0;
    public Slider temperatureSlider;
    public TextMeshProUGUI temperaturePercentage;

    [Header("Sunlight")]
    public float sunlight = 50;
    public Slider sunlightSlider;
    public TextMeshProUGUI sunlightPercentage;

    [Header("Wind")]
    public float wind = 50;
    public Slider windSlider;
    public TextMeshProUGUI windPercentage;

    [Header("Simulation Speed")]
    public float speed = 1;
    public Slider speedSlider;
    public TextMeshProUGUI speedPercentage;

    Coroutine temperatureChange;
    Coroutine humidityChange;
    private void Start()
    {
        temperatureChange = StartCoroutine(TemperatureCoroutine(temperatureSlider.value));
        humidityChange = StartCoroutine(HumidityCoroutine(humiditySlider.value));
    }
    private void Update()
    {
        if (!isRaining)
        {
            timeSinceLastRainfall += Time.deltaTime;
        } 
        else 
        {
            timeSinceRainStarted += Time.deltaTime;
            if(timeSinceRainStarted > rainDuration)
            {
                timeSinceRainStarted = 0;
                isRaining = false;
                if (rainParticle.isPlaying)
                    rainParticle.Stop();
                if (snowParticle.isPlaying)
                    snowParticle.Stop();
            }
        }
        if (timeSinceLastRainfall >= timeBetweenRainfalls)
        {
            timeSinceLastRainfall = 0;
            if (temperature > 0)
                rainParticle.Play();
            else
                snowParticle.Play();
            isRaining = true;
        }
        if (sunlight <= .9f)
            temperature -= speed * Time.deltaTime;
        else
            temperature += speed * Time.deltaTime;
        temperaturePercentage.text = temperature.ToString("0.0") + "°";
        humidityPercentage.text = humidity.ToString("0.0") + "%";
    }
    public void updateHumidity()
    {
        try
        {
            StopCoroutine(humidityChange);
        }
        catch (Exception e)
        {
            Debug.Log("Not a null reference exception here, move along");
        }
        humidityChange = StartCoroutine(HumidityCoroutine(humiditySlider.value));
        rainParticle.emissionRate = snowParticle.emissionRate = dropletDensity * humiditySlider.value / 10;
    }
    public void updateTemperature()
    {
        try
        {
            StopCoroutine(temperatureChange);
        }
        catch (Exception e)
        {
            Debug.Log("Not a null reference exception here, move along");
        }
        temperatureChange = StartCoroutine(TemperatureCoroutine(temperatureSlider.value));
        if (temperatureSlider.value > 0)
        {
            if (snowParticle.isPlaying)
            {
                snowParticle.Stop();
                rainParticle.Play();
            }
        }
        else 
        {
            if (rainParticle.isPlaying)
            {
                snowParticle.Play();
                rainParticle.Stop();
            }
        }
    }
    public void updateSunlight()
    {
        sunlight = sunlightSlider.value;
        sunlightPercentage.text = sunlight.ToString("0.0");
        sun.intensity = sunlight;
    }
    public void updateWind()
    {
        wind = windSlider.value;
        windPercentage.text = wind.ToString("0.0") + "%";
    }
    public void updateSimulationSpeed()
    {
        speed = speedSlider.value;
        speedPercentage.text = speed.ToString("0.0") + "X";
    }
    IEnumerator TemperatureCoroutine(float targetTemperature)
    {
        if(temperature < targetTemperature)
        {
            while(temperature < targetTemperature)
            {
                temperature += speed / 10;
                groundMaterial.color = groundByTemperature.Evaluate(Mathf.InverseLerp(temperatureSlider.minValue, temperatureSlider.maxValue, temperature));
                yield return null;
            }
        } 
        else
        {
            while (temperature > targetTemperature)
            {
                temperature -= speed / 10;
                groundMaterial.color = groundByTemperature.Evaluate(Mathf.InverseLerp(temperatureSlider.minValue, temperatureSlider.maxValue, temperature));
                yield return null;
            }
        }
    }
    IEnumerator HumidityCoroutine(float targetHumidity)
    {
        if (humidity < targetHumidity)
        {
            while (humidity < targetHumidity)
            {
                humidity += speed / 10;
                timeBetweenRainfalls += 10 / humidity;
                rainDuration += humidity / 10;
                yield return null;
            }
        }
        else
        {
            while (humidity > targetHumidity)
            {
                humidity -= speed / 10;
                timeBetweenRainfalls -= 10 / humidity;
                rainDuration -= humidity / 10;
                yield return null;
            }
        }
    }
}
