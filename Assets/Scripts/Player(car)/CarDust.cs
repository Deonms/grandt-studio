using UnityEngine;

public class CarDustEffect : MonoBehaviour
{
    public Transform[] wheelLocations;
    public Color dustColor = Color.white;
    public float minSpeed = 2f;
    public float particleSize = 0.85f;
    public float spawnRate = 40f;

    private Rigidbody rb;
    private ParticleSystem[] dustSystems;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dustSystems = new ParticleSystem[wheelLocations.Length];

        Material dustMat = new Material(Shader.Find("Sprites/Default"));
        dustMat.mainTexture = CreateCircleTexture();

        for (int i = 0; i < wheelLocations.Length; i++)
        {
            if (wheelLocations[i] == null) continue;

            GameObject obj = new GameObject("Dust");
            obj.transform.SetParent(wheelLocations[i]);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            ParticleSystem ps = obj.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.startLifetime = 0.6f;
            main.startSpeed = 0.5f;
            main.startSize = particleSize;
            main.startColor = dustColor;
            main.playOnAwake = false;

            var emission = ps.emission;
            emission.rateOverTime = spawnRate;

            var shape = ps.shape;
            shape.angle = 10f;
            shape.radius = 0.15f;

            obj.GetComponent<ParticleSystemRenderer>().material = dustMat;

            dustSystems[i] = ps;
        }
    }

    Texture2D CreateCircleTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size);
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                float alpha = Mathf.Clamp01(1f - (dist / radius));
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        tex.Apply();
        return tex;
    }

    void Update()
    {
        if (rb == null) return;

        bool isMoving = rb.linearVelocity.magnitude > minSpeed;

        foreach (ParticleSystem ps in dustSystems)
        {
            if (ps == null) continue;

            var emission = ps.emission;
            emission.rateOverTime = spawnRate;

            if (isMoving && !ps.isPlaying) ps.Play();
            else if (!isMoving && ps.isPlaying) ps.Stop();
        }
    }
}