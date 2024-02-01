using UnityEngine;

public class Food : MonoBehaviour, IPickable {
    [SerializeField] private float _regenerationEffect;

    public float regenerationEffect {
        get => _regenerationEffect;
    }

    public void PickUp() { 
        Destroy(gameObject);
    }
}
