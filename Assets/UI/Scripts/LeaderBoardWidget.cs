using Leaderboard;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LeaderBoardWidget : BaseWidget
    {
        [SerializeField] private LeaderboardField _leaderboardFieldPrefab;
        [SerializeField] private RectTransform _container;
        [SerializeField] private Button _close;
        [SerializeField] private VerticalLayoutGroup _layoutGroup;
        [SerializeField] private float _spacing;

        [Inject] private ILeaderboardSystem _leaderboardSystem;

        public override async void OnCreated()
        {
            await Awaiters.Until(() => _leaderboardSystem.Initialized);

            var leaders = _leaderboardSystem.AllUsers;

            _layoutGroup.spacing = _spacing;
            _container.sizeDelta = new Vector2(_container.sizeDelta.x, leaders.Count * (_spacing + _leaderboardFieldPrefab.Height));

            foreach (var model in leaders)
                Instantiate(_leaderboardFieldPrefab, _container).Init(model);

            _close.onClick.AddListener(CloseClickHandler);
        }

        public override void OnClosed() => _close.onClick.RemoveListener(CloseClickHandler);

        private void CloseClickHandler() => Closed?.Invoke(this);
    }
}