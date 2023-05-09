using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private CameraController cameraReference;
    [Header("Sonidos")]
    [SerializeField] private SoundsScritableO sonidoDisparar;
    [SerializeField] private SoundsScritableO sonidoRecivirDaño;
    [SerializeField] private SoundsScritableO sonidoPocaVida;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    private Vector3 distanciaFire;
    //Action Maps
    private string actionMapPlayerControls = "Player Controls";
    private string actionMapMenuControls = "Menu Controls";

    //Current Control Scheme
    private string currentControlScheme;

    public void SetupPlayer(int newPlayerID)
    {
        currentControlScheme = playerInput.currentControlScheme;
    }
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

        //Vector2 movementPlayer = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //myRBD2.velocity = movementPlayer * velocityModifier;

        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);

        //Vector3 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //CheckFlip(mouseInput.x);
        CheckFlip(distanciaFire.x);
        //Distancia entre la posicion de mi mouse - posicion del player
        Vector3 distance = distanciaFire - transform.position;
        Debug.DrawRay(transform.position, distance * rayDistance, Color.red);

        if(Input.GetMouseButtonDown(0)){
            //Si disparo se creara una bala desde posicon de player y a donde apunta
            //BulletController myBullet =  Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //Se le da una velocidad(en base a la distancia q hay entre el player y mi puntero) y el tag del player
            //myBullet.SetUpVelocity(distance.normalized, gameObject.tag, sonidoDisparar);
        }else if(Input.GetMouseButtonDown(1)){
            GetComponent<HealthBarController>().UpdateHealth(26);
        }
    }

    private void CheckFlip(float x_Position){
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        myRBD2.velocity = inputMovement * velocityModifier;
    }
    public void OnAim(InputAction.CallbackContext value)
    {
        distanciaFire = Camera.main.ScreenToWorldPoint(value.ReadValue<Vector2>());
    }
    public void OnFire(InputAction.CallbackContext value)
    {
        if(value.started)
        {
        Vector3 distance = distanciaFire - transform.position;
        BulletController myBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        //Se le da una velocidad(en base a la distancia q hay entre el player y mi puntero) y el tag del player
        myBullet.SetUpVelocity(distance.normalized, gameObject.tag, sonidoDisparar);
        }
        
    }
}
