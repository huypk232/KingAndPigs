using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private King player;
    [SerializeField] private Bounds _bounds;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
