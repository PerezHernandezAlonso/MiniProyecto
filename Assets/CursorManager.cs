using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Bloquear el cursor dentro de la pantalla y ocultarlo
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
