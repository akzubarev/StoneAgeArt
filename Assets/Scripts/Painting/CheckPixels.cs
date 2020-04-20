using System.Collections;
using UnityEngine;

public class CheckPixels : MonoBehaviour
{
    private const int MAX_STEPS = 10000;
    private const float AUTO_CHECK_DELAY_S = 1.0f;

    [SerializeField] private bool debugShowStats;

    private Texture2D _texture;
    private int _currentPixelIndex = 0;
    private bool _isCheckDone;

    private int _goodPixelCount;
    private int _badPixelCount;
    private int _incompletedPixelCount;

    private int _lastGoodPixelCount;
    private int _lastBadPixelCount;
    private int _lastIncompletedPixelCount;

    private Rect _debugWindowRect = new Rect(0, 0, 300, 100);

    public int GoodPixelCount => _lastGoodPixelCount;
    public int BadPixelCount => _lastBadPixelCount;
    public int IncompletedPixelCount => _lastIncompletedPixelCount;

    void Start()
    {
        _texture = GetComponent<RenderToTexture>().Texture;
    }

    private void OnEnable()
    {
        _isCheckDone = true;

        if (AUTO_CHECK_DELAY_S > 0)
            StartCoroutine(AutoCheck());
    }

    private IEnumerator AutoCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(AUTO_CHECK_DELAY_S);
            Check(true);
        }
    }

    void Update()
    {
        if (!_isCheckDone)
            ProcessCheck();
    }

    private void OnGUI()
    {
        if (debugShowStats)
        {
           
            _debugWindowRect = GUI.Window(0, _debugWindowRect, 
            x => {
                float total = GoodPixelCount + IncompletedPixelCount;
                var result = string.Format("Good: {0}\nIncompleted: {1}\nBad: {2}",
                    (GoodPixelCount / total).ToString("P"), (IncompletedPixelCount / total).ToString("P"), (BadPixelCount / total).ToString("P"));
    
                GUI.Label(new Rect(0, 0, 150, 70), result);
                GUI.DragWindow();
            }, 
            "Check pixels stats");
           
        }

    }

    public void Check(bool waitForPreviousCheck)
    {
        if (waitForPreviousCheck && !_isCheckDone)
            return;

        _goodPixelCount = _incompletedPixelCount = _badPixelCount = 0;
        _isCheckDone = false;
        _currentPixelIndex = 0;
    }

    private void ProcessCheck()
    {
        for (int i = 0; i < MAX_STEPS; i++)
        {
            int x = _currentPixelIndex % _texture.width;
            int y = _currentPixelIndex / _texture.width;
            if (y > _texture.height)
            {
                _isCheckDone = true;
                _lastGoodPixelCount = _goodPixelCount;
                _lastBadPixelCount = _badPixelCount;
                _lastIncompletedPixelCount = _incompletedPixelCount;
                return;
            }
            _currentPixelIndex++;

            var color = _texture.GetPixel(x, y);
            
            if (color.r > 0)
                _badPixelCount++;
            else if (color.g > 0)
                _goodPixelCount++;
            else if (color.b > 0)
                _incompletedPixelCount++;
        }

    }
}
