using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustAttack : StateMachineBehaviour
{

    Character character;
    [SerializeField] float damage;
    [SerializeField] Vector2 attackPlacement;
    [SerializeField] Vector2 hitbox;
    [SerializeField] float beginTime, endTime, tickRate;

    float timer = 0;
    int frameTimer = 0;

    GameObject enemy;

    Rigidbody2D rb;
    float gravity;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character = animator.GetComponent<Character>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        character.attackPlacement.localPosition = attackPlacement;
        character.hitboxSize = hitbox;

        character._flipable = false;
        character._movable = false;
        character._attacking = true;

        rb = character.GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        timer = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;

        Collider2D hit = Physics2D.OverlapBox(character.attackPlacement.position, character.hitboxSize, 0, LayerMask.GetMask("Enemy"));

        timer += Time.deltaTime;
        frameTimer++;

        if (timer > beginTime && frameTimer % tickRate == 0 && timer < endTime)
        {
            if (hit != null)
            {
                if (hit.CompareTag("Enemy") && enemy != null)
                {

                    hit.GetComponent<Enemy>().TakeDamage(damage, 0.1f);
                    Debug.Log("thrust attack hit enemy");

                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character._flipable = true;
        character._movable = true;
        IEnumerator endAttack = character.EndAttack();
        character.StartCoroutine(endAttack);

        rb.gravityScale = character.gravityScale;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
