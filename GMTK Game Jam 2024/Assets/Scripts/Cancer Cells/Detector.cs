using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Detector : MonoBehaviour
{
    public UnityAction<Transform> OnPlayerDetected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            OnPlayerDetected?.Invoke(playerMovement.transform);
    }
}
