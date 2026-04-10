using UnityEngine;
using ValtraIMU.Models;

public class TractorPlayer : MonoBehaviour
{
    [Header("Playback Settings")]
    public float playbackSpeed = 1f;
    public float positionScale = 1f;

    [Header("Wheel Rotation")]
    public WheelRotator wheelRotator;

    private ValtraIMU.DataProviders.IMUDataProvider _imuDataProvider;
    private float _currentTime;
    private bool _isPlaying;
    private bool _fileLoaded;

    private double _initialEasting;
    private double _initialNorthing;
    private float _initialTractorX;
    private float _initialTractorZ;

    private float _speed;

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current.enterKey.wasPressedThisFrame)
#else
        if (Input.GetKeyDown(KeyCode.Return))
#endif
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

        if (_isPlaying && _imuDataProvider != null)
        {
            Play();
        }
    }

    private void LoadFile()
    {
        string path = Application.dataPath + "/Data/valtra_mls_20250515_1.txt";
        _imuDataProvider = ValtraIMU.DataProviders.IMUDataProvider.Create(path);

        if (_imuDataProvider == null)
        {
            Debug.LogError($"File not found: {path}");
            return;
        }

        _fileLoaded = true;
        _currentTime = 0;

        var firstRecord = _imuDataProvider.Current;
        _initialEasting = firstRecord.Position.Easting;
        _initialNorthing = firstRecord.Position.Northing;
        _initialTractorX = transform.position.x;
        _initialTractorZ = transform.position.z;

        Debug.Log("Start");

        _isPlaying = true;
    }

    private void Play()
    {
        _currentTime += Time.deltaTime * playbackSpeed;

        IMUData a = _imuDataProvider.Current;
        IMUData b = null;

        while (true)
        {
            if (!_imuDataProvider.MoveNext())
            {
                _isPlaying = false;
                Debug.Log("Stop");
                return;
            }

            b = _imuDataProvider.Current;
            if (_currentTime <= b.Time)
            {
                break;
            }

            b = a;
        }

        float t = Mathf.InverseLerp((float)a.Time, (float)b.Time, _currentTime);

        double e = Mathf.Lerp((float)a.Position.Easting, (float)b.Position.Easting, t);
        double n = Mathf.Lerp((float)a.Position.Northing, (float)b.Position.Northing, t);

        SetPosition(e, n);
        SetRotation(a, b, t);

        // ---------------------------------------------------
        // ВЫЧИСЛЕНИЕ СКОРОСТИ ПО IMU (правильный способ)
        // ---------------------------------------------------
        double de = b.Position.Easting - a.Position.Easting;
        double dn = b.Position.Northing - a.Position.Northing;
        double dt = b.Time - a.Time; // миллисекунды

        if (dt > 0)
        {
            double dist = Mathf.Sqrt((float)(de * de + dn * dn));
            _speed = (float)(dist / (dt / 1000.0)); // м/с
        }
        else
        {
            _speed = 0;
        }

        if (wheelRotator != null)
            wheelRotator.SetSpeed(_speed);
    }

    private void SetPosition(double e, double n)
    {
        transform.position = new Vector3(
            (float)(e - _initialEasting) * positionScale + _initialTractorX,
            transform.position.y,
            (float)(n - _initialNorthing) * positionScale + _initialTractorZ
        );
    }

    private void SetRotation(IMUData a, IMUData b, float t)
    {
        double roll = Mathf.Lerp((float)a.Orientation.Roll, (float)b.Orientation.Roll, t);
        double pitch = Mathf.Lerp((float)a.Orientation.Pitch, (float)b.Orientation.Pitch, t);
        double heading = Mathf.Lerp((float)a.Orientation.Heading, (float)b.Orientation.Heading, t);

        Quaternion imuRot = Quaternion.Euler(
            (float)pitch,
            (float)heading,
            (float)roll
        );

        transform.rotation = imuRot;
    }
}
