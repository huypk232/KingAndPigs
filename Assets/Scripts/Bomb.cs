using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private LayerMask kingLayer;
    private float explodeRange = 1f;
    private Animator _animator;
    private float _timer = 2f;
    private bool _exploded = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("BombOn");
    }

    void Update()
    {
        if(_timer <= 0f)
        {
            if(!_exploded) {
                Explode();
                _exploded = true;
            }
        } else {
            _timer -= Time.deltaTime;
        }
    }

    private void Explode()
    {
        _animator.SetTrigger("Explode");
        Destroy(gameObject, 0.25f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explodeRange, kingLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.TryGetComponent<KingController>(out KingController king)){
                king.TakeDamage();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75f);
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
}
