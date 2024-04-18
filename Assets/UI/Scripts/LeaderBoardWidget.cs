using Leaderboard;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardWidget : BaseWidget
{
    [SerializeField] LeaderboardField _leaderboardFieldPrefab;
    [SerializeField] RectTransform _container;
    [SerializeField] Button _close;
    [SerializeField] VerticalLayoutGroup _layoutGroup;
    [SerializeField] float _spacing;

    ILeaderboardSystem _leaderboardSystem;
    
    public async void Init(ILeaderboardSystem leaderboardSystem)
    {
        _leaderboardSystem = leaderboardSystem;

        await Awaiters.Until(() => _leaderboardSystem.Initialized);
        
        var leaders = _leaderboardSystem.AllUsers;
        
        _layoutGroup.spacing = _spacing;
        _container.sizeDelta = new Vector2(_container.sizeDelta.x, leaders.Count * (_spacing + _leaderboardFieldPrefab.Height));
        
        foreach (var model in leaders)
            Instantiate(_leaderboardFieldPrefab, _container).Init(model);
        
        _close.onClick.AddListener(CloseClickHandler);
    }

    void CloseClickHandler()
    {
        Closed?.Invoke(this);
        _close.onClick.RemoveListener(CloseClickHandler);
        Destroy(gameObject);
    }
}
