﻿using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace Stone
{
    public class StoneController: MonoBehaviour
    {
        #region Dependency

        private IStoneClickEventsInvoke _stoneClickEvents;
        
        [Inject]
        private void Construct(IStoneClickEventsInvoke stoneClickEvents)
        {
            _stoneClickEvents = stoneClickEvents;
        }

        #endregion
        
        public void OnObjectClick()
        {
            Debug.Log("OnObjectClick");
            _stoneClickEvents.OnStoneClickInvoke();
        }
    }
}