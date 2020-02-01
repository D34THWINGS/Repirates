﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    public float BoatSpeed = 1.0f;
    public float GameDuration = 60 * 3;
    public float TimeBetweenEvents = 7;
    public float RockHitDelay = 5;
    public float WaterLevelIncreasePerHole = 0.2f;
    public float MaxWaterLevel = 9;
    public Transform BoatWrapper;

    public float Progress { get; private set; }
    
    private float endTime;
    private float lastEventTime;
    private float nextRockHit = -1;
    private int holesInHull = 0;
    private float waterLevel = 0;
    public Slider slider;

    public Text Timer;

    // Start is called before the first frame update
    void Start()
    {
        endTime = Time.time + GameDuration;
        lastEventTime = Time.time;
        Timer.text = Format(endTime);
        StartCoroutine("OnTimerTick");
    }

    // Update is called once per frame
    void Update()
    {
        if (nextRockHit != -1 && nextRockHit < Time.time)
        {
            Debug.Log("Hit by rock!");
            nextRockHit = -1;
            holesInHull += 1;
        }

        var targetWaterLvel = new Vector3(BoatWrapper.position.x, -waterLevel, BoatWrapper.position.z);
        BoatWrapper.position = Vector3.Lerp(BoatWrapper.position, targetWaterLvel, Time.deltaTime);
    }

    void TriggerEvent()
    {
        Debug.Log("Rock approaching!");
        nextRockHit = Time.time + RockHitDelay;
        lastEventTime = Time.time;
    }

    public void DodgeRock()
    {
        nextRockHit = -1;
    }

    void Lose()
    {
        Debug.Log("Game lost!");
        BoatWrapper.GetComponentInChildren<Animator>().Play("Sinking");
    }

    void Win()
    {
        Debug.Log("Game won!");
    }

    IEnumerator OnTimerTick()
    {
        while (endTime > Time.time && Progress < 100 && waterLevel < MaxWaterLevel)
        {
            yield return new WaitForSeconds(1);
            Progress = Mathf.Min(Progress + BoatSpeed, 100);
            Debug.Log("Time left: " + Mathf.RoundToInt(endTime - Time.time) + "s; Progress: " + Progress + "%; Water level: " + waterLevel);

            waterLevel += holesInHull * WaterLevelIncreasePerHole;
            slider.value = Progress;
            float secs = (endTime - Time.time);
            Timer.text = Format(secs);

            if (lastEventTime + TimeBetweenEvents < Time.time)
            {
                TriggerEvent();
            }
        }
        if (Progress == 100)
        {
            Win();
        } else
        {
            Lose();
        }
    }

    string Format(float second)
    {
        TimeSpan t = TimeSpan.FromSeconds(second);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
