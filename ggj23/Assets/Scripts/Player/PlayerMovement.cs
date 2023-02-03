using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float plrSpeed;

    // values
    private Vector3 _forward, _right;   // since vectors are different from world
    
    // components
    private Camera _mainCam;

    private void Start()
    {
        // define main camera
        _mainCam = Camera.main;
        
        // setup forward vector
        _forward = _mainCam.transform.forward;
        _forward.y = 0f;
        _forward = Vector3.Normalize(_forward);
        
        // setup right vector
        _right = Quaternion.Euler(new Vector3(0, 90f, 0)) * _forward;
    }

    private void Update()
    {
        if (Input.anyKey) Move();
    }

    private void Move()
    {
        // set direction to input.
        Vector3 dir = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
        Vector3 rightMovement = _right * (plrSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey"));
        Vector3 upMovement = _forward * (plrSpeed * Time.deltaTime * Input.GetAxis("VerticalKey"));
        
        // creates the direction for the player to move in ortho space
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        transform.forward = heading;
        
        // make movement happen
        transform.position += rightMovement;
        transform.position += upMovement;
    }
}
