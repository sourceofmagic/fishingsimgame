using UnityEngine;

public class ControlPanelScript : MonoBehaviour
{
    public GameObject panel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (panel != null)
            {
                panel.SetActive(true);
                SwitchControl.Instance.SwitchToFishing();
                Cursor.visible = true;
            }
        }
    }
}
