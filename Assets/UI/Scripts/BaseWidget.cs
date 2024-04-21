using System;
using GameManagment;
using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class BaseWidget : MonoBehaviour
    {
        public Action<BaseWidget> Closed;

        [Inject] protected IGameManager _gameManager;

        public abstract void OnCreated();
        public abstract void OnClosed();
    }

    public abstract class BaseWidgetWithData<T> : BaseWidget
    {
        public T Data { get; set; }
    }
}