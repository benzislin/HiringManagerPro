using UnityEngine;
using System.Collections;

public class CandidateMovement : MonoBehaviour {
    public float walkSpeed = 2.0f;
    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    public IEnumerator WalkToDesk(Vector3 targetPosition) {
        // 1. Start the walking animation
        if (anim != null) anim.SetBool("isWalking", true);

        // 2. Move until we are close enough to the desk
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);
            
            // Make sure they face the desk
            transform.LookAt(targetPosition); 
            
            yield return null; // Wait for the next frame
        }

        // 3. Stop walking and sit/stand idle
        if (anim != null) anim.SetBool("isWalking", false);
        
        // Face the player/camera once arrived
        transform.rotation = Quaternion.Euler(0, 180, 0); 
    }
    public void hire(bool hire)
    {
        if(hire)
        {
            anim.SetTrigger("hired");
        }
        else
            anim.SetTrigger("nothired");
    }
}