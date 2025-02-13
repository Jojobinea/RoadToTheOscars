using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private VotosSO _votosSO;
    [SerializeField] private int _troopValue;
    [SerializeField] private Button _button;
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

    private void Update()
    {
        if(_votosSO.quantVotos >= -_troopValue)
        {
            _button.interactable = true;
        }
        else
        {
            _button.interactable = false;
        }
    }

    void ChangeColor()
    {
        img.color = pressedColor; // Muda a cor quando clicado
        Invoke("ResetColor", 0.1f); // Volta à cor original após 0.5s
        EventManager.OnTroopBoughtTrigger(_troopValue);
    }

    void ResetColor()
    {
        img.color = originalColor;
    }
}