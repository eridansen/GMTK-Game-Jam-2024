using System;
using System.Collections;
using Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour, IDamageable
{
    #region Health
    [Header("Health Stats")]
    [SerializeField] private float _currentHealth = 100; // The player's health
    [SerializeField] private float _maxHealth = 100; // The player's maximum health

    [Header("Health Bar Settings")]
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _healthBarSpeed = 0.05f;


    [Header("Hurt Settings")]
    [SerializeField] private AudioClip[] _hurtSounds; // The player's hurt sounds
    [SerializeField] private float invincibilityTime = 1f; // The time the player is invincible after being hit
    [SerializeField] private Vector2 knockbackForce = new Vector2(15f, 5f); // The force of the knockback

    [HideInInspector] public Vector3 enemyPosition = Vector3.zero; // The position of the enemy that hit the player 

    private float invincibilityCounter = 0; // The counter for the invincibility time
    #endregion

    public void SetHealthBar(Image bar)
    {
        _healthBar = bar;
    }

    private void Start()
    {
        _currentHealth = _maxHealth; // Set the player's health to the maximum health
        ChangeAnimationState(BOSS_DROP);
    }

    public void Damage(float damageAmount)
    {
        if (inIntro || invincibilityCounter > 0) return; // If the player is invincible, return

        invincibilityCounter = invincibilityTime; // Set the invincibility counter to the invincibility time
        ChangeAnimationState(BOSS_HURT);
        AudioManager.Instance.PlayRandomSoundFXClip(_hurtSounds, transform, 0.5f); // Play a random hurt sound

        _currentHealth -= damageAmount;
        StartCoroutine(UpdateHealthBarUI());
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    public IEnumerator UpdateHealthBarUI()
    {

        float initValue = _healthBar.fillAmount;
        float percent = _currentHealth / _maxHealth;


        float iterator = 0;

        while (iterator < 1)
        {
            _healthBar.fillAmount = Mathf.Lerp(initValue, percent, iterator);
            iterator += 0.05f;
            yield return new WaitForSeconds(_healthBarSpeed);
        }

        _healthBar.fillAmount = percent;
    }

    #region behaviour
    bool inIntro = true;
    bool isGrounded = false;
    [Header("Behaviour")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float introFallSpeed;
    [SerializeField] private float deathEndDelay;
    void FixedUpdate()
    {
        if (inIntro)
        {
            IntroBehaviour();
            return;
        }

        isGrounded = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            var groundDot = Vector2.Dot(Vector2.up, contact.normal);
            if (groundDot > 0.7f)
            {
                isGrounded = true;
            }
        }
    }

    private void IntroBehaviour()
    {
        if (isGrounded)
        {
            ChangeAnimationState(BOSS_DROP_END);
        }
        else
        {
            rigidBody.velocity = Vector2.down * introFallSpeed;
        }
    }

    void BossDropEnd()
    {
        inIntro = false;
        ChangeAnimationState(BOSS_IDLE);
    }

    private void Die()
    {
        var colliders = new Collider2D[rigidBody.attachedColliderCount];
        rigidBody.GetAttachedColliders(colliders);
        foreach (var item in colliders)
        {
            item.enabled=false;
        }
        rigidBody.simulated=false;

        ChangeAnimationState(BOSS_DEATH);
        _healthBar.transform.parent.gameObject.SetActive(false);
        Invoke(nameof(End), deathEndDelay);
    }
    void End()
    {
        SceneLoader.Instance.LoadSceneWithoutLoadingScreen(Constants.Scenes.Credits);
    }
    #endregion

    #region animations
    [Header("Animator")]
    [SerializeField] private Animator animator;
    const string BOSS_DROP = "Boss Drop";
    const string BOSS_DROP_END = "Boss Drop End";
    const string BOSS_IDLE = "Boss Still";
    const string BOSS_CRAB = "Boss Crab";
    const string BOSS_SAW_START = "Boss SawStart";
    const string BOSS_SAW = "Boss Saw";
    const string BOSS_HURT = "Boss Hurt";
    const string BOSS_DEATH = "Boss Death";
    private string currentState = "";
    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
    void BossHurtEnd()
    {

    }
    void BossCrabEnd()
    {

    }
    void BossSawStart()
    {

    }
    #endregion
}
