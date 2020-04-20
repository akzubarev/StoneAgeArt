using UnityEngine;

public class RenderToTexture : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Material material;
    [SerializeField] private int width = 64;
    [SerializeField] private int height = 64;
    [SerializeField] private bool debugPreview;

    private RenderTexture _renderTexture;
    private Texture2D _texture2D;

    public Texture2D Texture => _texture2D;

    private void Awake()
    {
        _renderTexture = new RenderTexture(width, height, 0);
        _renderTexture.Create();

        _texture2D = new Texture2D(width, height);
    }

    void OnGUI()
    {
        if (debugPreview)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
                Graphics.DrawTexture(new Rect(0, 0, _texture2D.width, _texture2D.height), _texture2D);
            }
        }
    }
    
    void Update()
    {
        var sourceTexture = sprite != null ? sprite.texture : GetComponent<Renderer>()?.material.mainTexture;

        if (sourceTexture != null)
        {
            Graphics.Blit(sourceTexture, _renderTexture, material);
            Texture2DFromRenderTexture();
        }
    }

    private void Texture2DFromRenderTexture()
    {
        var previousRT = RenderTexture.active;
        RenderTexture.active = _renderTexture;
        _texture2D.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        _texture2D.Apply();
        RenderTexture.active = previousRT;
    }
}
