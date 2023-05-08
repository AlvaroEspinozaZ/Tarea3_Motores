using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRGB2D;
    [SerializeField] private float velocityMultiplier;
    [SerializeField] private int damage;
    public event Action<int, HealthBarController> onCollision;

    //Funcion para dar velocidad a la bala cuando la instancien desde un player o un enemy, ademas
    //funciona para activar el instance al ser creado
    //pide 2 cosas la velocidad y el tag del objeto desde se crea(Enemy/ Player)
    public void SetUpVelocity(Vector2 velocity, string newTag, SoundsScritableO myAudio)
    {
        myRGB2D.velocity = velocity * velocityMultiplier;
        gameObject.tag = newTag;
        myAudio.CreateSound();
        DamageManager.instance.SubscribeFunction(this);
    }


    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        //Si el objeto que choca es distinto al de la bala Y el objeto q choca es un Player o un Enemy
        if(!other.CompareTag(gameObject.tag) && (other.CompareTag("Player") || other.CompareTag("Enemy"))){
            //Si el obejto que coliciono tiene un Script de BarraVida
            if(other.GetComponent<HealthBarController>()){
                //Se invoca el evento de accion y le pasa los valores de daño y la BarraVida del objeto q golpeo
                onCollision?.Invoke(damage,other.GetComponent<HealthBarController>());
            }
            Destroy(this.gameObject);
        }
    }
}
