using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAtack : MonoBehaviour {
    [SerializeField] private bool _canAtack;

    [SerializeField] private int _atacksCount;
    [SerializeField] private int _attackMouseButton;
    
    [SerializeField] private float _damage;
    [SerializeField] private float _atackDistance;
    [SerializeField] private float _attackReloadTime;

    [SerializeField] private Transform _weaponPosition;

    [SerializeField] private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetMouseButton(_attackMouseButton) && _canAtack)
            PlayRandomAtackAnimation();
    }

    private void PlayRandomAtackAnimation() {
        int randomAtackId = Random.Range(1, _atacksCount+1);

        _animator.SetInteger("atackId", randomAtackId);
        _canAtack = false;
    }

    private void Attack() {
        List<IHitble> hitbleObjects = GetEnemiesInAtackRange();

        foreach (IHitble hited in hitbleObjects) {
            hited.GetHit(_damage);
        }

        _animator.SetInteger("atackId", 0);
        StartCoroutine(nameof(ReloadAttack));
    }

    private List<IHitble> GetEnemiesInAtackRange() {
        int gazeDirection = GetGazeDirection();

        RaycastHit2D[] hitedObjects = Physics2D.RaycastAll(_weaponPosition.position, Vector2.right*gazeDirection, _atackDistance);

        List<IHitble> hitbleObjects = new List<IHitble>();

        foreach(var hitedObjectInfo in hitedObjects) {
            GameObject hitedObject = hitedObjectInfo.collider.gameObject;

            if (hitedObject.TryGetComponent<Obstacle>(out Obstacle obstacle)
                || hitedObject.TryGetComponent<Ground>(out Ground ground))
                break;

            if(hitedObject.TryGetComponent<IHitble>(out IHitble hited)) {
                hitbleObjects.Add(hited);
            }
        }

        return hitbleObjects;
    }

    private int GetGazeDirection() => gameObject.transform.localScale.x > 0 ? 1 : -1;

    private IEnumerator ReloadAttack() {
        yield return new WaitForSeconds(_attackReloadTime);

        _canAtack = true;
    }
}

