using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField] private Transform gun;

    private Vector3 origin;
    private Vector3 dir;

    [SerializeField] private float rayDistance;

    private bool isDestructible;

    private RaycastHit hit;

    [SerializeField]private Vector3 offset;


    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            //origin = gun.position + offset; //local
            origin = gun.TransformPoint(offset);//global

            dir = gun.forward;

            //Physics.OverlapSphere(origin, 5f);

            if (Physics.Raycast(origin, dir, out hit, rayDistance))
            {
                isDestructible = hit.collider.CompareTag("Destructible") ? true : false; 
            }
        }

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isDestructible ? Color.green : Color.red;

        //Gizmos.DrawSphere(origin, 5f);

        Gizmos.DrawLine(origin, hit.point);

    }



}
