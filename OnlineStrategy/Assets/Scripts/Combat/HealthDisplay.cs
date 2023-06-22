using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private GameObject _healthBarParent;
    [SerializeField] private Image _healthBarImage;

    private void Awake()
    {
        _health.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDestroy()
    {
        _health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        _healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }


    // Show / Hide health ui
    private void OnMouseEnter()
    {
        _healthBarParent.SetActive(true);
    }

    private void OnMouseExit()
    {
        _healthBarParent.SetActive(false);
    }
}
