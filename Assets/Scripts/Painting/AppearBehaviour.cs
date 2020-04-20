using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private float _startTime;
    private float _target = 0;
    private float _start = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var material = GetComponent<Renderer>().sharedMaterial;

        material.SetFloat("_LastAppearValue", Mathf.Clamp(Mathf.Pow(Time.time - _startTime, 2) + _start, _start, _target));
        material.SetFloat("_AppearValue", Mathf.Clamp(Time.time - _startTime + _start, _start, _target));

        var texture = GetComponentInParent<GalleryTextureSelector>().CurrentTexture;
        if (material.mainTexture != texture && texture != null)
            material.mainTexture = texture;
    }

    public void DoAppear()
    {
        _startTime = Time.time;
        _start = _target;
        _target += 0.4f;
    }

    public void Clear()
    {
        _target = 0;
    }
}
