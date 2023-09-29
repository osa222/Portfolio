using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Game
{

    public interface IInputEventProvider
    {
        public Vector2 Turn { get; }
    }
}