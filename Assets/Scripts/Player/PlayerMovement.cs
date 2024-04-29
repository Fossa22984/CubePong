using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _acceleration = 20f;

    public void StartCircleMovement(Vector2 vector2)
    {
        _rb.velocity = vector2 * _acceleration;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _rb.velocity = Vector2.zero;
        }
    }
}
