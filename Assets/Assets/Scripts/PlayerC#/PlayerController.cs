using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private CameraController cameraReference;

    [SerializeField] private SoundsScritableO sonidoDisparar;
    [SerializeField] private SoundsScritableO sonidoRecivirDaño;
    [SerializeField] private SoundsScritableO sonidoPocaVida;
    private void Start() {
        //se le da al evento de golpe de la Barra vida la funcion de CallScreenShake
        GetComponent<HealthBarController>().onHit += cameraReference.CallScreenShake;
        GetComponent<HealthBarController>().onHit += sonidoRecivirDaño.CreateSound;
    }

    private void Update() {
        if (GetComponent<HealthBarController>().currentValue <= 25)
        {
            sonidoPocaVida.CreateSound();
        }
        Vector2 movementPlayer = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        myRBD2.velocity = movementPlayer * velocityModifier;

        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);

        Vector3 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckFlip(mouseInput.x);
        //Distancia entre la posicion de mi mouse - posicion del player
        Vector3 distance = mouseInput - transform.position;
        Debug.DrawRay(transform.position, distance * rayDistance, Color.red);

        if(Input.GetMouseButtonDown(0)){
            //Si disparo se creara una bala desde posicon de player y a donde apunta
            BulletController myBullet =  Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //Se le da una velocidad(en base a la distancia q hay entre el player y mi puntero) y el tag del player
            myBullet.SetUpVelocity(distance.normalized, gameObject.tag, sonidoDisparar);
        }else if(Input.GetMouseButtonDown(1)){
            GetComponent<HealthBarController>().UpdateHealth(26);
        }
    }

    private void CheckFlip(float x_Position){
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
}
