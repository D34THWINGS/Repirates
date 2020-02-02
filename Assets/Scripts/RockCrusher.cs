using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RockCrusher : MonoBehaviour
{
    public UnityEvent OnRockCrushed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        Destroy(collider.gameObject);
        OnRockCrushed.Invoke();
        StartCoroutine("ShakeCamera");
    }

    IEnumerator ShakeCamera()
    {
        Camera.main.transform.Translate(new Vector3(-1, -0.5f, 0));
        yield return new WaitForSeconds(0.01f);
        Camera.main.transform.Translate(new Vector3(2, 1.5f, 0));
        yield return new WaitForSeconds(0.01f);
        Camera.main.transform.Translate(new Vector3(-0.5f, -1, 0));
        yield return new WaitForSeconds(0.01f);
        Camera.main.transform.Translate(new Vector3(0.5f, -0.5f, 0));
        yield return new WaitForSeconds(0.01f);
        Camera.main.transform.Translate(new Vector3(-2, 0.5f, 0));
        yield return new WaitForSeconds(0.01f);
        Camera.main.transform.Translate(new Vector3(2, 0.5f, 0));
        yield return new WaitForSeconds(0.01f);
        Camera.main.transform.Translate(new Vector3(-1, -0.5f, 0));
    }
}
