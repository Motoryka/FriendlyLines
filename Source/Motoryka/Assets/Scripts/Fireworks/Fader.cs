/**********************************************************************
Copyright (C) 2015  Wojciech Nadurski

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
    public Image fadingTexture;
    public float fadingSpeed = 20f;
    public bool isFading = false;

    string finishSceneName = "end";

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

        Debug.Log("Fading to scene " + startingScene + " in time: " + time);
        StartCoroutine(LoadFading(startingScene, time, true));
        GameManager.Instance.titleMusic.Play();
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

        if (scene.Equals(finishSceneName))
            GameManager.Instance.gameFinishedSound.Play();

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
