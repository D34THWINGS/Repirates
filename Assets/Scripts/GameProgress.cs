using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public float BoatSpeed = 1.0f;
    public float GameDuration = 60 * 3;
    public float TimeBetweenEvents = 7;

    public float Progress { get; private set; }
    
    private float endTime;
    private float lastEventTime;

    // Start is called before the first frame update
    void Start()
    {
        endTime = Time.time + GameDuration;
        lastEventTime = Time.time;
        StartCoroutine("OnTimerTick");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TriggerEvent()
    {
        Debug.Log("Rock approaching!");
    }

    void Lose()
    {
        Debug.Log("Game lost!");
    }

    void Win()
    {
        Debug.Log("Game won!");
    }

    IEnumerator OnTimerTick()
    {
        while (endTime > Time.time && Progress < 100)
        {
            yield return new WaitForSeconds(1);
            Progress = Mathf.Min(Progress + BoatSpeed, 100);
            Debug.Log("Time left: " + (endTime - Time.time) / 1000 + "s; Progress: " + Progress + "%");

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
}
