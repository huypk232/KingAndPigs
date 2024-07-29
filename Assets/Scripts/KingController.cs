using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KingController : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public LayerMask boxLayer;
    public GameObject life;

    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool inFirstRoom = false;

    private static GameObject _healthBar;
    private float lastVerticalVelocity = 0f;
    private bool falling = false;
    private bool changingRoom = false;
    private bool _isGround = false;

    private Animator _animator;
    private Rigidbody2D _rb;
    private GroundSensor groundSensor;

    private static int _hp;
    private int _maxHp = 3;
    private float attackRange = 1f;
    private bool _inDoorTrigger;
    private Door _door;

    private static Vector3[] lifeCanvasPos = new Vector3[3];


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("Ground Sensor").GetComponent<GroundSensor>();
        _hp = _maxHp;
        _healthBar = GameObject.Find("/Canvas/Health Bar");
        attackPoint = transform.Find("Attack Point").transform;
        int index = 0;
        foreach (Transform lifePos in _healthBar.transform)
        {
            lifeCanvasPos[index] = new Vector3(lifePos.position.x, lifePos.position.y, lifePos.position.z);
            index += 1;
        }
    }

    void Update()
    {
        CheckGround();
        if(!changingRoom)
        {
            Move();
            Jump();
            Smash();
        }
        
        ManualOnTriggerStay();
    }

    private void CheckGround()
    {
        if (!_isGround && groundSensor.State()) {
            _isGround = true;
            _animator.SetBool("IsGround", _isGround);
        }

        if(_isGround && !groundSensor.State()) {
            _isGround = false;
            _animator.SetBool("IsGround", _isGround);
        }
    }

    // move call trigger stay manually in update
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
                _animator.SetTrigger("GoOut");
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
        _animator.SetFloat("VerticalVelocity", _rb.velocity.y);
        if(Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            _animator.SetTrigger("Jump");
            _isGround = false;
            _animator.SetBool("IsGround", _isGround);
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
        
        // todo apply fall animation

        // if(_isGround)
        // {
        //     if(Input.GetKeyDown(KeyCode.Space))
        //     {
        //         _rb.velocity = Vector2.up * _jumpForce;
        //         _isGround = false;
        //         _animator.SetBool("IsGround", false);
        //         _animator.SetBool("Falling", false);
        //     }
        // } else {
        //     if(Mathf.Abs(_rb.velocity.y) <= 0.000001f) // approximate 0
        //     {
        //         Debug.Log(lastVerticalVelocity);
        //         Debug.Log("Approximate");
        //         if(lastVerticalVelocity > 0)
        //         {
        //             Debug.Log("Jump");
        //             falling = true;
        //             _animator.SetBool("Falling", true);
        //         } else if (lastVerticalVelocity < 0)
        //         {
        //             Debug.Log("Fall");
        //             // falling = false;
        //             _isGround = true;
        //             _animator.SetBool("IsGround", true);
        //             _animator.SetBool("Falling", false);
        //         }
                
        //     }
        //     lastVerticalVelocity = _rb.velocity.y;
        // }
    }

    // todo refactor
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
        _animator.SetTrigger("Hitted");
        _healthBar.transform.GetChild(_hp - 1).gameObject.SetActive(false);
        _hp -= 1;

        if (_hp <= 0)
        {
            _animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    public void Heal()
    {
        if(_hp >= 3){
            return;
        }
        _healthBar.transform.GetChild(_hp).gameObject.SetActive(true);
        _hp += 1;
    }

    public void Respawn()
    {
        _hp = _maxHp;
        foreach(Transform transform in _healthBar.transform)
        {
            transform.gameObject.SetActive(true);
        }
        _animator.SetTrigger("Respawn");
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
            StartCoroutine(GoOut());
        } else 
        {
            inFirstRoom = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out Door door))
        {
            _inDoorTrigger = true;
            _door = door;
            door.Open();
        }
    }

    private IEnumerator GoIn2(GameObject currentRoom, GameObject destination)
    {
        changingRoom = true;
        yield return new WaitForSeconds(0.5f);
        changingRoom = false;
        GameManager.instance.GoToRoom(currentRoom, destination);
    }
    
    private IEnumerator GoIn(GameObject currentRoom, GameObject destination)
    {
        changingRoom = true;
        yield return new WaitForSeconds(0.5f);
        transform.position = _door.linkedDoor.transform.position;
        changingRoom = false;
        // GameManager.instance.GoToRoom(currentRoom, destination);
    }

    private IEnumerator GoOut()
    {
        changingRoom = true;
        yield return new WaitForSeconds(0.5f);
        changingRoom = false;
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
