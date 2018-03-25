using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rotationRocket = 100f; // Controle de força de rota
    [SerializeField] float thrust = 100f; //
    [SerializeField] AudioClip aliveSound;
    [SerializeField] AudioClip sucess;
    [SerializeField] AudioClip deathSound;

    Rigidbody rigidbody;//Classe de controle de physis
    AudioSource audiosource;
    enum State { Alive, Dead, Transitioning}// Enum para controle de estados
    State state = State.Alive;


    // Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();// Componente de Audio. Localiza todos os componentes de forma genérica.
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            Thrust();//Lança o Foguete
            Rotate();//Rotaciona o Foguete
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Save");
                break;

            case "Finish":
                state = State.Transitioning;
                audiosource.PlayOneShot(sucess);
                Invoke("LoadNextScene", 1f);
                break;

            case "Untagged":
                state = State.Dead;
                audiosource.PlayOneShot(deathSound);
                print("Die");
                Invoke("LoadPreviousScene", 1f);
                break;

            default:
                break;
        }
    }

    private void LoadPreviousScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true;
        float rotationthisframe = rotationRocket * Time.deltaTime;
       if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationthisframe);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationthisframe);
        }
        rigidbody.freezeRotation = false;

    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ActivateThrust();

        }
        else
        {
            audiosource.Stop();
        }
    }

    private void ActivateThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * thrust);
        if (!audiosource.isPlaying)
        {
            audiosource.PlayOneShot(aliveSound);
        }
    }
}
