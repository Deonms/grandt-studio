using System.Collections.Generic;
using UnityEngine;

public class SpawningBoxes : MonoBehaviour
{
    [SerializeField] private List<GameObject> _boxPrefeb = new List<GameObject>();
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _maxSpawning = 10; //to reduce lag can be changed during release and testing

    private List<GameObject> _spawnedboxes = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPackages();
    }

    // Update is called once per frame
    void Update()
    {
        _spawnedboxes.RemoveAll(package => package == null);

        if (_spawnedboxes.Count < _maxSpawning)  //checker for the amount of packages if its not above the maximum or equal to it so no lag can exist
        {
            SpawnPackages(); //this summons the packages
        }
    }

    private void SpawnPackages()
    {
        if (_boxPrefeb.Count == 0)
        {
            print("Debug: no package prefab yet");
            return;
        }

        int randomIndex = Random.Range(0, _boxPrefeb.Count);
        GameObject prefabToSpawn = _boxPrefeb[randomIndex];

        Vector3 spawnPosition = _spawnPoint != null ? _spawnPoint.position : transform.position;
        GameObject newPackage = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        _spawnedboxes.Add(newPackage);
    }

}
