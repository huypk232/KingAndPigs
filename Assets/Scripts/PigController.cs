using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask kingLayer;
    public Transform[] waypoints;

    private bool isCollidePlayer;
    private float deltaTimeCollidePlayer = 2f;
    private float attackRange = 0.5f;
    private float patrolRange = 2f;
    private Rigidbody2D _rb;
    private Animator _animator;
    private int hp;
    private static int maxHp = 2;
    private Transform destination;
    private int m_CurrentWaypointIndex;
    private float freeze = 3f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();        
        _animator = GetComponent<Animator>(); 
        hp = maxHp;
        m_CurrentWaypointIndex = 0;
        destination = waypoints[0];
    }

    void Update()
    {
        CheckInAttackRange();
        CheckInPatrolRange();
        Patrol();
    }

    private void ManualOnCollisionStay()
    {
        if(isCollidePlayer)
        {
            deltaTimeCollidePlayer -= Time.deltaTime;
            if(deltaTimeCollidePlayer <= 0)
            {
                _animator.SetTrigger("Attack");
            }
            deltaTimeCollidePlayer = 2f;
        }
    }

    private void CheckInAttackRange()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, kingLayer);
    
        if(hitEnemies.Length == 0)
        {
            isCollidePlayer = false;
            deltaTimeCollidePlayer = 2f;
            return;
        }

        isCollidePlayer = true;
        
        deltaTimeCollidePlayer -= Time.deltaTime;
        if(deltaTimeCollidePlayer <= 0)
        {
            _animator.SetTrigger("Attack");
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<KingController>().TakeDamage();
            }
            deltaTimeCollidePlayer = 2f;
        }
    }

    private void CheckInPatrolRange()
    {
        // realize player position
    }

    private void Patrol()
    {
        
        if (transform.position == destination.position) {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            destination = waypoints[m_CurrentWaypointIndex];
            freeze = 3f;
        }
        transform.position = Vector2.MoveTowards(transform.position, destination.position, 2f * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D other) {
        // if(other.gameObject.CompareTag("Player"))
        // {
        //     isCollidePlayer = true;
        //     deltaTimeCollidePlayer = 2f;
        // }
    }

    private void OnCollisionExit2D(Collision2D other) {
        // if(other.gameObject.CompareTag("Player"))
        // {
        //     isCollidePlayer = false;
        // }
    }

    

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage()
    {
        hp -= 1;
        _animator.SetTrigger("Hitted");
        if(hp <= 0)
        {
            _animator.SetTrigger("Dead");
            Destroy(gameObject, 2f);
        }
    }

}
