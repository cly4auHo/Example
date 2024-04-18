using TMPro;
using UnityEngine;

public class LeaderboardField : MonoBehaviour
{
    [SerializeField] RectTransform _transform;
    [SerializeField] TextMeshProUGUI _userName;
    [SerializeField] TextMeshProUGUI _score;

    public float Height => _transform.sizeDelta.y;
    
    public void Init(LeaderboardUserModel model)
    {
        _userName.text = model.name;
        _score.text = $"{model.score}";
    }
}
