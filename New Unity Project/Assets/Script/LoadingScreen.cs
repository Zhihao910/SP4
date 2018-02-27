using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public Transform loadingBar;
    public Transform textIndicator;
    public Transform textLoading;

    private float currentAmount, speed;
    private bool loaded = false;

    // Use this for initialization
    void Start()
    {
        currentAmount = 0;
        speed = 20;
        Screen.orientation = ScreenOrientation.Landscape;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
            textIndicator.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
        }
        else
        {
#if UNITY_EDITOR || UNITY_WINDOWS
            textLoading.GetComponent<Text>().text = "Press Space to continue..";
#elif UNITY_ANDROID
            textLoading.GetComponent<Text>().text = "Press to continue..";
#endif
            loaded = true;
        }

        loadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;

        if (loaded && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            SceneManager.LoadScene("MainMenu");
        }
        //StartCoroutine(loadAsyncScene("MainMenu"));
    }

    IEnumerator loadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            currentAmount = progress;
            loadingBar.GetComponent<Image>().fillAmount = currentAmount;

            yield return null;
        }
    }
}
