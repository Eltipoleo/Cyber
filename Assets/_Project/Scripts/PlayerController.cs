using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 7f;
    
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Camera mainCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    void Update()
    {
        // 1. Leer el movimiento (Teclas WASD) usando el New Input System
        if (Keyboard.current != null)
        {
            float moveX = (Keyboard.current.dKey.isPressed ? 1f : 0f) - (Keyboard.current.aKey.isPressed ? 1f : 0f);
            float moveY = (Keyboard.current.wKey.isPressed ? 1f : 0f) - (Keyboard.current.sKey.isPressed ? 1f : 0f);
            
            // Normalizar para que no se mueva más rápido en diagonal
            moveInput = new Vector2(moveX, moveY).normalized; 
        }

        // 2. Leer la posición del ratón para apuntar (Twin-Stick)
        if (Mouse.current != null)
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(mouseScreenPosition);
            
            // Calcular el ángulo hacia el cursor
            Vector2 aimDirection = mouseWorldPosition - transform.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f; 
            
            // Aplicar la rotación
            rb.rotation = aimAngle;
        }

        // 3. Reinicio rápido (Requerimiento del GDD)
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        // Aplicar la velocidad física en el FixedUpdate para evitar temblores
        rb.linearVelocity = moveInput * moveSpeed;
    }
}