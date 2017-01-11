﻿using System;
using System.Numerics;

namespace RotationTest
{
    /// <summary>
    /// Provides a set of methods to interconvert expressions of 3D rotation.
    /// </summary>
    /// <remarks>
    /// Euler angles depend on combinations of (x, y, z) and (roll, pitch, yaw), and its operation order.
    /// This class operates yaw on y, pitch on x, and roll on z.
    /// System.Numerics also uses these combinations and order.
    /// </remarks>
    public static class NRotationHelper
    {
        public static NEulerAngles ToEulerAngles(this Quaternion q) => Matrix4x4.CreateFromQuaternion(q).ToEulerAngles();

        public static NEulerAngles ToEulerAngles(this Matrix4x4 m) => ToEulerAngles(Vector3.Transform(Vector3.UnitZ, m), Vector3.Transform(Vector3.UnitY, m));

        public static NEulerAngles ToEulerAngles(Vector3 rotatedUnitZ, Vector3 rotatedUnitY)
        {
            var y_yaw = (float)Math.Atan2(rotatedUnitZ.X, rotatedUnitZ.Z);

            var q_yaw_inv = Quaternion.CreateFromAxisAngle(Vector3.UnitY, -y_yaw);
            rotatedUnitZ = Vector3.Transform(rotatedUnitZ, q_yaw_inv);
            rotatedUnitY = Vector3.Transform(rotatedUnitY, q_yaw_inv);

            var x_pitch = (float)Math.Atan2(-rotatedUnitZ.Y, rotatedUnitZ.Z);

            var q_pitch_inv = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -x_pitch);
            rotatedUnitY = Vector3.Transform(rotatedUnitY, q_pitch_inv);

            var z_roll = (float)Math.Atan2(-rotatedUnitY.X, rotatedUnitY.Y);

            return new NEulerAngles { Yaw = y_yaw, Pitch = x_pitch, Roll = z_roll };
        }
    }

    public struct NEulerAngles
    {
        public float Yaw;
        public float Pitch;
        public float Roll;
    }
}
