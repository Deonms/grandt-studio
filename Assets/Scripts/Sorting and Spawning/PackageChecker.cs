using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PackageChecker : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private GameObject _boxPrefebToDestroy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _tag)
        {
            print("is destroyed");
            Destroy(collision.gameObject);

        }
        else 
        {
            //nothing will happen to the other objects to prevent to prevent the game from breaking
        }
        
    }
}
