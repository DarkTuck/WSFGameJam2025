using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace _Scripts
{
    public class QTEDisplay : MonoBehaviour
    {
        //[SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float gridOffset;
        [SerializeField] private GameObject prefab;
        private TextMeshProUGUI[] text;
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

        void SpawnObject(int id)
        {
            var go = Instantiate(prefab,_rectTransform);
            RectTransform goTransform = go.GetComponent<RectTransform>();
            goTransform.anchoredPosition = new Vector2((gridOffset*id), 0);
            go.name = id.ToString();
            text[id] = go.GetComponent<TextMeshProUGUI>();

        }

        public void CurrentCharacter(int id)
        {
            text[id].color = Color.gold;
        }

        public void SuccessCharacter(int id)
        {
           text[id].color = Color.green;
        }
        
    }
}