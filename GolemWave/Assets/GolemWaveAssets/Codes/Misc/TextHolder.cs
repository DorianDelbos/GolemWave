using TMPro;
using UnityEngine;

namespace GolemWave
{
    public class TextHolder : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMesh;
        public string Text
        {
            get => textMesh.text;
            set => textMesh.text = value;
        }

        public Color Color
        {
            get => textMesh.color;
            set => textMesh.color = value;
        }

        public float Size
        {
            get => textMesh.fontSize;
            set => textMesh.fontSize = value;
        }
    }
}
