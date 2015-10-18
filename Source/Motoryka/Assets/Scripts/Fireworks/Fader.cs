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
        if( !fadingTexture )
        {
            Debug.LogError("No texture to fade.");
            return;
        }

        Debug.Log("Fading to scene " + scene);
        StartCoroutine("LoadFading", scene);
    }

    private IEnumerator LoadFading(string scene)
    {
        isFading = true;

        float delay = 0.1f;
        
        _alpha = 0f;
        fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, _alpha);

        while(_alpha < 1f)
        {
            yield return new WaitForEndOfFrame();

            _alpha += Time.deltaTime * fadingSpeed;
            fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, _alpha);
        }

        fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, 1f);

        yield return new WaitForSeconds(delay/2);

        Application.LoadLevel(scene);

        yield return new WaitForSeconds(delay / 2);

        while (_alpha > 0)
        {
            _alpha -= Time.deltaTime * fadingSpeed;
            fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, _alpha);

            yield return new WaitForEndOfFrame();
        }
        fadingTexture.color = new Color(fadingTexture.color.r, fadingTexture.color.g, fadingTexture.color.b, 0f);


        isFading = false;

		ShapeGenerator sg = new ShapeGenerator ();
		sg.Start ();
    }
}
