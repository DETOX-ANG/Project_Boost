using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcktthrust = 100f;
    [SerializeField] float thrust = 100f;
    Rigidbody rigidbody;
    AudioSource audiosource;
    enum State { Alive, Dead, Transitioning}
    State state = State.Alive;

    // Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
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
                Invoke("LoadNextScene", 1f);
                break;

            case "Untagged":
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
        float rotationthisframe = rcktthrust * Time.deltaTime;
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
            rigidbody.AddRelativeForce(Vector3.up * thrust);
            if (!audiosource.isPlaying)
            {
                audiosource.Play();
            }

        }
        else
        {
            audiosource.Stop();
        }
    }
}
