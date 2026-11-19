using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUpgradePopup : MonoBehaviour
{
    [SerializeField] private UIDocument _hudDocument;
    [SerializeField] private SpriteRenderer _playerRenderer;
    [SerializeField] private Camera _gameplayCamera;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _headClearance = 0.5f;
    private Label _label;
    private float _expiresAt;
    private bool _showing;

    private void OnEnable()
    {
        PlayerProgress.AbilityUnlocked += Show;
    }

    private void Start()
    {
        if (_playerRenderer == null && PlayerController.Instance != null)
            _playerRenderer = PlayerController.Instance.GetComponent<SpriteRenderer>();
        if (_gameplayCamera == null) _gameplayCamera = Camera.main;
        BindLabel();
    }

    private void OnDisable()
    {
        PlayerProgress.AbilityUnlocked -= Show;
        _showing = false;
        if (_label != null) _label.AddToClassList("upgrade-popup-hidden");
        _label = null;
    }

    private void BindLabel()
    {
        if (_hudDocument != null)
            _label = _hudDocument.rootVisualElement.Q<Label>("upgradeUnlockedLabel");
    }

    private void Show()
    {
        if (_label == null || _label.panel == null) BindLabel();
        _expiresAt = Time.unscaledTime + _duration;
        _showing = true;
        UpdatePosition();
    }

    private void LateUpdate()
    {
        if (_showing) UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (_label == null || _label.panel == null) BindLabel();
        if (_label == null) return;
        if (Time.unscaledTime >= _expiresAt || _playerRenderer == null ||
            !_playerRenderer.gameObject.activeInHierarchy || _gameplayCamera == null)
        {
            _showing = false;
            _label.AddToClassList("upgrade-popup-hidden");
            return;
        }
        Bounds bounds = _playerRenderer.bounds;
        Vector3 anchor = new Vector3(bounds.center.x, bounds.max.y + _headClearance, bounds.center.z);
        Vector3 viewport = _gameplayCamera.WorldToViewportPoint(anchor);
        bool visible = viewport.z > 0 && viewport.x >= 0 && viewport.x <= 1 && viewport.y >= 0 && viewport.y <= 1;
        _label.EnableInClassList("upgrade-popup-hidden", !visible);
        if (!visible || _label.panel == null) return;
        Vector2 point = RuntimePanelUtils.CameraTransformWorldToPanel(_label.panel, anchor, _gameplayCamera);
        point = _label.parent.WorldToLocal(point);
        // Only dynamic position is assigned in code; presentation is in USS.
        float width = float.IsNaN(_label.resolvedStyle.width) ? 0f : _label.resolvedStyle.width;
        float height = float.IsNaN(_label.resolvedStyle.height) ? 0f : _label.resolvedStyle.height;
        _label.style.translate = new Translate(point.x - width * 0.5f, point.y - height);
    }
}
