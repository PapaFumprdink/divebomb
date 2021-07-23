using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class CursorObject : MonoBehaviour
{
    [SerializeField] private Image m_CursorRenderer;
    [SerializeField] private Sprite m_DefaultSprite;

    private Camera m_MainCamera;

    public Sprite CursorSprite { get; set; }
    public float FillProgress { get; set; }

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // Set the position to the mouse position within our parent canvas.
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_CursorRenderer.canvas.transform as RectTransform, mousePosition, m_MainCamera, out Vector2 cursorPoint);
        transform.localPosition = cursorPoint;

        // Update the sprite and fill amount.
        m_CursorRenderer.sprite = CursorSprite;
        m_CursorRenderer.fillAmount = FillProgress;

        // Reset the properties.
        FillProgress = 1f;
        CursorSprite = m_DefaultSprite;
    }

    private void OnEnable()
    {
        // Hide the windows cursor, as this object is displaying over it.
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        // Show the windows cursor, as we are now hidden.
        Cursor.visible = true;
    }
}
