using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScaleSettings", menuName = "Player/Player Scale Settings")]
public class PlayerScaleSettings : ScriptableObject
{
    [Header("Player Scale Settings")]
    public string sizeName;
    public Vector2 playerScale;    
    public float gravityScale;

    [Header("Player Movement Settings")]
    public float walkSpeed;
    public float _sprintSpeed;

    [Header("Player Jump Settings")]
    public float jumpSpeed;
    public float fallSpeed;
    public float _jumpVelocityFalloff;

    [Header("Player Wall Jump Settings")]
    public float wallSlideSpeed;
    public Vector2 wallJumpingPower;
    
    [Header("Player Dash Settings")]
    public float dashPower;
    public float dashDuration;
    public float dashCooldown;


    [Header("Player Attack Settings")]
    public float attackPower;

}
