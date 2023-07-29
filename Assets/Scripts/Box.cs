using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject diamondPrefab;
    public GameObject jadePrefab;
    public GameObject bombPrefab;
    public GameObject lifePrefab;

    private ParticleSystem particle;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    private AudioSource audioSource;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void BrokenRandomSpawn()
    {
        int dropIndex = Random.Range(0, 4);
        float velocityX = Random.Range(0.1f, 0.5f);
        float velocityY = Random.Range(0.1f, 0.5f);
        float randomSpeed = Random.Range(1f, 10f);
        Vector3 randomVelocity = new Vector3(velocityX, velocityY, 0);
        randomVelocity.Normalize();
        randomVelocity *=  randomSpeed;

        // refactor
        if(dropIndex == 0)
        {
            GameObject newDiamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity);
            newDiamond.GetComponent<Rigidbody2D>().velocity = randomVelocity;
        } else if (dropIndex == 1)
        {
            GameObject newJade = Instantiate(jadePrefab, transform.position, Quaternion.identity);
            newJade.GetComponent<Rigidbody2D>().velocity = randomVelocity;
        } else if (dropIndex == 2)
        {
            GameObject newBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            newBomb.GetComponent<Rigidbody2D>().velocity = randomVelocity;
        } else if (dropIndex == 3)
        {
            GameObject newLife = Instantiate(lifePrefab, transform.position, Quaternion.identity);
            newLife.GetComponent<Rigidbody2D>().velocity = randomVelocity;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // if(other.collider.gameObject.GetComponent<KingController>() && other.contacts[0].normal.y > 0.5f)
        // {
        //     StartCoroutine(Break());
        // }
    }

    private IEnumerator Break()
    {
        particle.Play();
        if(audioSource != null)
            audioSource.Play();
        boxCollider.enabled = false;
        sprite.enabled = false;
        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject, 5f);
    }

    public void Broken()
    {
        BrokenRandomSpawn();
        StartCoroutine(Break());
    }
}
