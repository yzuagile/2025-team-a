using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startnutton : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text blinkingText;   // ©ì¤J TextMeshPro UI ¤¸¥ó
    public float blinkInterval = 0.5f;

    void Start()
    {
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            blinkingText.enabled = !blinkingText.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

}
