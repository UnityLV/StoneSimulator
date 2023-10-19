using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LocationGameObjects
{
    public class StoneClickEffectPoints : MonoBehaviour
    {
        private Camera _main;

        public void Awake()
        {
            _main = Camera.main;
        }

        public Vector3 GetPointForEffectOnStone()
        {
            var ray = _main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                Vector3 toCamera = hit.point - transform.position;
                toCamera = (toCamera.normalized);
                return hit.point + toCamera;
            }

            Debug.Log("No hit for STONE");
            return Vector3.zero;
        }
    }
}