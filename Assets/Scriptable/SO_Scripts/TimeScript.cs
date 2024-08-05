using System;
using UnityEngine;

namespace InventorySpace
{
    public class TimeScript : MonoBehaviour
    {
        private float _startTime;
        private float _duration = 600f;

        private void Start()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - _startTime >= _duration)
            {
                Debug.Log("Done");

                this.enabled = false;
            }
        }
    }
}