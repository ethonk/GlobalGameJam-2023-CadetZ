using UnityEngine;

public class IsoMovement : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f;
    public float turnSpeed = 360f;

    [Header("States")]
    public bool aimInfluenced;
    
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
        if (aimInfluenced) LookAimInfluenced();

        // move on input
        if (aimInfluenced)
        {
            if (Input.GetKey(KeyCode.W)) MoveAimInfluenced();
            if (Input.GetKey(KeyCode.S)) MoveAimInfluenced(-1f);
            if (Input.GetKey(KeyCode.A)) MoveAimInfluenced(-1f, false);
            if (Input.GetKey(KeyCode.D)) MoveAimInfluenced(1f, false);
        }
        else if (Input.anyKey)
        {
            Move();
        }
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
    
    private void MoveAimInfluenced(float dir = 1f, bool vertical = true)
    {
        if (vertical)
            _rb.MovePosition(transform.position + transform.forward * (dir * (speed * .75f * Time.deltaTime)));
        else
            _rb.MovePosition(transform.position + transform.right * (dir * (speed * .75f * Time.deltaTime)));
    }

    private void LookAimInfluenced()
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

            // slerp to that rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, look,
                turnSpeed * Time.deltaTime);
        }
    }
}
