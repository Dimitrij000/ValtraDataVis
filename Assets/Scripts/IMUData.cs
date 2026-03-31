using System;

namespace ValtraIMU.Models
{
    internal class Angular
    {
        protected static double Deg2Rad(double degrees) => degrees * Math.PI / 180;
    }

    internal class Coordinates
    {
        /// <summary>degress</summary>
        public double Latitude;
        /// <summary>degress</summary>
        public double Longitude;
        /// <summary>meters</summary>
        public double Elevation;
        public Coordinates(double latitude, double longitude, double elevation)
        {
            Latitude = latitude;
            Longitude = longitude;
            Elevation = elevation;
        }
    }

    internal class Position
    {
        /// <summary>meters</summary>
        public double Easting;
        /// <summary>meters</summary>
        public double Northing;
        public Position(double easting, double northing)
        {
            Easting = easting;
            Northing = northing;
        }
    }

    internal class Orientation : Angular
    {
        /// <summary>degrees</summary>
        public double Roll;
        /// <summary>degrees</summary>
        public double Pitch;
        /// <summary>degrees</summary>
        public double Heading;
        public Orientation(double roll, double pitch, double heading)
        {
            Roll = roll;
            Pitch = pitch;
            Heading = heading;
        }
        public Orientation ToRadians() => new(Deg2Rad(Roll), Deg2Rad(Pitch), Deg2Rad(Heading));
    }

    internal class Velocity
    {
        /// <summary>m/s</summary>
        public double East;
        /// <summary>m/s</summary>
        public double North;
        /// <summary>m/s</summary>
        public double Up;
        public Velocity(double east, double north, double up)
        {
            East = east;
            North = north;
            Up = up;
        }
    }

    internal class AbsoluteAcceleration
    {
        /// <summary>m/s^2</summary>
        public double East;
        /// <summary>m/s^2</summary>
        public double North;
        /// <summary>m/s^2</summary>
        public double Up;
        public AbsoluteAcceleration(double east, double north, double up)
        {
            East = east;
            North = north;
            Up = up;
        }
    }

    internal class BodyAcceleration
    {
        /// <summary>m/s^2</summary>
        public double X;
        /// <summary>m/s^2</summary>
        public double Y;
        /// <summary>m/s^2</summary>
        public double Z;
        public BodyAcceleration(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    internal class AngularVelocity : Angular
    {
        /// <summary>degrees/s</summary>
        public double X;
        /// <summary>degrees/s</summary>
        public double Y;
        /// <summary>degrees/s</summary>
        public double Z;
        public AngularVelocity(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public AngularVelocity ToRadians() => new(Deg2Rad(X), Deg2Rad(Y),Deg2Rad(Z));
    }

    internal class IMUData
    {
        public int Week;
        /// <summary>seconds since the start of the session</summary>
        public double Time;
        public Coordinates Coordinates;
        public Position Position;
        public Orientation Orientation;
        public Velocity Velocity;
        public AbsoluteAcceleration AbsoluteAcceleration;
        public BodyAcceleration BodyAcceleration;
        public AngularVelocity AngularVelocity;
        public IMUData(int week, float time, Coordinates coordinates,
            Position position, Orientation orientation,
            Velocity velocity,
            AbsoluteAcceleration absoluteAcceleration,
            BodyAcceleration bodyAcceleration,
            AngularVelocity angularVelocity)
        {
            Week = week;
            Time = time;
            Coordinates = coordinates;
            Position = position;
            Orientation = orientation;
            Velocity = velocity;
            AbsoluteAcceleration = absoluteAcceleration;
            BodyAcceleration = bodyAcceleration;
            AngularVelocity = angularVelocity;
        }
    }
}