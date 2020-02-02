using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public RockSpawner RockSpawn;

    public float Progress { get; private set; }
    
    private float endTime;
    private float lastEventTime;
    private float nextRockHit = -1;
    private int holesInHull = 0;
    private float waterLevel = 0;
    public Slider slider;

    public Text Timer;
    public GameObject AlertPanel;
    public GameObject ArrowDown;
    public GameObject LosePanel;
    public GameObject WinPanel;

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
            nextRockHit = -1;
            Debug.Log("Hit by rock!");
            RockSpawn.SpawnRock();
            AlertPanel.SetActive(false);
            ArrowDown.SetActive(false);
        }

        var targetWaterLvel = new Vector3(BoatWrapper.position.x, -waterLevel, BoatWrapper.position.z);
        BoatWrapper.position = Vector3.Lerp(BoatWrapper.position, targetWaterLvel, Time.deltaTime);
    }

    void TriggerEvent()
    {
        Debug.Log("Rock approaching!");
        nextRockHit = Time.time + RockHitDelay;
        lastEventTime = Time.time;
        AlertPanel.SetActive(true);
        ArrowDown.SetActive(true);
    }
    public void repairHole()
    {
        holesInHull -= 1;
        waterLevel -= 2;
    }
    public GameObject findHole(int HoleNumber)
    {
        var GameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject objects in GameObjects){
            if (objects.name == "Hole (" + HoleNumber.ToString() + ")")
            {
                return objects;
            }
        }
        return new GameObject();
    }
    public void HitRock()
    {
        if (GameObject.FindGameObjectsWithTag("Holes").Length == 19) // No more hole to open
        {
            Debug.Log("No more hole to show");
            return;
        }
        int holeInt = UnityEngine.Random.Range(1, 19);
        GameObject HoleGameObject = findHole(holeInt);
        holesInHull += 1;
        HoleGameObject.SetActive(true);
    }

    public void DodgeRock()
    {
        nextRockHit = -1;
        AlertPanel.SetActive(false);
        ArrowDown.SetActive(false);
    }

    void Lose()
    {
        Debug.Log("Game lost!");
        BoatWrapper.GetComponentInChildren<Animator>().Play("Sinking");
        LosePanel.SetActive(true);
    }

    void Win()
    {
        Debug.Log("Game won!");
        WinPanel.SetActive(true);
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
        return string.Format("{0:D2} : {1:D2}", t.Minutes, t.Seconds);
    }
}
