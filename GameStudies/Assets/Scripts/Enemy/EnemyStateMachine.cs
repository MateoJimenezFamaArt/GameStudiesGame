using System;
using System.Collections;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public enum EnemyState { Idle, Chasing, Attacking, Recovery, Dying }
    private EnemyState currentState;

    public float idleRadius = 10f;
    public float chaseSpeed = 4f;
    public float attackRange = 2f;
    public float recoveryTime = 2f;

    private Transform player;
    private Rigidbody rb;

    private Animator animator;

    public event Action OnAttack; // Event for triggering attacks
    public event Action OnChase;  // Event for chasing
    public event Action OnIdle;   // Event for idle

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(StateMachine());
        animator = GetComponent<Animator>();
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    yield return StartCoroutine(IdleState());
                    break;
                case EnemyState.Chasing:
                    yield return StartCoroutine(ChasingState());
                    break;
                case EnemyState.Attacking:
                    yield return StartCoroutine(AttackingState());
                    break;
                case EnemyState.Recovery:
                    yield return StartCoroutine(RecoveryState());
                    break;
            }
            yield return null;
        }
    }

    private IEnumerator IdleState()
    {
        while (currentState == EnemyState.Idle)
        {
            OnIdle?.Invoke(); // Notify listeners that enemy is idle
            if (Vector3.Distance(transform.position, player.position) < idleRadius)
            {
                currentState = EnemyState.Chasing;
            }
            yield return null;
        }
    }

    private IEnumerator ChasingState()
    {
        while (currentState == EnemyState.Chasing)
        {
            OnChase?.Invoke(); // Notify listeners that enemy is chasing


            Vector3 direction = (player.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * chaseSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                currentState = EnemyState.Attacking;
            }

            yield return null;
        }
    }

    private IEnumerator AttackingState()
    {
        animator.SetTrigger("Windup");
        OnAttack?.Invoke(); // Notify listeners that enemy is attacking
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f); // Assume some attack delay
        currentState = EnemyState.Recovery;
    }

    private IEnumerator RecoveryState()
    {
        animator.SetTrigger("Recovery");
        yield return new WaitForSeconds(recoveryTime);
        currentState = EnemyState.Idle;
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;
    }
}