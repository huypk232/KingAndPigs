using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughPaltform : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _boxCollider.enabled = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(Input.GetAxis("Vertical") < 0 && Input.GetKey(KeyCode.LeftShift))
        {
            _boxCollider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }
}
