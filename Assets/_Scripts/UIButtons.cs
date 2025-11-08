using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UIButtons : MonoBehaviour
{
    [SerializeField, HideIf("useSprites")] GameObject[] images;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject textField;
    [SerializeField] string[] storyText;
    [SerializeField] bool useSprites = true,playSoundAtStart=false;
    private typewriterUI type;
    private string toType;
    int index,maxIndex;
    [SerializeField, EnableIf("useSprites")] Image storyBoard;
    [SerializeField] string SceneName,endText;
    private Inputs inputs;
    private InputAction nextline;

    private void Awake()
    {
        inputs = new Inputs();
    }

    private void OnEnable()
    {
        inputs.Enable();
        nextline = inputs.Dialogue.NextLine;
    }

    private void OnDisable()
    {
        index = 0;
        inputs.Disable();
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
        type.Write();
    }

    private void Update()
    {
        nextline.performed += context =>
        {
            ChangeSprite();
        };
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
    //public void ChangeSprite(GameObject button)
    public void ChangeSprite()
    {
        if (index < maxIndex)
        {
            //if (index > maxIndex - 2) { ChangeText(button); }
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
