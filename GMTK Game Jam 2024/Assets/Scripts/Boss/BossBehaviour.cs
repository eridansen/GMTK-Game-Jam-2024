using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Unity.Mathematics;
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

    private float invencibilityDeadline = 0; // The counter for the invincibility time
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
        if (inIntro || invencibilityDeadline > Time.time) return; // If the player is invincible, return

        invencibilityDeadline = Time.time + invincibilityTime; // Set the invincibility counter to the invincibility time
        AttemptDisplayHurt();
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

        if (combatState == BOSS_SAW)
        {
            rigidBody.velocity = sawVelocity;
        }

        isGrounded = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        WallHit();
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
        StartCoroutine(nameof(CombatRoutine));
    }
    [SerializeField] private float idleDurationBase = 0.5f;
    [SerializeField] private float idleDurationHealth = 0.5f;
    [SerializeField] private float wiggleForce = 10;
    [SerializeField] private float wiggleInterval = 0.3f;

    [Header("Spray settings")]
    [SerializeField] private SprayedCancer sprayedPrefab;
    [SerializeField] private float sprayDelay = 0.2f;
    [SerializeField] private float sprayAmount = 2;
    [SerializeField] private GameObject clawCollider;

    [Header("Saw settings")]
    [SerializeField] private float sawDuration = 3f;
    [SerializeField] private float sawSpeed = 16f;
    [SerializeField] private GameObject sawCollider;

    string combatState = BOSS_IDLE;
    Vector2 sawVelocity;
    IEnumerator CombatRoutine()
    {
        var idleTransitions = new[]{
            BOSS_CRAB,
            BOSS_SAW_START,
        };

        var lastState = "";
        while (_currentHealth > 0)
        {
            if (lastState == combatState)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            lastState = combatState;
            ChangeAnimationState(combatState);
            switch (combatState)
            {
                default:
                    break;
                case BOSS_IDLE:
                    StartCoroutine(nameof(WiggleRoutine));
                    yield return new WaitForSeconds(idleDurationBase + idleDurationHealth * (_currentHealth / _maxHealth));
                    combatState = idleTransitions[UnityEngine.Random.Range(0, idleTransitions.Length)];
                    break;
                case BOSS_CRAB:
                    yield return new WaitForSeconds(sprayDelay);
                    Spray();
                    clawCollider.SetActive(true);
                    break;
                case BOSS_SAW:
                    yield return new WaitForSeconds(sawDuration);
                    sawCollider.SetActive(false);
                    combatState = BOSS_IDLE;
                    rigidBody.gravityScale = 1;
                    break;
            }
        }
    }
    IEnumerator WiggleRoutine()
    {
        while (combatState == BOSS_IDLE)
        {
            if (isGrounded)
            {
                rigidBody.velocity = UnityEngine.Random.Range(-wiggleForce, wiggleForce) * Vector2.right;
            }
            yield return new WaitForSeconds(wiggleInterval);
        }
    }

    void AttemptDisplayHurt()
    {
        if (combatState != BOSS_IDLE)
        {
            return;
        }
        ChangeAnimationState(BOSS_HURT);
    }

    void BossCrabEnd()
    {
        clawCollider.SetActive(false);
        combatState = BOSS_IDLE;
    }

    float[] validSawAngles = new[] {
        45f,135f
    };
    void BossSawStart()
    {
        combatState = BOSS_SAW;
        var angle = Mathf.PI * validSawAngles[UnityEngine.Random.Range(0, validSawAngles.Length)] / 360;
        sawVelocity = new Vector2(
            Mathf.Cos(angle),
            Mathf.Cos(angle)
        ) * sawSpeed;
        rigidBody.gravityScale = 0;
        sawCollider.SetActive(true);
    }

    float shiftDelay;
    void WallHit()
    {
        if (combatState != BOSS_SAW)
        {
            return;
        }
        sawVelocity = new Vector2(sawVelocity.y, -sawVelocity.x);
    }

    private List<SprayedCancer> _attackersList = new List<SprayedCancer>();
    private void Spray()
    {
        for (int i = 0; i < sprayAmount; i++)
        {
            var attacker = ObjectPooler.ProvideObject(sprayedPrefab, transform.position,
                sprayedPrefab.transform.rotation) as SprayedCancer;

            AddListender(attacker);
        }
    }
    private void AddListender(SprayedCancer sprayedCancer)
    {
        _attackersList.Add(sprayedCancer);
        sprayedCancer.OnPlayerSpotted += HiveAttack;
        sprayedCancer.OnDie += RemoveListener;
    }
    public void RemoveListener(SprayedCancer listener)
    {
        listener.OnPlayerSpotted -= HiveAttack;
        _attackersList.Remove(listener);
    }

    private void HiveAttack(Transform attackTarget)
    {
        foreach (var attacker in _attackersList)
            attacker.StartAttacking(attackTarget);
    }

    private void Die()
    {
        var colliders = new Collider2D[rigidBody.attachedColliderCount];
        rigidBody.GetAttachedColliders(colliders);
        foreach (var item in colliders)
        {
            item.enabled = false;
        }
        rigidBody.simulated = false;

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
        ChangeAnimationState(BOSS_IDLE);
    }
    #endregion
}
