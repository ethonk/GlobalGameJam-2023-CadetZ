using UnityEngine;

public class IsoMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float speed = 5f;
    [SerializeField] private float turnSpeed = 360f;

    [Header("States")]
    [SerializeField] private bool aimInfluenced;
    
    // values / references
    private Vector3 _plrInput;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        _plrInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Move()
    {
        _rb.MovePosition(transform.position + transform.forward * (_plrInput.magnitude * (speed * Time.deltaTime)));
    }

    private void Look()
    {
        // don't rotate if no input or influenced by aim
        if (_plrInput == Vector3.zero || aimInfluenced) return;
        
        // create a custom matrix for iso
        var isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        // change up the input relative to the iso matrix
        var relativeInput = isoMatrix.MultiplyPoint3x4(_plrInput);
        
        var relPos = (transform.position + relativeInput - transform.position);
        var rotateTowards = Quaternion.LookRotation(relPos, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTowards,
            turnSpeed * Time.deltaTime);
    }
}
