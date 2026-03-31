using System.Collections.Generic;
using UnityEngine;

public class TractorPlayer : MonoBehaviour
{
    public float playbackSpeed = 1f;
    public float positionScale = 1f;

    private List<ValtraRecord> _records;
    private int _currentIndex;
    private float _currentTime;
    private bool _isPlaying;
    private bool _fileLoaded;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!_fileLoaded)
            {
                LoadFile();
            }
            else
            {
                _isPlaying = !_isPlaying;
            }
        }

        if (_isPlaying && _records != null && _records.Count > 1)
        {
            Play();
        }
    }

    private void LoadFile()
    {
        // Временно — путь вручную. Позже заменишь на FileBrowser.
        string path = Application.dataPath + "/imu.txt";

        _records = ValtraParser.Parse(path);
        if (_records.Count == 0)
            return;

        _fileLoaded = true;
        _currentIndex = 0;
        _currentTime = _records[0].gpsTime;

        SetPosition(_records[0].easting, _records[0].northing);
        _isPlaying = true;
    }

    private void Play()
    {
        _currentTime += Time.deltaTime * playbackSpeed;

        while (_currentIndex < _records.Count - 2 &&
               _currentTime > _records[_currentIndex + 1].gpsTime)
        {
            _currentIndex++;
        }

        var a = _records[_currentIndex];
        var b = _records[_currentIndex + 1];

        float t = Mathf.InverseLerp(a.gpsTime, b.gpsTime, _currentTime);
        double e = Mathf.Lerp((float)a.easting, (float)b.easting, t);
        double n = Mathf.Lerp((float)a.northing, (float)b.northing, t);

        SetPosition(e, n);

        if (_currentIndex >= _records.Count - 2 && _currentTime >= b.gpsTime)
            _isPlaying = false;
    }

    private void SetPosition(double e, double n)
    {
        transform.position = new Vector3(
            (float)e * positionScale,
            transform.position.y,
            (float)n * positionScale
        );
    }
}
