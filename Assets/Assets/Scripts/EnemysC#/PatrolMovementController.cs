using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovementController : MonoBehaviour
{
    [SerializeField] private Transform[] checkpointsPatrol;
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask layerInteraction;
    private Transform currentPositionTarget;
    private int patrolPos = 0;
    private float fastVelocity = 0f;
    private float normalVelocity;

    private void Start() {
        currentPositionTarget = checkpointsPatrol[patrolPos];
        transform.position = currentPositionTarget.position;

        normalVelocity = velocityModifier;
        fastVelocity = velocityModifier * 2.5f;
    }

    private void Update() {
        CheckNewPoint();

        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);
    }
    //Funcion para revisar la nueva posicion
    private void CheckNewPoint(){
        //Si el valor absoluto de la distancia(magnitude) de la posicion del escorpion menos la posicion del objetivo actual es menor a 0.25
        if(Mathf.Abs((transform.position - currentPositionTarget.position).magnitude) < 0.25){
            transform.position = currentPositionTarget.position;
            //se suma 1 patrolPos y la cadena de las posiciones a pasa al siguiente 
            patrolPos = patrolPos + 1 == checkpointsPatrol.Length? 0: patrolPos+1;
            currentPositionTarget = checkpointsPatrol[patrolPos];
            CheckFlip(myRBD2.velocity.x);
        }
        //Resto las posicioes de mi objetivo actual(una de las 4 posiciones) con mi posicion
        Vector2 distanceTarget = currentPositionTarget.position - transform.position;
        //Creo un raycast hit2D desde mi posicion hasta distanceTarget
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, distanceTarget, raycastDistance, layerInteraction);
        if(hit2D){
            //Si mi hit2D golpea un Player
            if(hit2D.collider.CompareTag("Player")){
                velocityModifier = fastVelocity;
            }
        }else{
            velocityModifier = normalVelocity;
        }

        myRBD2.velocity = (currentPositionTarget.position - transform.position).normalized*velocityModifier;
        Debug.DrawRay(transform.position, distanceTarget * raycastDistance, Color.cyan);
        
    }
    //realizar un flip en el x
    private void CheckFlip(float x_Position){
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
}
