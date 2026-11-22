using UnityEngine;

public class EchoWaveVisual : MonoBehaviour
{
    public float expandSpeed = 12f;
    public float maxRadius = 15f;

    public Color waveColor = new Color(0.1f, 0.8f, 1f, 1f);

    private float currentRadius = 0f;
    private Material waveMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        waveMaterial = renderer.material;
        waveMaterial.SetColor("_BaseColor", waveColor);

        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        currentRadius += expandSpeed * Time.deltaTime;

        float diameter = currentRadius * 2f;

        transform.localScale = Vector3.one * diameter;
        float progress = currentRadius / maxRadius;

        float alpha = Mathf.Lerp(waveColor.a, 0f, progress);

        //control transparency of the wave
        waveMaterial.SetFloat("_WaveAlpha", alpha);

        if (currentRadius >= maxRadius)

        {
            Destroy(gameObject);
        }

    }

    private void OnDestroy()
    {
        if (waveMaterial != null)
        {
            Destroy(waveMaterial);
        }
    }
}
