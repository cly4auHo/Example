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
        [Inject] IBackgroundSystem _backgroundSystem;
        [Inject] ILeaderboardSystem _leaderboardSystem;
        [Inject] IServerApi _serverApi;
        [Inject] IExampleGenerator _exampleGenerator;
        [Inject] Widget _widget;

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