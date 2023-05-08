using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageManager : MonoBehaviour
{
    public static DamageManager instance {get; private set;}

    private void Awake() {
        if(instance != null && instance != this){
            Destroy(this.gameObject);
        }

        instance = this;
    }

    /*public void SubscribeToEvent(Action <int, HealthBarController> currentAction){
        currentAction += DamageCalculation;
    }*/

    //Funcion suscrita para la bala del enemigo
    public void SubscribeFunction(BulletController enemy){
        enemy.onCollision += DamageCalculation;
    }

    //Funcion suscrita para cuando el player choca con el enemigo del enemigo
    public void SubscribeFunction(EnemySimpleController enemy){
        enemy.onCollision += DamageCalculation;
    }

    //Funcion que toma el componente de healthBarController, para llamar la funcion de actualizar vida
    //y usa el daño que es recivido del objeto
    private void DamageCalculation(int damageTaken, HealthBarController healthBarController){
        healthBarController.UpdateHealth(-damageTaken);
    }
}
