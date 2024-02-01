using System.Collections;
using UnityEngine;

public class SceletonAtack : MonoBehaviour {
    [SerializeField] private bool _isCooldown;

    [SerializeField] private float _atackDistance;
    [SerializeField] private float _atackCooldown;
    [SerializeField] private float _damage;

    [SerializeField] private LayerMask _playerMask;

    [SerializeField] private Transform _rayStartPoint;

    [SerializeField] private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public void StartAtack() {
        if (_isCooldown)
            return;

        _isCooldown = true;
        _animator.SetBool("isAttacking", true);
    }

    private void EndAttackAnimation() {
        _animator.SetBool("isAttacking", false);
        StartCoroutine(nameof(ReloadAtack));
    }

    private void Atack() {
        int gazeDirection = GetComponent<SceletonMovement>().GetGazeDirection();

        RaycastHit2D hitedOjects = Physics2D.Raycast(_rayStartPoint.position, Vector2.right * gazeDirection, _atackDistance, _playerMask);

        if (hitedOjects.collider != null)
            hitedOjects.collider.GetComponent<PlayerHealth>().GetHit(_damage);
    }

    private IEnumerator ReloadAtack() {
        yield return new WaitForSeconds(_atackCooldown);
        
        _isCooldown = false;
    }
}
