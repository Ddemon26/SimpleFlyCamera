using System;
using UnityEngine;

namespace Damon.SimpleFlyCamera
{
    [Serializable]
    public class MoveAxis
    {
        public KeyCode Positive;
        public KeyCode Negative;

        public MoveAxis(KeyCode positive, KeyCode negative)
        {
            Positive = positive;
            Negative = negative;
        }

        public static implicit operator float(MoveAxis axis)
        {
            return (Input.GetKey(axis.Positive)
                ? 1.0f : 0.0f) -
                (Input.GetKey(axis.Negative)
                ? 1.0f : 0.0f);
        }
    }

}