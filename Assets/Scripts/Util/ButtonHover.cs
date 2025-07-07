using TMPro;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] private AudioClip hoverSound; // Sound played when the button is hovered over

    public void OnMouseEnterSoundEffect()
    {
        // Play the hover sound effect
        if (hoverSound != null)
        {
            SoundFXManager._instance.PlaySoundFXClip(hoverSound, transform, 1f);
        }
        else
        {
            Debug.LogWarning("Hover sound clip is not assigned in ButtonHover.");
        }
    }

}
