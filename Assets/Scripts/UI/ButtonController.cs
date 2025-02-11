using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button btn;
    private Image img;
    public Color pressedColor = Color.red;  // Cor ao pressionar
    private Color originalColor;            // Cor original

    void Start()
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        originalColor = img.color; // Guarda a cor original

        btn.onClick.AddListener(ChangeColor);
    }

    void ChangeColor()
    {
        img.color = pressedColor; // Muda a cor quando clicado
        Invoke("ResetColor", 0.1f); // Volta à cor original após 0.5s
    }

    void ResetColor()
    {
        img.color = originalColor;
    }
}