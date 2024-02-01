using UnityEngine;

[RequireComponent(typeof(SceletonMovement))]
[RequireComponent(typeof(SceletonAtack))]
public class Sceleton : MonoBehaviour {
    [SerializeField] private float _attatckDistance;
    [SerializeField] private float _forwardCheckDistance;
    [SerializeField] private float _peripheralCheckDistance;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private Transform _raysStartPoint;

    [SerializeField] private SceletonAtack _atackSystem;
    [SerializeField] private SceletonMovement _movementSystem;

    private void Start() {
        _movementSystem = GetComponent<SceletonMovement>();
        _atackSystem = GetComponent<SceletonAtack>();
    }

    private void Update() {
        int gazeDirection = _movementSystem.GetGazeDirection();

        RaycastHit2D forwardHitInfo = Physics2D.Raycast(_raysStartPoint.position, Vector2.right * gazeDirection, _forwardCheckDistance);
        RaycastHit2D atackHitInfo = Physics2D.Raycast(_raysStartPoint.position, Vector2.right * gazeDirection, _attatckDistance, _playerLayer);
        RaycastHit2D topHitInfo = Physics2D.Raycast(_raysStartPoint.position, new Vector2(gazeDirection, 1), _peripheralCheckDistance);
        RaycastHit2D bottomHitInfo = Physics2D.Raycast(_raysStartPoint.position, new Vector2(0, -1), _peripheralCheckDistance, _groundLayer);

        if (bottomHitInfo.collider != null && forwardHitInfo.collider is null && atackHitInfo.collider is null) {
            _movementSystem.state = States.Move;
        } else if (bottomHitInfo.collider is null) {
            _movementSystem.state = States.Idle;

            if (_movementSystem.isTargeted)
                _movementSystem.StopFollowing();
        }

        if (forwardHitInfo.collider is null)
            return;

        bool isObstacleInFront = CheckIsComponentExistOnObject<Ground>(forwardHitInfo) || CheckIsComponentExistOnObject<Obstacle>(forwardHitInfo);
        bool isObstacleInTop = topHitInfo.collider != null && (CheckIsComponentExistOnObject<Ground>(topHitInfo) || CheckIsComponentExistOnObject<Obstacle>(topHitInfo));

        if(!isObstacleInTop && isObstacleInFront) {
            _movementSystem.Jump();
        }

        if (atackHitInfo.collider != null) {
            _atackSystem.StartAtack();
            _movementSystem.state = States.Idle;

            bool isTargeted = _movementSystem.isTargeted;

            if(!isTargeted) {
                _movementSystem.currentRoutePoint = atackHitInfo.collider.gameObject.transform;
                _movementSystem.isTargeted = true;
            }
        }
    }

    private bool CheckIsComponentExistOnObject<T>(RaycastHit2D objectToCheck) {
        if (objectToCheck.collider is null)
            return false;

        return objectToCheck.collider.TryGetComponent<T>(out T component);
    }
}
