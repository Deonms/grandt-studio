using System.Collections;
using UnityEngine;

public class Stoplight : MonoBehaviour
{
    [System.Serializable]
    public struct StoplightPhase
    {
        public string phaseName;
        public Renderer objectRenderer;
        public Material materialOff;
        public Material materialOn;
        public float duration;
    }

    public StoplightPhase[] lights = new StoplightPhase[3];

    void Start()
    {
        StartCoroutine(StoplightCycle());
    }

    IEnumerator StoplightCycle()
    {
        while (true)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                UpdateLights(i);
                yield return new WaitForSeconds(lights[i].duration);
            }
        }
    }

    void UpdateLights(int activeIndex)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].objectRenderer != null)
            {
                if (i == activeIndex)
                {
                    lights[i].objectRenderer.material = lights[i].materialOn;
                }
                else
                {
                    lights[i].objectRenderer.material = lights[i].materialOff;
                }
            }
        }
    }
}