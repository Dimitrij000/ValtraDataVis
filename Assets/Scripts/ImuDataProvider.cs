using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace ValtraIMU.DataProviders
{
    /// <summary>
    /// Reads data from IMU+GNSS data log file
    /// </summary>
    internal class IMUDataProvider : IDataProvider<Models.IMUData>
    {
        public Models.IMUData Current => _nextData ?? throw new Exception();

        /// <summary>Constructor</summary>
        /// <param name="filename">data filename</param>
        public IMUDataProvider(string filename)
        {
            _stream = new StreamReader(filename);

            // Skip comments at the beginning of the file
            var line = _stream.ReadLine();
            while (line != null && !_stream.EndOfStream)
            {
                if (line.Length > 0 && int.TryParse(line[0..1], out int _))
                {
                    _nextData = GetData(line);
                    if (_nextData != null)
                    {
                        _startTime = _nextData.Time;
                        _nextData.Time = 0;
                        break;
                    }
                }

                line = _stream.ReadLine();
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="IMUDataProvider"/> based on the settings.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <returns>an instance of <see cref="IMUDataProvider"/>, or null if the filename is not set or the file does not exist</returns>
        public static IMUDataProvider Create(string filename)
        {
            IMUDataProvider result = null;

            if (File.Exists(filename))
            {
                result = new IMUDataProvider(filename);
            }

            return result;
        }

        #region IDataProvider implementation

        public void Dispose()
        {
            _stream.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            _nextData = GetData();
            return _nextData != null;
        }

        public void Reset()
        {
            _stream.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        public bool Get(long timestamp, out Models.IMUData value)
        {
            bool result;
            while (result = MoveNext())
            {
                if (Current.Time >= timestamp)
                    break;
            }
            value = result ? Current : null;
            return result;
        }

        #endregion

        #region Internal

        object IEnumerator.Current => Current;

        readonly StreamReader _stream;
        readonly double _startTime;

        Models.IMUData _nextData = null;

        private Models.IMUData GetData(string line = null)
        {
            while (!_stream.EndOfStream)
            {
                double[] values = null;

                line ??= _stream.ReadLine();
                if (line == null)
                    break;

                try
                {
                    values = line
                        .Split(' ')
                        .Where(item => !string.IsNullOrWhiteSpace(item))
                        .Select(double.Parse)
                        .ToArray();
                }
                catch (FormatException) { }
                catch { Console.WriteLine($"Invalid data: {line}"); }

                if (values?.Length == 22)
                {
                    var imuData = new Models.IMUData((int)values[0], (float)(values[1] - _startTime),
                        new Models.Coordinates(values[2], values[3], values[4]),
                        new Models.Position(values[5], values[6]),
                        new Models.Orientation(values[7], values[8], values[9]),
                        new Models.Velocity(values[10], values[11], values[12]),
                        new Models.AbsoluteAcceleration(values[13], values[14], values[15]),
                        new Models.BodyAcceleration(values[16], values[17], values[18]),
                        new Models.AngularVelocity(values[19], values[20], values[21])
                    );

                    return imuData;
                }

                line = null;
            }

            return null;
        }

        #endregion
    }
}