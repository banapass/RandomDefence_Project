namespace framework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<string, BaseUi> uiDict = new Dictionary<string, BaseUi>();
        [SerializeField] Transform pageCanvas;
        [SerializeField] Transform popupCanvas;


        public void Show(string _uiPath, bool _isPopup)
        {
            if (uiDict.ContainsKey(_uiPath))
            {
                Debug.LogError("이미 생성된 UI를 보여주려고 하고있습니다.");
                return;
            }

            BaseUi _rawUI = ResourceStorage.GetResource<BaseUi>(_uiPath);
            Transform _targetParent = _isPopup ? popupCanvas : pageCanvas;

            if (_rawUI != null)
            {
                BaseUi _instUi = Instantiate(_rawUI, _targetParent);
                _instUi.SetUIPath(_uiPath);
                uiDict.Add(_uiPath, _instUi);
                _instUi.OnOpen();
            }

        }
        public void Hide(BaseUi _baseUi)
        {
            if (uiDict.ContainsKey(_baseUi.UiPath))
            {
                uiDict.Remove(_baseUi.UiPath);
                _baseUi.OnClose(() => Destroy(_baseUi.gameObject));
            }
            else
            {
                Debug.LogError("UIManager를 통해 생성이 되지 않은 UI를 닫으려고 하고있습니다");
                return;
            }
        }
    }

}