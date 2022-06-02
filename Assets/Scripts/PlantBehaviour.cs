using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public GameObject humidForm;
    public float mostHumidity;
    public GameObject normalForm;
    public float normalHumidity;
    public GameObject dryForm;
    public float leastHumidity;

    public Vector2 supportedTemperatureRange;
    GameManager manager;

    private void Start()
    {
        humidForm.SetActive(false);
        dryForm.SetActive(false);
        manager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if (manager.temperature < supportedTemperatureRange.x || manager.temperature > supportedTemperatureRange.y)
        {
            humidForm.SetActive(false);
            normalForm.SetActive(false);
            dryForm.SetActive(true);
            return;
        }

        if (manager.humidity > mostHumidity)
        {
            humidForm.SetActive(true);
            normalForm.SetActive(false);
            dryForm.SetActive(false);
        } 
        else if (manager.humidity > normalHumidity)
        {
            humidForm.SetActive(false);
            normalForm.SetActive(true);
            dryForm.SetActive(false);
        }
        else
        {
            humidForm.SetActive(false);
            normalForm.SetActive(false);
            dryForm.SetActive(true);
        }
    }
}
