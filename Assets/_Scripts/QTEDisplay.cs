using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace _Scripts
{
    /*
     * This Script Is Responsible only for Displaying QTE and Handles all things around it
     */
    public class QTEDisplay : MonoBehaviour
    {
        //[SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float gridOffset;
        [SerializeField] private GameObject prefab;
        private TextMeshProUGUI[] text;
        //this dictionary is responsible for translating Vector2 input of QTE to visuals (currently characters)
        //can be changed if needed to images
        private Dictionary<Vector2, string> displayDirections = new Dictionary<Vector2, string>
        {
            { Vector2.up, "↑" },
            { Vector2.down, "↓" },
            { Vector2.left, "←" },
            { Vector2.right, "→" }
        };
        private RectTransform _rectTransform;
        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        //get and display QTE task
        public void WriteCharacters(Vector2[] characters)
        {
            Vector2 origin = transform.position;
            text = new TextMeshProUGUI[characters.Length];
            for (int i = 0; i < text.Length; i++)
            {
                Vector2 spawnPosition = new Vector2(i*gridOffset, 0)+origin;
                SpawnObject(i);
                text[i].text = displayDirections[characters[i]];
            }
        }
        //Creates displayable field for QTE task
        void SpawnObject(int id)
        {
            var go = Instantiate(prefab,_rectTransform);
            RectTransform goTransform = go.GetComponent<RectTransform>();
            goTransform.anchoredPosition = new Vector2((gridOffset*id), 0);
            go.name = id.ToString();
            text[id] = go.GetComponent<TextMeshProUGUI>();

        }
        //Change color of current(ly required) character
        public void CurrentCharacter(int id)
        {
            text[id].color = Color.gold;
        }
        //Marks character if Succeeded 
        public void SuccessCharacter(int id)
        {
           text[id].color = Color.green;
        }

        public void Clear()
        {
            foreach (var textMeshProUGUI in text)
            {
                Destroy(textMeshProUGUI.gameObject);
            }
        }
        
    }
}