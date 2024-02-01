using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class SceletonMovement : MonoBehaviour {
    [SerializeField] private bool _isRight;
    [SerializeField] private bool _onGround;
    [SerializeField] private bool _isTargeted;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxDistanceDelta;

    [SerializeField] private States _state;

    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _currentRoutePoint;
    [SerializeField] private Transform[] _routesPoints;

    public bool isTargeted {
        get => _isTargeted;
        set => _isTargeted = value;
    }

    public States state {
        get => _state;
        set => _state = value;
    }

    public Transform currentRoutePoint { 
        get => _currentRoutePoint;
        set => _currentRoutePoint = value;
    }

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        if (_state == States.Move) {
            Move();
        } else if(_state == States.Idle) {
            _animator.SetBool("isWalking", false);
        } 

        if(_isTargeted) {
            Vector2 delta = _currentRoutePoint.position - transform.position;

            if(delta.magnitude > _maxDistanceDelta) {
                _isTargeted = false;
                _currentRoutePoint = GetFarestRoutePoint();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.TryGetComponent<Ground>(out Ground ground)) {
            _onGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!_isTargeted) {
            foreach (Transform routePoint in _routesPoints) {
                if (routePoint.gameObject == collision.gameObject) {
                    _currentRoutePoint = GetNextRoutePoint();

                    break;
                }
            }
        }
    }

    public void StopFollowing() {
        _isTargeted = false;
        _currentRoutePoint = GetFarestRoutePoint();

        Flip();
    }

    public void Jump() {
        if (!_onGround)
            return;

        _onGround = false;
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
    }

    private void Move() {
        Vector2 direction = (_currentRoutePoint.position - transform.position).normalized;

        _animator.SetBool("isWalking", true);

        if ((_isRight && direction.x < 0)
            || (!_isRight && direction.x > 0))
            Flip();

        _rb.velocity = new Vector2(direction.x * _speed, _rb.velocity.y);
    }

    private void Flip() {
        Vector3 objectSizes = gameObject.transform.localScale;

        _isRight = !_isRight;
        gameObject.transform.localScale = new Vector2(-objectSizes.x, objectSizes.y);
    }

    private Transform GetNextRoutePoint() {
        int curentRouteIndex = 0;

        for(int i = 0; i < _routesPoints.Length; i++) {
            if(_currentRoutePoint == _routesPoints[i]) {
                curentRouteIndex = i;

                break;
            }
        }

        return curentRouteIndex == _routesPoints.Length - 1 ? _routesPoints[0] : _routesPoints[curentRouteIndex + 1];
    }

    private Transform GetFarestRoutePoint() {
        int maxIndex = 0;
        float maxMagnitude = (_routesPoints[0].position - transform.position).magnitude;

        for(int i = 0; i < _routesPoints.Length; i++) {
            float tempMagnitude = (_routesPoints[i].position - transform.position).magnitude;  
            
            if(tempMagnitude > maxMagnitude) {
                maxMagnitude = tempMagnitude;
                maxIndex = i;
            }
        }

        return _routesPoints[maxIndex];
    }

    public int GetGazeDirection() => _isRight ? 1 : -1;
}
