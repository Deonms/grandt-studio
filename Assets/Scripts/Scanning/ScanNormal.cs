using UnityEngine;
using TMPro;
using System.Collections;

public class ScanNormal : MonoBehaviour
{
    public Transform scanpoint;
    public float scanRange = 10f;
    public TMP_Text BoxState;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Scan();
        }
    }

    void Scan()
    {   
        IEnumerator HideText()
        {
            yield return new WaitForSeconds(2f);
            BoxState.text = "Scan Ready";
            BoxState.color = Color.green;
        }
        if (Physics.Raycast(scanpoint.position, scanpoint.forward, out RaycastHit hit, scanRange))
        {
            if (hit.collider.CompareTag("Fragile"))
            {
                BoxState.text = "Fragile";
                BoxState.color = Color.red;
            }
            else if (hit.collider.CompareTag("liquid"))
            {
                BoxState.text = "Liquid";
                BoxState.color = Color.red;
            }
            else if (hit.collider.CompareTag("light"))
            {
                BoxState.text = "Light";
                BoxState.color = Color.red;
            }
            else if (hit.collider.CompareTag("heavy"))
            {
                BoxState.text = "Heavy";
                BoxState.color = Color.red;
            }
            else if (hit.collider.CompareTag("normal"))
            {
                BoxState.text = "Non-Fragile";
                BoxState.color = Color.red;
            }
            StartCoroutine(HideText());
        }
    }
}
