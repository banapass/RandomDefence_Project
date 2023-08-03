namespace framework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;
    using System;

    abstract public class BaseUi : MonoBehaviour
    {
        public string UiPath { get; private set; }
        public virtual void OnOpen() { }
        public virtual void OnClose(TweenCallback _onComplete) { }

        public void SetUIPath(string _name) => UiPath = _name;
    }

}