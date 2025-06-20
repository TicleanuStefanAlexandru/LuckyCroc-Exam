using UnityEngine;
using UnityEngine.UI;

public class TrainingDummyHealthUI : MonoBehaviour
{
    public TrainingDummy dummy;
    public Image healthBarFill;

    void Update()
    {
        if (dummy != null && healthBarFill != null)
        {
            float fillAmount = Mathf.Clamp01((float)dummy.CurrentHealth / dummy.maxHealth);
            healthBarFill.fillAmount = fillAmount;
        }
    }
}