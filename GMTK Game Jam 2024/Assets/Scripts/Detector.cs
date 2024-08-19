using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    private Collider2D collider;

    public UnityAction<Transform> OnPlayerDetected;
    public UnityAction OnPlayerLost;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
            OnPlayerDetected?.Invoke(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            OnPlayerLost?.Invoke();
    }
}
