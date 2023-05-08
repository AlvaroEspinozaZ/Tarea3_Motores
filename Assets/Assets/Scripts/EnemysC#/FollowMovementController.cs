using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script para el campo vision donde si el player choca el enemigo se acercara al player
public class FollowMovementController : MonoBehaviour
{//Estoy modificando la velocidad y posicion del ogro desde otro script
    [SerializeField] private Transform ogreTransform;
    [SerializeField] private Rigidbody2D ogreRB2D;
    [SerializeField] private float velocityModifier;
    [SerializeField] private BulletController bullet;
    [SerializeField] private float TimeToShoot;
    [SerializeField] private SoundsScritableO sonidoDisparo;
    [SerializeField] private SoundsScritableO sonidoFollow;
    private Transform currentTarget;
    private bool isFollowing;
    private bool isMoving;
    private bool canShoot = true;

    private void Start() {
        //Al iniciar mi objetivo actual es mi misma posicion
        currentTarget = transform;
    }

    private void Update() {
        //Si me estoy moviendo 
        if(isMoving){
            ogreRB2D.velocity = (currentTarget.position - ogreTransform.position).normalized * velocityModifier;

            if(isFollowing && canShoot){
                sonidoFollow.CreateSound();
                StartCoroutine(ShootBullet());
                canShoot = false;
            }
            CalculateDistance();
        }else{
            //si no me estoy moviendo, mi currentTarget es igual a mi transform position y no me muevo
            ogreRB2D.velocity = (currentTarget.position - ogreTransform.position).normalized * velocityModifier;
            CalculateDistance();
        }
    }

    //Funcion para calcular distancia
    private void CalculateDistance(){
        //Si la distancia entre mi objetivo y mi posicion es menor a 0.05
        if((currentTarget.position - ogreTransform.position).magnitude < 0.05f){
            //Estoy muy cerca o no me estoy moviendo, x ende me detengo
            ogreTransform.position = currentTarget.position;
            isMoving = false;
            ogreRB2D.velocity = Vector2.zero;
        }else{
            //Caso contrario me muevo
            isMoving = true;
        }
    }
    //Funcion para poner un tiempo de espera a mis disparos
    IEnumerator ShootBullet(){
        Instantiate(bullet, ogreTransform.position, Quaternion.identity).SetUpVelocity(ogreRB2D.velocity, "Enemy", sonidoDisparo);
        //esto es lo q me dice q lo dispare cada 1s.
        yield return new WaitForSeconds(TimeToShoot);
        canShoot = true;
    }

    //Si toca al Player, reemplazo el objetivo x el transform del player y vuelvo true el isFollowing
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            currentTarget = other.transform;
            isFollowing = true;
        }
    }

    //Si dejo de tocar al Player, reemplazo mi objetivo x mi posicion e isFollowing es false.
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            currentTarget = transform;
            isFollowing = false;
        }
    }
}
