using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class c1 : MonoBehaviour
{
    private static int BIKE_LAYER = 3;
    private static int HELM_ON = BIKE_LAYER + 1;
    private static int HELM_OFF = BIKE_LAYER - 1;
    private int helmLayer = HELM_OFF;
    
    [SerializeField] private GameController GameController;
    [SerializeField] private int maxHelmCooldown = 5;
    [SerializeField] private bool HelmState = false;
    [SerializeField] private float CAR_SPEED;
    [SerializeField] private float driveDistance = 10f;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private GameObject police;
    [SerializeField] private GameObject car;
    [SerializeField] private  GameObject vehicleSprite;
    [SerializeField] private  GameObject helmSprite;
    [SerializeField] private AudioSource src;

    private int helmCooldown = 5;
    private float curCooldownCount = 0;
    private Vector3 _startPosition;
    private PoliceCar policeCarSound;
    // Start is called before the first frame update
    void Start()
    {
        if (HelmState) helmLayer = HELM_ON;
        vehicleSprite.GetComponent<SpriteRenderer> ().sortingOrder = BIKE_LAYER;
        UpdateHelm();
        helmCooldown = Random.Range(0, maxHelmCooldown);
        originalPosition = transform.localPosition;
        _startPosition = originalPosition;
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_startPosition, this.transform.localPosition) > driveDistance)
        {
            moveDirection *= -1;
            float z = vehicleSprite.transform.eulerAngles.z ;
            vehicleSprite.transform.rotation  = Quaternion.Euler(new Vector3(0, 0, z + 180));
            _startPosition = transform.localPosition;
        }
        if (helmLayer == HELM_ON) curCooldownCount += Time.deltaTime;
        checkHelmCooldown();
        car.transform.position += Time.deltaTime * moveDirection * CAR_SPEED;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetType() == police.GetType())
        {
            if (helmLayer == HELM_OFF)
            {
                src.Play();
                helmLayer = HELM_ON;
                UpdateHelm();
                GameController.IncrementScore();
            } 
        }
    }


    private void UpdateHelm()
    {
        helmSprite.GetComponent<SpriteRenderer> ().sortingOrder = helmLayer;  
    }

    private void checkHelmCooldown()
    {
        if (!(curCooldownCount > helmCooldown)) return;
        helmLayer = HELM_OFF;
        UpdateHelm();
        curCooldownCount = 0;
        helmCooldown = Random.Range(0, maxHelmCooldown);
    }
}
