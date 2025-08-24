// Filename: BusinessView.cs
// Location: _Project/Scripts/Views/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This component lives on each business UI element.
public class BusinessView : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text LevelText;
    public TMP_Text IncomeText;
    public Slider ProgressBar;
    public Button LevelUpButton;
    public TMP_Text LevelUpButtonText;

    public Button Upgrade1Button;
    public TMP_Text Upgrade1ButtonText;
    public Button Upgrade2Button;
    public TMP_Text Upgrade2ButtonText;
}