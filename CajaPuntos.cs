using UnityEngine;

public class CajaPuntos : MonoBehaviour
{
    private bool puntoEntregado = false;
    private Transform jugador;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) jugador = p.transform;
    }

    void Update()
    {
        // Si el jugador pasa la posición Z de la caja, le damos el punto
        if (jugador != null && !puntoEntregado && jugador.position.z > transform.position.z)
        {
            puntoEntregado = true;
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(1); // Aquí es donde se conectan
            }
        }
    }
}