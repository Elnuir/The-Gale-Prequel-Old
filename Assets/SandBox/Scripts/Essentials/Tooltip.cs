using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform TooltipObject;
    public Vector3 Offset = new Vector3(0, 0, 10);
    private bool isHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }

    private void OnGUI()
    {
        Debug.Log(gameObject +"AAAAAAAAAAAA");
        if (TooltipObject?.gameObject == null)
            ;
        TooltipObject.gameObject.SetActive(isHover);

        var screenPoint = Input.mousePosition;
        TooltipObject.transform.position = screenPoint + Offset;
    }
}
