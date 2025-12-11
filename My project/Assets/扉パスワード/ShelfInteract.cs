using UnityEngine;

public class ShelfInteract : MonoBehaviour
{
    public GameObject numberPanelCanvas;
    public float interactDistance = 3f;
    public Transform player;

    private bool isOpen = false;

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist < interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            numberPanelCanvas.SetActive(isOpen);

            Cursor.visible = isOpen;

            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    
    }
    }


    

