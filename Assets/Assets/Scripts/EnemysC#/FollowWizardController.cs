using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWizardController : MonoBehaviour
{
    [SerializeField] private Transform wizTransform;
    [SerializeField] private float velocityModifier;
    [SerializeField] private BulletController bullet;
    [SerializeField] private float TimeToShoot;
    [SerializeField] private Transform[] checkpointsPatrol;
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask layerInteraction;
    [SerializeField] private SoundsScritableO sonidoDisparo;
    [SerializeField] private SoundsScritableO sonidoFollow;
    private Transform currentPositionTarget;
    private Transform currentTarget;
    private int patrolPos = 0;
    private float fastVelocity = 0f;
    private float normalVelocity;
    private bool isFollowing;
    private bool isMoving;
    private bool canShoot = true;
    // Start is called before the first frame update
    void Start()
    {
        currentPositionTarget = checkpointsPatrol[patrolPos];
        transform.position = currentPositionTarget.position;
        normalVelocity = velocityModifier;
        fastVelocity = velocityModifier * 2.5f;
    }

    // Update is called once per frame
    void Update()
    {       
        if (isFollowing)
        {
            sonidoFollow.CreateSound();
            myRBD2.velocity = (currentTarget.position - wizTransform.position).normalized * velocityModifier;
            if (canShoot)
            {
                StartCoroutine(ShootBullet());
                canShoot = false;
            }
        }          
        
        else
        {
            currentTarget = transform;
            CheckNewPoint();
            animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);
        }
        
    }
    private void CheckNewPoint()
    {
        //Si el valor absoluto de la distancia(magnitude) de la posicion del escorpion menos la posicion del objetivo actual es menor a 0.25
        if (Mathf.Abs((transform.position - currentPositionTarget.position).magnitude) < 0.25)
        {
            transform.position = currentPositionTarget.position;
            //se suma 1 patrolPos y la cadena de las posiciones a pasa al siguiente 
            patrolPos = patrolPos + 1 == checkpointsPatrol.Length ? 0 : patrolPos + 1;
            currentPositionTarget = checkpointsPatrol[patrolPos];
            CheckFlip(myRBD2.velocity.x);
        }
        //Resto las posicioes de mi objetivo actual(una de las 4 posiciones) con mi posicion
        Vector2 distanceTarget = currentPositionTarget.position - transform.position;
        //Creo un raycast hit2D desde mi posicion hasta distanceTarget
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, distanceTarget, raycastDistance, layerInteraction);
        if (hit2D)
        {
            //Si mi hit2D golpea un Player
            if (hit2D.collider.CompareTag("Player"))
            {
                velocityModifier = fastVelocity;
            }
        }
        else
        {
            velocityModifier = normalVelocity;
        }

        myRBD2.velocity = (currentPositionTarget.position - transform.position).normalized * velocityModifier;
        Debug.DrawRay(transform.position, distanceTarget * raycastDistance, Color.cyan);

    }
    //realizar un flip en el x
    private void CheckFlip(float x_Position)
    {
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }

    private void CalculateDistance()
    {
        //Si la distancia entre mi objetivo y mi posicion es menor a 0.05
        if ((currentTarget.position - wizTransform.position).magnitude < 0.05f)
        {
            //Estoy muy cerca o no me estoy moviendo, x ende me detengo
            wizTransform.position = currentTarget.position;

            myRBD2.velocity = Vector2.zero;
        }  
    }
    IEnumerator ShootBullet()
    {
        Instantiate(bullet, wizTransform.position, Quaternion.identity).SetUpVelocity(myRBD2.velocity, "Enemy", sonidoDisparo);
        //esto es lo q me dice q lo dispare cada 1s.
        yield return new WaitForSeconds(TimeToShoot);
        canShoot = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentTarget = other.transform;
            isFollowing = true;
        }
    }

    //Si dejo de tocar al Player, reemplazo mi objetivo x mi posicion e isFollowing es false.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentTarget = transform;
            isFollowing = false;
        }
    }
}
