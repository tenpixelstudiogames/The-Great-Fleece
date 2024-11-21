using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Player : MonoBehaviour
{
    private NavMeshAgent agent;
   
    private Vector3 destination;

    [SerializeField]
    private Animator playerAniamtor;
    
   private void Awake()
   {
        

   }
    // Start is called before the first frame update
    void Start()
    {
       agent = GetComponent<NavMeshAgent>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerAniamtor.SetBool("Idle", false);

            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(rayOrigin,out hitInfo))
            {
                agent.SetDestination(hitInfo.point);
                destination = hitInfo.point;
            }
        }
        float distance = Vector3.Distance(transform.position, destination);
        if (distance < 1.0f)
        {
            Debug.Log("Idling is going to be true");
            playerAniamtor.SetBool("Idle", true);
        }
    }
}
