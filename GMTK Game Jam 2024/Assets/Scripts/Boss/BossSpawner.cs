using UnityEngine;
using UnityEngine.UI;
public class BossSpawner : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [Header("Spawn Settings")]
    [SerializeField] private float delay;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private BossBehaviour prefab;
    [SerializeField] private Image _bossBar;

    public void SetBossBar(Image bossBar)
    {
        _bossBar = bossBar;
    }
    
    void Awake()
    {
        Invoke(nameof(PopPod), delay);
    }

    void PopPod()
    {
        animator.Play("Boss Pod");
    }

    void BossAnimDrop()
    {
        if (!enabled)
        {
            return;
        }
        animator.speed = 0;

        var instance = Instantiate(prefab, spawnPosition.position, spawnPosition.rotation, null);
        _bossBar.transform.parent.gameObject.SetActive(true);
        instance.SetHealthBar(_bossBar);
        enabled = false;
    }
}
