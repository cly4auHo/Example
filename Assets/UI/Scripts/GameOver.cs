using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOver : BaseWidgetWithData<bool>
    {
        [SerializeField] private GameObject _newRecordContainer;
        [SerializeField] private Button _cancel;
        [SerializeField] private Button _submit;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _back;
        
        public override void OnCreated()
        {
            _newRecordContainer.SetActive(Data);
            _back.gameObject.SetActive(!Data);
            
            _cancel.onClick.AddListener(CancelClickHandler);
            _submit.onClick.AddListener(SubmitClickHandler);
            _back.onClick.AddListener(CancelClickHandler);
        }

        public override void OnClosed()
        {
            _cancel.onClick.RemoveListener(CancelClickHandler);
            _submit.onClick.RemoveListener(SubmitClickHandler);
            _back.onClick.RemoveListener(CancelClickHandler);
        }

        private void SubmitClickHandler()
        {
            _gameManager.Submit(string.IsNullOrEmpty(_inputField.text) ? "User" : _inputField.text);
            Close();
        }

        private void CancelClickHandler() => Close();

        private void Close()
        {
            _gameManager.Restart();
            Closed?.Invoke(this);
        }
    }
}