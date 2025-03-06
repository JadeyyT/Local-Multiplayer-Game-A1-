using UnityEngine;

public class Player2Slash : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("Slash");
        }
    }
}
