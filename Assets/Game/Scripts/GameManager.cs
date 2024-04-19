using Background;
using Generator;
using Leaderboard;
using Network;
using UnityEngine;
using Zenject;

namespace Boot
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private IBackgroundSystem _backgroundSystem;
        [Inject] private ILeaderboardSystem _leaderboardSystem;
        [Inject] private IServerApi _serverApi;
        [Inject] private IExampleGenerator _exampleGenerator;
        [Inject] private Widget _widget;

        async void Start()
        {
            _backgroundSystem.Init();
            _leaderboardSystem.Init();

            var model = await _serverApi.GetGameModel();

            _widget.Init(model);

            _exampleGenerator.Init(model.AmountOfAnswers);
        }
    }
}