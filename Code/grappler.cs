using UnityEngine;

public class grappler : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInput.GroundedActions grounded;
    LineRenderer lr;
    public Vector3 grapplepoint;
    SpringJoint joint;
    public LayerMask whatIsGrappleable;
    public Transform tip, cam, player;
    public float maxDistance;
    bool canGrapple = true;

    public float maxMod = 0.8f;
    public float minMod = 0.25f;
    public float spring = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<playerMovement>().playerInput;
        grounded = playerInput.Grounded;
    }

    private void Update()
    {
        if(grounded.Grapple.ReadValue<float>() > 0 && canGrapple)
        {
            canGrapple = false;
            StartGrapple();
        }
        else if (grounded.Grapple.ReadValue<float>() == 0)
        {
            canGrapple = true;
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast( cam.position, cam.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplepoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplepoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplepoint);

            joint.maxDistance = distanceFromPoint * maxMod;
            joint.minDistance = distanceFromPoint * minMod;

            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;

            lr.positionCount = 2;
        }
    }

    public bool isGrappling()
    {
        return joint != null;
    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, tip.position);
        lr.SetPosition(1, grapplepoint);
    }

}
