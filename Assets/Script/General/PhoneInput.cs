using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneInput : EventTrigger
{

    // Start is called before the first frame update
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Debug.Log("按下" + this.gameObject.name);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        Debug.Log("抬起" + this.gameObject.name);
    }
}
