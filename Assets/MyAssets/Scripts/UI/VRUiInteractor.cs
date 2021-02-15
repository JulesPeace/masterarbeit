using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRUiInteractor: MonoBehaviour
{
    private Button selectedButton = null;

    public void PressButton()
    {
        if(selectedButton != null)
        {
            selectedButton.onClick.Invoke();
        }
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {        
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 5))
        {
            // Wenn das getroffene Objekt einen Button besitzt, wähle ihn aus!
            Button button = hitInfo.collider.GetComponent<Button>();
            if (button != null)
            {
                button.Select();
                selectedButton = button;
            }
        }
        // Beispiel-Aufruf von PressButton, klappt für PC und GearVR
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            PressButton();
        }
    }
}
