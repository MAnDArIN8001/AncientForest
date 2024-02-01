using UnityEngine;

public class Box : MonoBehaviour, IHitble {
    [SerializeField] private float _health;

    [SerializeField] private GameObject[] _lootVariants;

    public void GetHit(float damage) {
        if (_health - damage <= 0) {
            InstantiateRandomLoot();
            Destroy(gameObject);
        }
        _health -= damage;
    }

    private void InstantiateRandomLoot() {
        GameObject lootToDrop = _lootVariants[Random.Range(0, _lootVariants.Length)];

        Instantiate(lootToDrop, transform.position, Quaternion.identity);
    }
}
