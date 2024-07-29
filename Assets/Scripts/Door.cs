using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;

    public GameObject destination;
    public GameObject currentRoom;
    public GameObject kingPig;

    [SerializeField] public bool lastDoor;
    public GameObject linkedDoor;
    

    void Start()
    {

        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        _animator.SetTrigger("Open");
    }

    public void Close()
    {
        _animator.SetTrigger("Close");
    }

    public bool IsLastDoor()
    {
        return lastDoor;
    }

    private IEnumerator GoIn()
    {
        yield return new WaitForSeconds(0.5f);
        // _boxCollider.enabled = true;
    }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     if(other.gameObject.CompareTag("Player"))
    //     {
    //         Open();
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D other) {
    //     if(other.gameObject.CompareTag("Player"))
    //     {
    //         Close();
    //     }
    // }

    // private void OnCollisionStay2D(Collision2D other) {
    //     if(other.gameObject.CompareTag("Player") && !lastDoor)
    //     {
    //         if(Input.GetKeyDown(KeyCode.UpArrow))
    //         {
    //             _animator.SetTrigger("GoIn");
    //         }
    //     }
    // }

    
}
