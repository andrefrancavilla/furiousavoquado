using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public TextMeshProUGUI credits;

    public void UpdateCredits(int amount)
    {
        credits.text = amount.ToString("00");
    }
}
