using UnityEngine;

public class BackgroundParalax : MonoBehaviour {
    [SerializeField] private bool _blockedVerticalMovement;

    [SerializeField] private float _speed;

    [SerializeField] private string _targetTag;

    [SerializeField] private Vector3 _targetPreviousPosition;
    
    private Transform _target;

    private void Start() {
        _target = GameObject.FindGameObjectWithTag(_targetTag).transform;

        _targetPreviousPosition = transform.position;
    }

    private void Update() {
        if (_target is null)
            return;

        Vector2 direction = _target.position - _targetPreviousPosition;

        if (_blockedVerticalMovement)
            direction.y = 0;

        _targetPreviousPosition = _target.position;

        gameObject.transform.position += (Vector3)direction * _speed;
    }
}
