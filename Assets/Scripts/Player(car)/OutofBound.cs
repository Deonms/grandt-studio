using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HerstartLevel();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HerstartLevel();
        }
    }

    private void HerstartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}