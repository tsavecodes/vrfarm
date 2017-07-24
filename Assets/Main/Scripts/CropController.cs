using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{

    WorldController WC;

    public enum CropState
    {
        Empty = 0,
        Unplowed = 1,
        Plowed = 2,
        Seeded = 3,
        Young = 4,
        Mature = 5
    }

    
    [SerializeField] private CropState _state;
    public CropState State
    {
        get { return _state; }
        private set
        {
            _state = value;

            for (int i = 0; i < models.Length; i++)
            {
                if (models[i] != null)
                {
                    bool isActive = (i == (int)_state);
                    models[i].SetActive(isActive);
                }
            }
        }
    }

    public int TotalStates
    {
        get { return Enum.GetNames(typeof(CropState)).Length; }
    }


    public enum CropHealth
    {
        Dry = 0,
        Fair = 1,
        Ideal = 2,
        Wet = 3,
        DeadDry = 4,
        DeadWet = 5,
        Unripe = 6,
        Ripe = 7
    }

    [SerializeField] private CropHealth _health;
    public CropHealth Health
    {
        get { return _health; }
        private set
        {
            _health = value;
            //TODO: Add health indication
            //Debug.Log("Health is " + _health);

      
        }
    }

    
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject healthBar;

    [SerializeField] private float[] durations;
    [SerializeField] public int[] minWaterings;
    [SerializeField] public int[] maxWaterings;

    [SerializeField] private LayerMask layerMask;
    private bool isInteractable = false;
    private float lastAction;
    private float lastWatered;
    private float lastSeeded;

    private float totalWaterings = 0;
    private float wateringsDuringCurrentState = 0;
    private float timeStateChanged = 0;

    private float debugVar = 0;

    public Seed plantedSeed = null;

    void Awake()
    {
        WC = WorldController.Instance;
        WC.OnWorldStateChange += HandleWorldStateChange;

    }

    private void Start()
    {
        State = CropState.Unplowed;
        Health = CropHealth.Dry;
        lastAction = Time.time;

        progressBar.SetActive(false);
        UpdateProgressBar(0);

        healthBar.SetActive(false);
        UpdateHealthBar();

    }

    private void Update()
    {

        progressBar.transform.rotation = Camera.main.transform.rotation;
        healthBar.transform.rotation = Camera.main.transform.rotation;


        //float healthProgress = -1.0f;
        float progress = 0.0f;
        float duration = durations[(int)State];


        switch (State)
        {
            case CropState.Empty:

                break;

            case CropState.Unplowed:

                // plow the crop
                if (isInteractable && Input.GetKeyDown(KeyCode.E))
                {
                    
                    wateringsDuringCurrentState = 0;
                    timeStateChanged = Time.time;
                    State = CropState.Plowed;
                }

                progressBar.SetActive(false);
                UpdateProgressBar(0);

                healthBar.SetActive(false);
                UpdateHealthBar();

                break;

            case CropState.Plowed:

                //seed the crop
                if (isInteractable && Input.GetKeyDown(KeyCode.E))
                {
                   
                    lastAction = Time.time;
                    wateringsDuringCurrentState = 0;
                    timeStateChanged = Time.time;

                    
                    Seed seed = WC.PopSeed();
                    if(seed) {
                        plantSeed(seed);
                        seed.UpdateSeedState(Seed.SeedState.Planted);
                        State = CropState.Seeded;
                    } else {
                        Debug.Log("No Seeds");
                    }
                    
                }

                progressBar.SetActive(false);
                UpdateProgressBar(0);

                healthBar.SetActive(false);
                UpdateHealthBar();


                break;

            case CropState.Seeded:

                //water the crop
                if (isInteractable && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Water!");

                    WC.UseWater();
                    
                    totalWaterings += 1;
                    wateringsDuringCurrentState += 1;

                    Debug.Log(wateringsDuringCurrentState);

                    if (wateringsDuringCurrentState < minWaterings[(int)State])
                    {
                        Health = CropHealth.Dry;
                        //healthProgress = 0.3f;
                    }
                    else if (wateringsDuringCurrentState == minWaterings[(int)State])
                    {
                        Health = CropHealth.Fair;
                        //healthProgress = 0.75f;
                    }
                    else if (wateringsDuringCurrentState > minWaterings[(int)State] && wateringsDuringCurrentState < maxWaterings[(int)State])
                    {
                        Health = CropHealth.Ideal;
                        //healthProgress = 0.9f;
                    }
                    else if (wateringsDuringCurrentState == maxWaterings[(int)State])
                    {
                        Health = CropHealth.Fair;
                        //healthProgress = 0.75f;

                    } else if (wateringsDuringCurrentState > maxWaterings[(int)State])
                    {
                        //healthProgress = 0.0f;
                        Health = CropHealth.Wet;
                        KillCrop();
                    }

                }

                


                //check timing and kill or promote
                
                if (Time.time - timeStateChanged > duration)
                {
                    bool kill = false;

                    if(Health == CropHealth.Dry)
                    {
                        Health = CropHealth.DeadDry;
                        kill = true;
                    }
                    else if(Health == CropHealth.Wet)
                    {
                        Health = CropHealth.DeadWet;
                        kill = true;
                    }

                    if (!kill) {
                        State = CropState.Young;
                        wateringsDuringCurrentState = 0;
                        timeStateChanged = Time.time;
                    } else {
                        KillCrop();
                    }
                }

                progressBar.SetActive(true);
                progress = (Time.time - timeStateChanged) / duration;
                UpdateProgressBar(progress);

                healthBar.SetActive(true);
                UpdateHealthBar();

                break;

            case CropState.Young:

                //water
                if (isInteractable && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Water!");

                    totalWaterings += 1;
                    wateringsDuringCurrentState += 1;

                    Debug.Log(wateringsDuringCurrentState);

                    if (wateringsDuringCurrentState < minWaterings[(int)State])
                    {
                        Health = CropHealth.Dry;
                    }
                    else if (wateringsDuringCurrentState == minWaterings[(int)State])
                    {
                        Health = CropHealth.Fair;
                    }
                    else if (wateringsDuringCurrentState > minWaterings[(int)State] && wateringsDuringCurrentState < maxWaterings[(int)State])
                    {
                        Health = CropHealth.Ideal;
                    }
                    else if (wateringsDuringCurrentState == maxWaterings[(int)State])
                    {
                        Health = CropHealth.Wet;
                    }
                    else if (wateringsDuringCurrentState > maxWaterings[(int)State])
                    {
                        Health = CropHealth.DeadWet;
                        KillCrop();
                    }

                }


                if (Time.time - timeStateChanged > durations[(int)State])
                {
                    bool kill = false;

                    if (Health == CropHealth.Dry)
                    {
                        Health = CropHealth.DeadDry;
                        kill = true;
                    }
                    else if (Health == CropHealth.Wet || Health == CropHealth.DeadWet)
                    {
                        Health = CropHealth.DeadWet;
                        kill = true;
                    }

                    if (!kill)
                    {
                        State = CropState.Mature;
                        wateringsDuringCurrentState = 0;
                        timeStateChanged = Time.time;
                    }
                    else
                    {
                        KillCrop();
                    }
                }


                progressBar.SetActive(true);
                progress = (Time.time - timeStateChanged) / duration;
                UpdateProgressBar(progress);

                healthBar.SetActive(true);
                UpdateHealthBar();

                break;

            case CropState.Mature:

                //pick
                if (isInteractable && Input.GetKeyDown(KeyCode.E))
                {
                    State = CropState.Unplowed;
                    wateringsDuringCurrentState = 0;
                    timeStateChanged = Time.time;
                }


                float halflife = durations[(int)State] / 2;

                if (Time.time - timeStateChanged < halflife)
                {
                    Health = CropHealth.Unripe;
                }
                if (Time.time - timeStateChanged >= halflife && Time.time - timeStateChanged < durations[(int)State])
                {
                    Health = CropHealth.Ripe;
                } else  if (Time.time - timeStateChanged > durations[(int)State])
                {
                    KillCrop();
                }

                progressBar.SetActive(true);
                progress = (Time.time - timeStateChanged) / duration;
                UpdateProgressBar(progress);

                healthBar.SetActive(true);
                UpdateHealthBar();

                break;
        }

        
    }

    public void plantSeed(Seed seed)
    {
        plantedSeed = seed;

  
        //TODO: update with models rather than just change the color
        models[(int)CropState.Seeded].GetComponent<MeshRenderer>().material.SetColor("_Color", seed.GetColor());
        models[(int)CropState.Young].GetComponent<MeshRenderer>().material.SetColor("_Color", seed.GetColor());
        models[(int)CropState.Mature].GetComponent<MeshRenderer>().material.SetColor("_Color", seed.GetColor());


    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ValidLayer(other)) { return; }
        isInteractable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!ValidLayer(other)) { return; }
        isInteractable = false;
    }

    private bool ValidLayer(Collider other)
    {
        int layer = other.gameObject.layer;
        return (layerMask == (layerMask | (1 << layer)));
    }

    public void KillCrop()
    {
        //TODO Kill Crop & Animation
        Debug.Log("Killing The Crop");
        Debug.Log(Health);

        State = CropState.Unplowed;
        Health = CropHealth.Dry;

    }

    public void UpdateProgressBar(float progress = 0.0f)
    {
        progressBar.GetComponent<Renderer>().material.SetFloat("_Cutoff", Mathf.InverseLerp(1, 0, progress));
    }

    public void UpdateHealthBar(float progress=-1)
    {

        if (progress < 0)
        {
            switch (Health)
                //uses defaults based on health
            {
                case CropHealth.DeadDry:
                case CropHealth.DeadWet:
                    progress = 0;
                    break;
                case CropHealth.Dry:
                    progress = 0.2f;
                    break;
                case CropHealth.Fair:
                    progress = 0.5f;
                    break;
                case CropHealth.Ideal:
                    progress = 0.8f;
                    break;
                case CropHealth.Ripe:
                    progress = 1.0f;
                    break;
                case CropHealth.Unripe:
                    progress = 0.5f;
                    break;
                case CropHealth.Wet:
                    progress = 0.2f;
                    break;
            }
        }

        healthBar.GetComponent<Renderer>().material.SetFloat("_Cutoff", Mathf.InverseLerp(1, 0, progress));
    }

    private void HandleWorldStateChange()
    {

    }
}
