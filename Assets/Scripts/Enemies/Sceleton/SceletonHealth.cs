using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SceletonHealth : MonoBehaviour, IHitble {
    [SerializeField] private bool _isAlive;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;

    [SerializeField] private Animator _animator;

    public delegate void SceletonDied();

    private event SceletonDied _EOnDied;
    public event SceletonDied EOnDied { 
        add => _EOnDied += value;
        remove => _EOnDied -= value;    
    }

    private void Start() {
        _animator = GetComponent<Animator>();

        _animator.SetBool("isAlive", _isAlive);
    }

    public void GetHit(float damage) {
        float healthResult = _health - damage;

        _animator.SetTrigger("hit");

        if(healthResult <= 0) {
            Die();
        }

        _health = healthResult;
    }

    private void Die() {
        _health = 0;
        _isAlive = false;

        _animator.SetBool("isAlive", _isAlive);
        _EOnDied?.Invoke();

        GetComponent<Sceleton>().enabled = false;
        GetComponent<SceletonMovement>().enabled = false;
    }
}
