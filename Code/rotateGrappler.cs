using UnityEngine;

public class rotateGrappler : MonoBehaviour
{
    public grappler grappler;

    Quaternion desiredRotation;
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        if (!grappler.isGrappling())
        {
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(grappler.grapplepoint - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
