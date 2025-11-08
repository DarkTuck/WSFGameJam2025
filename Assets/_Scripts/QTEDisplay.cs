using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace _Scripts
{
    public class QTEDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private Dictionary<Vector2, string> displayDirections = new Dictionary<Vector2, string>
        {
            { Vector2.up, "↑" },
            { Vector2.down, "↓" },
            { Vector2.left, "←" },
            { Vector2.right, "→" }
        };

        public void WriteCharacters(Vector2[] characters)
        {
            text.text = "";
            foreach (Vector2 character in characters)
            {
                text.text += displayDirections[character];
            }
        }

        public void CurrentCharacter(int id)
        {
            //int meshIndex = text.textInfo.characterInfo[id].materialReferenceIndex;
        }

        public void SuccessCharacter(int id)
        {
           // text.textInfo.characterInfo[id].color = Color.green;
        }
        
    }
}