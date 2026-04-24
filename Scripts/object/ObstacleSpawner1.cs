using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; 
    public float spawnRate = 2f;      
    public float laneWidth = 3f;      

    void Start()
    {
      
        InvokeRepeating("Spawn", 1f, spawnRate);
    }

    void Spawn()
    {
       
        float randomX = Random.Range(-laneWidth, laneWidth);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, transform.position.z);

        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }

    void Update()
    {
        
        transform.Translate(Vector3.forward * 8f * Time.deltaTime);
    }
}