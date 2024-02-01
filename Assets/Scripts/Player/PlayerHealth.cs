using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour {
    [SerializeField] private bool _isAlive;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;

    [SerializeField] private Animator _animator;

    public delegate void OnDied();

    private event OnDied _EOnDied;
    public event OnDied EOnDied {
        add => _EOnDied += value;
        remove => _EOnDied -= value;
    }

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent<Food>(out Food food)) {
            GetHeal(food.regenerationEffect);
            food.PickUp();
        }
    }

    public void GetHit(float damage) {
        float healthResult = _health - damage;

        _animator.SetTrigger("hit");

        if(healthResult <= 0) {
            _health = 0;

            _EOnDied?.Invoke();
        }


        _health = healthResult;
    }

    public void GetHeal(float heal) { 
        _health = _health + heal >= _maxHealth ? _maxHealth : _health + heal;
    }
}
