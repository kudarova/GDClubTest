using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    private Rigidbody2D rigid;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = joystick.Direction;
        moveVelocity = moveInput.normalized * Level.Player.Speed;
    }

    void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveVelocity * Time.fixedDeltaTime);
    }
}