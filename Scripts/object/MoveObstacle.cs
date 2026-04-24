using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float moveSpeed = 10f;

    void Update()
    {
        transform.Translate(Vector3.back
            * moveSpeed
            * Time.deltaTime);

        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }
}