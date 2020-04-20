using UnityEngine;

public class GalleryTextureSelector : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D _texture;
    public Texture2D CurrentTexture => _texture;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        var permission = NativeGallery.CheckPermission();
        if (permission == NativeGallery.Permission.Granted)
            NativeGallery.GetImageFromGallery(OnImageSelected);
        else
            NativeGallery.RequestPermission();
    }

    private void OnImageSelected(string path)
    {
        _texture = NativeGallery.LoadImageAtPath(path);
    }
}
