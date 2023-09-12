using System.Collections;
using UnityEngine;

public class PulsingLight : MonoBehaviour
{
    public Light pointLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float pulseSpeed = 1.0f;
    public float waitTimeMin = 0.5f;
    public float waitTimeMax = 2.0f;

    private float targetIntensity = 1.0f;
    private float currentIntensity;

    void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }
        currentIntensity = pointLight.intensity;
        StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        while (true)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            while (Mathf.Abs(targetIntensity - currentIntensity) > 0.01f)
            {
                currentIntensity = Mathf.MoveTowards(currentIntensity, targetIntensity, Time.deltaTime * pulseSpeed);
                pointLight.intensity = currentIntensity;
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
        }
    }
}
