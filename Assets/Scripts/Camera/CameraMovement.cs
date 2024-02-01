using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] private float _speed;

    [SerializeField] private string _targetTag;

    [SerializeField] private Vector2 _offset;

    private Transform _target;

    private void Start() {
        GameObject temp = GameObject.FindGameObjectWithTag(_targetTag);

        _target = temp != null ? temp.transform : null;
    }

    private void Update() {
        if (_target is null)
            return;

        Vector2 direction = (Vector2)(_target.position - transform.position) + _offset; 

        transform.Translate(direction * _speed * Time.deltaTime);
    }
}
