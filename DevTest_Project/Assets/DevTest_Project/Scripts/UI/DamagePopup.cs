using TMPro;
using UnityEngine;

// --------------------------------------------------------------------------
// Name: DamagePopup (Class)
// Desc: This method represents an popup value asset, that realize an visual
//       feedback on the game screen.
// --------------------------------------------------------------------------
public class DamagePopup : MonoBehaviour
{
    //Public Data
    [HideInInspector] public TextMeshProUGUI textDisplay => GetComponent<TextMeshProUGUI>();

    // ----------------------------------------------------------------------
    // Name: SetUp (Method)
    // Desc: This method setup the popup action, setting his text value and
    //       his color.
    // ----------------------------------------------------------------------
    public void SetUp(int damageValue, bool isDamage)
    {
        textDisplay.text = damageValue.ToString();
        textDisplay.color = isDamage ? Color.red : Color.yellow;
    }
    // ----------------------------------------------------------------------
    // Name: SetUp (Method)
    // Desc: This method simply deactivate the object in an certain time
    //       (This method is used an animation event on the correct animation).
    //       
    // ----------------------------------------------------------------------
    public void PopUpEnd() => transform.parent.gameObject.SetActive(false);
}