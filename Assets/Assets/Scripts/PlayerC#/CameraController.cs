using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera myVC;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Start() {
        //Le damos los componentes de CinemachineBasicMultiChannelPerlin a noise
        noise = myVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //StartCoroutine(ShakeCamera(3,5f));
    }

    //Creamos la funcion q llama el movimiento de la camara
    public void CallScreenShake(){
        StartCoroutine(ShakeCamera(5,0.5f));
    }

    //Numerator q da los valores al shakeCamera, pide intensidad y tiempo
    IEnumerator ShakeCamera(float intensity, float time){
        noise.m_AmplitudeGain = intensity;
        float totalTime = time;
        float initIntensity = intensity;
        //MIentras el tiempo es mayor a 0
        while(totalTime > 0){
            totalTime -= Time.deltaTime;//tiempo va disminuyendo
            noise.m_AmplitudeGain = Mathf.Lerp(initIntensity,0f, 1-(totalTime/time));//Aca si no entiendo bien q hace
            //pero supongo q entre los rangos de intensidad inicial y 0, por un tiempo 1- 5(q va disminuyendo)/5
            yield return null;// q solo pasa 1 vez...creo
        }
    }
}
