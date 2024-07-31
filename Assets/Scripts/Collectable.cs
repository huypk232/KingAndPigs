using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            if(gameObject.CompareTag("Diamond") || gameObject.CompareTag("Jade"))
            {
                GameManager.instance.ColectDiamond();
            } else if(gameObject.CompareTag("Life"))
            {
                if(other.gameObject.TryGetComponent<King>(out King king))
                {
                    king.Heal();
                }
            }
            Destroy(gameObject);
        }
    }
}
