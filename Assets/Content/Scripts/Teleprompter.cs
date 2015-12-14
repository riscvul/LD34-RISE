using UnityEngine;
using System.Collections;
using TMPro;

public class Teleprompter : MonoBehaviour {

    private TextMeshPro m_textMeshPro;
    int currentCharacterCounter = 0;
    int totalVisibleCharacters = 0;
    bool hideMe = true;

	// Use this for initialization
	void Start () {
        m_textMeshPro = gameObject.GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
	}

    public void PrepMessage(string message) {
        m_textMeshPro.text = message;
        hideMe = false;
        totalVisibleCharacters = m_textMeshPro.GetTextInfo(message).characterCount;

        StopAllCoroutines();
        StartCoroutine("TypeMessageOnScreen");
    }

    IEnumerator TypeMessageOnScreen() {
        //totalVisibleCharacters = m_textMeshPro.textInfo.characterCount;
        while (!hideMe) {

            int visibleCount = currentCharacterCounter % (totalVisibleCharacters + 1);

            m_textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters) {
                hideMe = true;
                yield return new WaitForSeconds(1.5f);
            }

            currentCharacterCounter++;

            yield return new WaitForSeconds(0.05f);
        }
        m_textMeshPro.text = "";
        currentCharacterCounter = 0;
        totalVisibleCharacters = 0;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
