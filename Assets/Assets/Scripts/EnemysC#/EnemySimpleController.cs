using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class EnemySimpleController : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int score = 50;
    public event Action<int, HealthBarController> onCollision;
    public GameObject objetoPadre;
    [SerializeField] private SoundsScritableO sonidoRecivirDaño;

    private void Start() {
        //Al iniciar esto se debe instanciar al DamageManager para q al chocar con un player haga daño
        DamageManager.instance.SubscribeFunction(this);
        //Ademas de suscribierlo al evento de muerto de su barra vida
        GetComponent<HealthBarController>().onDeath += OnDeath;
        GetComponent<HealthBarController>().onHit += sonidoRecivirDaño.CreateSound;
    }

    //Funcion de Muerte
    private void OnDeath(){
        //Animacion de muerte
        GetComponent<AnimatorController>().SetDie();        
        //Se llama la instancia de la GuiManager para sumarle los puntos del enemy
        GuiManager.instance.UpdateText(score);
        Destroy(this.gameObject, 1f);
        Destroy(objetoPadre, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            //Al tocar un Player, este tiene componentes de BarraVida
            if(other.GetComponent<HealthBarController>()){
                //Se invoca el evento colision, pasandole el daño y actualizando la barra vida player
                onCollision?.Invoke(damage,other.GetComponent<HealthBarController>());
            }
        }
    }
}
