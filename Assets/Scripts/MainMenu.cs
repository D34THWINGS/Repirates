using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
  public GameObject SliderPanel;
  public Slider slider;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnPlay()
  {
        //SceneManager.LoadScene(1);
        SliderPanel.SetActive(true);
        StartCoroutine(LoadSceneAsync());
  }

  IEnumerator LoadSceneAsync()
  {
    AsyncOperation operation = SceneManager.LoadSceneAsync(1);
    while(!operation.isDone)
    {
      float progress = Mathf.Clamp01(operation.progress);
      slider.value = progress;

      yield return null;
    }
  }
}
