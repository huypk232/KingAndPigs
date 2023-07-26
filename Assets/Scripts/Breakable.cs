using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator DelayAwake()
    {
        yield return new WaitForSeconds(1f);
        enabled = true;
    }
}
