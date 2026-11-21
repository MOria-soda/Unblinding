using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CreateEchoController : MonoBehaviour
{
    public float echoSpeed = 12f;
    public float maxRadius = 15f;
    public float cooldownTime = 2f;

    public Transform caneTip;
    public LayerMask echolayer;

    private float nextEchoTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryEcho();
        }
    }

    void TryEcho()
    {
        if (Time.time < nextEchoTime)
        {
            return;//sound checker is cooling down.
        }

        nextEchoTime = Time.time + cooldownTime;

        Vector3 origin = caneTip != null ? caneTip.position : transform.position;
        StartCoroutine(ExpandEcho(origin));
    }

    IEnumerator ExpandEcho(Vector3 origin)
    {
        float radius = 0f;
        HashSet<EchoTarget> detectedTargets = new HashSet<EchoTarget>();
        while (radius < maxRadius)
        {
            radius = Mathf.Min(radius + echoSpeed * Time.deltaTime, maxRadius);
            Collider[] hits = Physics.OverlapSphere(origin, radius, echolayer, QueryTriggerInteraction.Ignore);

            foreach (Collider hit in hits)
            {
                // Handle the objects that are detected by the echo
                EchoTarget target = hit.GetComponent<EchoTarget>();

                if (target == null)
                    continue;

                if (detectedTargets.Add(target))
                {
                    // Handle the newly detected target
                    target.CreateEchoSnapshot();
                }
            }
            yield return null; // Wait for the next frame
        }
    }

}
