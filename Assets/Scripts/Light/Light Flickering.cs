using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlickering : MonoBehaviour
{
    private Light2D lightToFlicker;

    public float maxIntensity = 5f;
    public float minIntensity = 1f;
    public float timeFlicker = 0.2f;

    private float currentTimer;

    void Start()
    {
        lightToFlicker = GetComponent<Light2D>();
    }

    void Update()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= timeFlicker)
        {
            lightToFlicker.intensity = Random.Range(minIntensity, maxIntensity);
            currentTimer = 0;
        }

    }
}
