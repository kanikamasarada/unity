using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    CharacterController controller;
    public Transform playerBody; // Player の transform

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (playerBody == null) playerBody = transform; // PlayerMovement が付いてるオブジェクトが Player の場合
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * x + playerBody.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }
}
