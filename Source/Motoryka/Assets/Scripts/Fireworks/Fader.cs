using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
    public Image fadingTexture;
    public float fadingSpeed = 20f;
    public bool isFading = false;

    float _alpha = 0f;

    void Start()
    {
        isFading = false;
        _alpha = 0f;
    }

	public void LoadSceneFading(string scene)
    {
        LoadSceneFadingAfterTime(scene, new WaitForSeconds(0f));
    }

    public void LoadSceneFadingAfterTime(string scene, WaitForSeconds time)
    {
        if (!fadingTexture)
        {
            Debug.LogError("No texture to fade.");
        }

        Debug.Log("Fading to scene " + scene);
        StartCoroutine(LoadFading(scene, time));
    }

    public void FinishGame(string startingScene, WaitForSeconds time)
    {
        if (!fadingTexture)
        {
            Debug.LogError("No texture to fade.");
        }

        Debug.Log("Fading to scene " + startingScene);
        StartCoroutine(LoadFading(startingScene, time, true));
    }


    private IEnumerator LoadFading(string scene, WaitForSeconds time, bool destroy = false)
    {
        yield return time;

        isFading = true;

        float delay = 0.1f;
        
        _alpha = 0f;
        if( fadingTexture )
            fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, _alpha);

        while(_alpha < 1f)
        {
            yield return new WaitForEndOfFrame();

            _alpha += Time.deltaTime * fadingSpeed;
            if (fadingTexture)
                fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, _alpha);
        }

        if (fadingTexture)
            fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, 1f);

        yield return new WaitForSeconds(delay / 2);

        Application.LoadLevel(scene);

        yield return new WaitForSeconds(delay / 2);

        while (_alpha > 0)
        {
            _alpha -= Time.deltaTime * fadingSpeed;
            if (fadingTexture)
                fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, _alpha);

            yield return new WaitForEndOfFrame();
        }
        if (fadingTexture)
            fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, 0f);


        isFading = false;

        if (destroy)
            GameManager.Instance.RestartGame();
    }
}
