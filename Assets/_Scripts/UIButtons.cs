using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NaughtyAttributes;

public class UIButtons : MonoBehaviour
{
    [SerializeField] GameObject[] images;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject textField;
    [SerializeField] string[] storyText;
    [SerializeField] bool useSprites = true,playSoundAtStart=false;
    private typewriterUI type;
    private string toType;
    int index,maxIndex;
    [SerializeField, EnableIf("useSprites")] Image storyBoard;
    [SerializeField] string SceneName,endText;
    private void OnDisable()
    {
        index = 0;
    }
    private void Start()
    {
        
        if (!useSprites)
        {
            maxIndex = images.Length - 1;
            return;
        }
        maxIndex=sprites.Length - 1;
        storyBoard.sprite = sprites[index];
        type = textField.GetComponent<typewriterUI>();
        
    }
    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    void PlayStartSound()
    {
        //AudioManager.instance.PlayOneShot(AudioManager.instance.StorySound, this.transform.position);
    }
    public void Change(GameObject button)
    {
        if (index < maxIndex)
        {
            if(index>maxIndex-2) { ChangeText(button); }
            images[index].SetActive(false);
            index++;
            images[index].SetActive(true);
            return;
        }
        LoadScene(SceneName);
    }
    public void ChangeSprite(GameObject button)
    {
        if (index < maxIndex)
        {
            if (index > maxIndex - 2) { ChangeText(button); }
            index++;
            storyBoard.sprite = sprites[index];
            type.writer = storyText[index];
            type.Write();
            //AudioManager.instance.PlayOneShot(AudioManager.instance.StorySound, this.transform.position);
            return;
        }
        LoadScene(SceneName);
    }
    void ChangeText(GameObject button)
    {
;       button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = endText;
    }
}
