using Network;
using TMPro;
using UnityEngine;

public class LeaderboardField : MonoBehaviour
{
    [SerializeField] private RectTransform _transform;
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private TextMeshProUGUI _score;

    public float Height => _transform.sizeDelta.y;
    
    public void Init(LeaderboardUserModel model)
    {
        _userName.text = model.name;
        _score.text = $"{model.score}";
    }
}
