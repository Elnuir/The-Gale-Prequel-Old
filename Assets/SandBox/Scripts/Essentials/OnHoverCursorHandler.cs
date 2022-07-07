using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class
    OnHoverCursorHandler : MonoBehaviour, IPointerEnterHandler,
        IPointerExitHandler // required interface when using the OnPointerEnter method.
{
    private Animator animator;
    private Button button;
    [SerializeField] private GameObject pointer;
    Vector2 reference;

    private float startTick = 0.5f;
    [SerializeField]private bool isHovered;
    private OnHoverCursorHandler[] onHoverCursorHandlers;
    [SerializeField]private bool isMenuScene;

    private void Start()
    {
        onHoverCursorHandlers = FindObjectsOfType<OnHoverCursorHandler>();
    }

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (startTick > -2000)
            startTick -= Time.unscaledDeltaTime;
        if (isHovered && isMenuScene)
        {
            pointer.transform.position = Vector2.SmoothDamp(pointer.transform.position,
                new Vector2(transform.position.x, transform.position.y + 90f), ref reference, 0.2f);
            // if (pointer.transform.position.x - transform.position.x < 0.01f)
            // {
            //     isHovered = false;
            // }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            animator.SetBool("Increased", true);
            if (isMenuScene)
            {
                foreach (var onHoverCursorHandler in onHoverCursorHandlers)
                {
                    onHoverCursorHandler.isHovered = false;
                }
            }
            isHovered = true;
//            print("GOINGOIGN");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Increased", false);
        // if (Mathf.Abs(pointer.transform.position.x - transform.position.x) < 0.01f)
        // {
        //     isHovered = false;
        // }
        //isHovered = false;
    }
    
}