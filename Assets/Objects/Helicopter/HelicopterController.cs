using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    #region Variables
    private Rigidbody _rigidbody;

    public float engineSpeed;
    public float rotationForce;

    public float tiltForce;

    public float tiltLimit = 20;
    private float rotationZ = 0;
    private float rotationX = 0;

    public Transform rotor;
    public Transform tailRotor;
    public float maxRotorSpeed;
    private float currentRotorSpeed;


    private AudioSource _audioSource;
    private float SFXvol;
    private float SFXpitch;
    #endregion

    #region Basic methods
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SFXpitch = 0;
        SFXvol = 0;
    }

    private void FixedUpdate()
    {
        rotor.Rotate(0, currentRotorSpeed, 0);
        tailRotor.Rotate(0, 0, currentRotorSpeed);

        // ÏÓ˘ÌÓÒÚ¸
        if (Input.GetKey(KeyCode.W))
        {
            currentRotorSpeed = Mathf.Lerp(currentRotorSpeed, maxRotorSpeed, Time.fixedDeltaTime * 0.7f);

            SFXvol = Mathf.Lerp(SFXvol, 1, Time.deltaTime);
            SFXpitch = Mathf.Lerp(SFXpitch, 1, Time.deltaTime);

            if (currentRotorSpeed > 350)
                _rigidbody.AddRelativeForce(new Vector3(0, 1, 0) * engineSpeed, ForceMode.Acceleration);            
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.AddRelativeForce(new Vector3(0, -1, 0) * engineSpeed, ForceMode.Acceleration);
        }


        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit) && hit.distance > 5.5f)
        {
            mainFunctionality();
        }
        else
        {
            currentRotorSpeed = Mathf.Lerp(currentRotorSpeed, 0, Time.fixedDeltaTime * 3);
            SFXvol = Mathf.Lerp(SFXvol, 0, Time.deltaTime);
            SFXpitch = Mathf.Lerp(SFXpitch, 0, Time.deltaTime);
        }

        _audioSource.volume = SFXvol;
        _audioSource.pitch = SFXpitch;

    }
    #endregion

    #region Additional methods
    private void mainFunctionality()
    {
        // ÔÓ‚ÓÓÚ
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -1, 0) * rotationForce);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 1, 0) * rotationForce);
        }

        // Ì‡ÍÎÓÌ ÎÂ‚Ó/Ô‡‚Ó
        if (Input.GetKey(KeyCode.Keypad4))
        {
            LeftŒrRight(tiltLimit);
        }
        else if (Input.GetKey(KeyCode.Keypad6))
        {
            LeftŒrRight(-tiltLimit);
        }
        else
        {
            LeftŒrRight(0);
        }

        // Ì‡ÍÎÓÌ ÔÂÂ‰/Á‡‰
        if (Input.GetKey(KeyCode.Keypad8))
        {
            forwardŒrBackward(tiltLimit);
        }
        else if (Input.GetKey(KeyCode.Keypad5))
        {
            forwardŒrBackward(-tiltLimit);
        }
        else
        {
            forwardŒrBackward(0);
        }
    }

    private void forwardŒrBackward(float tiltLimit)
    {
        rotationX = Mathf.Lerp(rotationX, tiltLimit, Time.fixedDeltaTime);
        transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void LeftŒrRight(float tiltLimit)
    {
        rotationZ = Mathf.Lerp(rotationZ, tiltLimit, Time.fixedDeltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotationZ);
    }
    #endregion

}
