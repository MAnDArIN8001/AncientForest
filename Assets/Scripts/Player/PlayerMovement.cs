using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] private bool _isRight;
    [SerializeField] private bool _isOnGround;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [SerializeField] private KeyCode _jumpKey;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        float direction = Input.GetAxis("Horizontal");

        Move(direction);

        if (_isOnGround && Input.GetKey(_jumpKey))
            Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.TryGetComponent<Ground>(out Ground ground)) {
            _isOnGround = true;
            _animator.SetBool("isOnGround", _isOnGround);
        }
    }

    private void Jump() {
        _isOnGround = false;
        _animator.SetBool("isOnGround", _isOnGround);

        _rb.velocity = new Vector2() {
            x = _rb.velocity.x,
            y = _jumpForce,
        };

    }

    private void Move(float direction) {
        if ((_isRight && direction < 0) 
            || (!_isRight && direction > 0))
            Flip();

        _rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);
        _animator.SetFloat("speed", Mathf.Abs(direction));
    }

    private void Flip() {
        Vector3 objectSizes = gameObject.transform.localScale;

        _isRight = !_isRight;
        gameObject.transform.localScale = new Vector2(-objectSizes.x, objectSizes.y);
    }
}
