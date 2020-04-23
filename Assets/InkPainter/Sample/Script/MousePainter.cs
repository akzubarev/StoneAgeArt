using UnityEngine;
using UnityEngine.UI;

namespace Es.InkPainter.Sample
{
    public class MousePainter : MonoBehaviour
    {
        /// <summary>
        /// Types of methods used to paint.
        /// </summary>
        [System.Serializable]
        private enum UseMethodType
        {
            RaycastHitInfo,
            WorldPoint,
            NearestSurfacePoint,
            DirectUV,
        }
        public GameObject brushobject, target, slider;

        [SerializeField]
        public Brush brush;

        [SerializeField]
        private UseMethodType useMethodType = UseMethodType.RaycastHitInfo;

        [SerializeField]
        bool erase = false;

        float timepressed = 0;
        float timetopass = 1;
        public bool isEnabled = true;

        void Start()
        {
            Input.multiTouchEnabled = false;
        }

        private void Update()
        {
            if (isEnabled)
                if (Input.GetMouseButton(0))
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    bool success = true;
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        var paintObject = hitInfo.transform.GetComponent<InkCanvas>();
                        hitInfo.point = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.6f, hitInfo.point.z);
                        brushobject.transform.position = hitInfo.point;//new Vector3( hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);

                        if (timepressed < timetopass)
                        {
                            timepressed += Time.deltaTime;
                            slider.GetComponent<Slider>().value = timepressed;
                            target.SetActive(true);
                            slider.SetActive(true);
                        }
                        else
                        {
                            target.SetActive(false);
                            slider.SetActive(false);
                            slider.GetComponent<Slider>().value = 0;
                            if (paintObject != null)
                                switch (useMethodType)
                                {
                                    case UseMethodType.RaycastHitInfo:
                                        success = erase ? paintObject.Erase(brush, hitInfo) : paintObject.Paint(brush, hitInfo);
                                        break;

                                    case UseMethodType.WorldPoint:
                                        success = erase ? paintObject.Erase(brush, hitInfo.point) : paintObject.Paint(brush, hitInfo.point);
                                        break;

                                    case UseMethodType.NearestSurfacePoint:
                                        success = erase ? paintObject.EraseNearestTriangleSurface(brush, hitInfo.point) : paintObject.PaintNearestTriangleSurface(brush, hitInfo.point);
                                        break;

                                    case UseMethodType.DirectUV:
                                        if (!(hitInfo.collider is MeshCollider))
                                            Debug.LogWarning("Raycast may be unexpected if you do not use MeshCollider.");
                                        success = erase ? paintObject.EraseUVDirect(brush, hitInfo.textureCoord) : paintObject.PaintUVDirect(brush, hitInfo.textureCoord);
                                        break;
                                }
                            if (!success)
                                Debug.LogError("Failed to paint.");
                        }
                    }
                }
                else
                {
                    timepressed = 0;
                    target.SetActive(false);
                    slider.SetActive(false);
                    slider.GetComponent<Slider>().value = 0;
                }
        }
        /*
		public void OnGUI()
		{
			if(GUILayout.Button("Reset"))
			{
				foreach(var canvas in FindObjectsOfType<InkCanvas>())
					canvas.ResetPaint();
			}
		}
        */

        public void Clear()
        {
            foreach (var canvas in FindObjectsOfType<InkCanvas>())
                canvas.ResetPaint();
        }
    }
}