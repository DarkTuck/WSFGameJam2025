using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        private GameObject[] arrows;
        //this dictionary is responsible for translating Vector2 input of QTE to visuals (currently characters)
        //can be changed if needed to images
        private Dictionary<Vector2, float> displayDirections = new Dictionary<Vector2, float>
        {
            { Vector2.up, 0f},
            { Vector2.down, 180f },
            { Vector2.left, 90f },
            { Vector2.right, 270f }
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
            arrows= new GameObject[characters.Length];
            for (int i = 0; i < arrows.Length; i++)
            {
                Vector2 spawnPosition = new Vector2(i*gridOffset, 0)+origin;
                SpawnObject(i,characters[i]);
                //arrows[i].transform.rotation = new Quaternion(0,0,displayDirections[characters[i]],0);
            }
        }
        //Creates displayable field for QTE task
        void SpawnObject(int id,Vector2 character)
        {
            var go = Instantiate(prefab,_rectTransform);
            RectTransform goTransform = go.GetComponent<RectTransform>();
            goTransform.anchoredPosition = new Vector2((gridOffset*id), 0);
            goTransform.rotation = Quaternion.Euler(0f, 0f, displayDirections[character]);
            go.name = id.ToString();
            arrows[id] = go;

        }
        //Change color of current(ly required) character
        public void CurrentCharacter(int id)
        {
            arrows[id].GetComponent<Image>().color = Color.gold;
        }
        //Marks character if Succeeded 
        public void SuccessCharacter(int id)
        {
            arrows[id].GetComponent<Image>().color = Color.green;
        }

        public void Clear()
        {
            foreach (var arrow in arrows)
            {
                Destroy(arrow.gameObject);
            }
        }
        public void Restart()
        {
            CurrentCharacter(0);
            for (int i = 1; i < arrows.Length; i++)
            {
                arrows[i].GetComponent<Image>().color  = Color.white;
            }
        }
    }
}