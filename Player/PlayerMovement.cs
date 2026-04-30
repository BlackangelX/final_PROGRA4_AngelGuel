using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float sideSpeed = 7f;
    public Rigidbody rb;
    public FixedJoystick joystick;

    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            
            Vector3 forwardMove = transform.forward * forwardSpeed * Time.fixedDeltaTime;
            float horizontalInput = joystick.Horizontal;
            Vector3 sideMove = transform.right * horizontalInput * sideSpeed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + forwardMove + sideMove);
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Caja"))
        {
            if (GameManager.instance != null) GameManager.instance.GameOver();
        }
    }
}