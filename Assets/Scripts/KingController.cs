using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public LayerMask boxLayer;

    [SerializeField] float speed = 4f;
    [SerializeField] float _jumpForce = 5f;
    [SerializeField] bool inFirstRoom = false;

    private float lastVerticalVelocity = 0f;
    private bool falling = false;

    private bool _isGround = false;

    private Animator _animator;
    private Rigidbody2D _rb;

    private int _hp;
    private int _maxHp = 3;
    private float attackRange = 1f;
    private bool _inDoorTrigger = false;
    private Door _door;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _rb = GetComponent<Rigidbody2D>();
        _hp = _maxHp;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        Move();
        Jump();
        Smash();
        ManualOnTriggerStay();
    }

    private void CheckGround()
    {
        
        if(!_isGround && _rb.velocity.y == 0)
        {
            _isGround = true;
            _animator.SetBool("IsGround", true);
        }
    }

    private void ManualOnTriggerStay()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && _inDoorTrigger)
        {
            if(_door.IsLastDoor())
            {
                GameManager.instance.CompleteStage();
            } else {
                _animator.SetTrigger("GoIn");
                StartCoroutine(GoIn(_door.currentRoom, _door.destination));
            }
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0, 0);
        if(horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

        } else if(horizontal > 0)
        {

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0, 0f));
        }
        _animator.SetFloat("Speed", Mathf.Abs(horizontal));
        _rb.velocity = new Vector2(horizontal * speed, _rb.velocity.y);
    }

    private void Jump()
    {
        if(_isGround)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _rb.velocity = Vector2.up * _jumpForce;
                _isGround = false;
                _animator.SetBool("IsGround", false);
            }
        } else {
            if(_rb.velocity.y == 0)
            {
                if(lastVerticalVelocity > 0)
                {
                    falling = true;
                    _animator.SetBool("Falling", true);
                } else 
                {
                    falling = false;
                    _isGround = true;
                    _animator.SetBool("IsGround", true);
                    _animator.SetBool("Falling", false);
                }

            } else {
                lastVerticalVelocity = _rb.velocity.y;
            }
        }
    }

    // refactor
    private void Smash()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            _animator.SetTrigger("Attack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<PigController>().TakeDamage();
            }

            Collider2D[] hitBoxes = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, boxLayer);

            foreach(Collider2D box in hitBoxes)
            {
                box.GetComponent<Box>().Broken();
            }
        }
    }

    public void TakeDamage()
    {
        _hp -= 1;
        _animator.SetTrigger("Hitted");
        if (_hp < 0)
        {
            _animator.SetTrigger("Dead");
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        if(!_isGround && _rb.velocity.y == 0)
        {
            _isGround = true;
            _animator.SetBool("IsGround", true);
        }  
        if(!inFirstRoom)
        {
            _animator.SetTrigger("GoOut");
        } else 
        {
            inFirstRoom = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // if(other.gameObject.CompareTag("Diamond"))
        // {
        //     GameManager.instance.ColectDiamond();
        //     Destroy(other.gameObject);
        // } else if(other.gameObject.CompareTag("Jade"))
        // {
        //     GameManager.instance.ColectJade();
        //     Destroy(other.gameObject);
        // }
    }


    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<Door>(out Door door))
        {
            _inDoorTrigger = true;
            _door = door;
            door.Open();
        }
    }

    private IEnumerator GoIn(GameObject currentRoom, GameObject destination)
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.GoToRoom(currentRoom, destination);
        yield return new WaitForSeconds(0.5f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // if(other.gameObject.TryGetComponent<Door>(out Door door))
        // {
        //     if(_doorIn)
        //     {
        //         _animator.SetTrigger("GoIn");
        //         StartCoroutine(GoIn(door.currentRoom, door.destination));
        //     }
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<Door>(out Door door))
        {
            _inDoorTrigger =false;
            _door = null;
            door.Close();
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.color = new Color(1, 1, 0, 0.75f);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
