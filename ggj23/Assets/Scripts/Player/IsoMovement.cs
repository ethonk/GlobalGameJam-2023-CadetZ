using UnityEngine;

public class IsoMovement : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f;
    public float turnSpeed = 360f;

    [Header("States")]
    public bool aimInfluenced;

    [Header("References")]
    [SerializeField] private Transform playerModel;
    
    // values / references
    private Vector3 _forward, _right;   // augmented forward and right for iso
    private Rigidbody _rb;
    private Camera _mainCam;
    private int _raycastLayer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
        
        _raycastLayer = LayerMask.GetMask("AimCollision");
        
        // setup isometric values
        _forward = _mainCam.transform.forward;
        _forward.y = 0;
        _forward = Vector3.Normalize(_forward);
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
    }

    private void FixedUpdate()
    {
        // process look
        Look();
        
        // move on input
        if (Input.anyKey) Move();
    }

    private void Move()
    {
        Vector3 rightMovement = _right * (speed * Input.GetAxis("Horizontal"));
        Vector3 forwardMovement = _forward * (speed * Input.GetAxis("Vertical"));

        Vector3 heading = Vector3.Normalize(rightMovement + forwardMovement);
        
        // Set new position
        Vector3 newPosition = transform.position;
        newPosition += rightMovement;
        newPosition += forwardMovement;

        // Smoothly move the new position
        transform.forward = heading;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
    }

    private void Look()
    {
        // if influenced by aim, look towards aim location
        if (aimInfluenced)
        {
            RaycastHit hit;
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        
            // attempt raycast
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _raycastLayer))
            {
                // adjust the hit point to ignore the y-axis
                Vector3 newHit = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                // using hit point, get direction
                Vector3 dir = (newHit - transform.position).normalized;
                Quaternion look = Quaternion.LookRotation(dir);

                // slerp to that rotation (only the model)
                playerModel.rotation = Quaternion.Slerp(playerModel.rotation, look,
                    turnSpeed * Time.deltaTime);
            }
        }

        else
        {
            // slerp to that rotation (only the model)
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, transform.rotation,
                turnSpeed * Time.deltaTime);
        }
    }
}
