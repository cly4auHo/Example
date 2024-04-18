using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Action<string> Submit;
    public Action Cancel;

    [SerializeField] GameObject _newRecordContainer;
    [SerializeField] Button _cancel;
    [SerializeField] Button _submit;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Button _back;
    
    public void Init(in bool isNewRecord)
    {
        _newRecordContainer.SetActive(isNewRecord);
        _back.gameObject.SetActive(!isNewRecord);
        
        if (isNewRecord)
        {
            _cancel.onClick.AddListener(CancelClickHandler);
            _submit.onClick.AddListener(SubmitClickHandler);
        }
        else
        {
            _back.onClick.AddListener(CancelClickHandler);
        }
    }

    public void Dispose()
    {
        _cancel.onClick.RemoveListener(CancelClickHandler);
        _submit.onClick.RemoveListener(SubmitClickHandler);
        _back.onClick.RemoveListener(CancelClickHandler);
    }

    void CancelClickHandler() => Cancel?.Invoke();

    void SubmitClickHandler() => Submit?.Invoke(string.IsNullOrEmpty(_inputField.text) ? "User" : _inputField.text);
}
