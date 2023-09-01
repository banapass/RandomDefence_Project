namespace framework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<string, BaseUi> uiDict = new Dictionary<string, BaseUi>();
        private Queue<BaseUi> popupQueue = new Queue<BaseUi>();

        [SerializeField] Transform pageCanvas;
        [SerializeField] RectTransform pageRect;
        [SerializeField] Transform popupCanvas;
        [SerializeField] RectTransform popupRect;

        public Transform UIParent { get { return pageCanvas; } }

        [field: SerializeField, ReadOnly]
        public BaseUi CurrentPage { get; private set; }

        public void Init()
        {
            uiDict = new Dictionary<string, BaseUi>();
            popupQueue = new Queue<BaseUi>();
        }

        public void Show(string _uiPath, bool _isPopup, System.Action _onOpen = null)
        {
            if (uiDict.ContainsKey(_uiPath))
            {
                Log.Logger.LogError("이미 생성된 UI를 보여주려고 하고있습니다.");
                return;
            }

            ResourceStorage.GetComponentAsset<BaseUi>(_uiPath, _rawUi =>
            {
                Transform _targetParent = _isPopup ? popupRect : pageRect;

                if (!_isPopup)
                {
                    CloseAllUI();
                    // CloseCurrentPage();
                }


                if (_rawUi != null)
                {
                    BaseUi _instUi = Instantiate(_rawUi, _targetParent);
                    _instUi.SetUIPath(_uiPath);
                    uiDict.Add(_uiPath, _instUi);
                    _instUi.OnOpen();
                    _onOpen?.Invoke();

                    if (!_isPopup)
                        CurrentPage = _instUi;
                    else
                        popupQueue.Enqueue(_instUi);
                }
            });



        }
        public void Hide(BaseUi _baseUi)
        {
            if (uiDict.ContainsKey(_baseUi.UiPath))
            {
                uiDict.Remove(_baseUi.UiPath);
                _baseUi.OnClose(OnClosed);
            }
            else
            {
                Debug.LogError("UIManager를 통해 생성이 되지 않은 UI를 닫으려고 하고있습니다");
                return;
            }
        }

        private void CloseCurrentPage()
        {
            if (CurrentPage == null) return;

            uiDict.Remove(CurrentPage.UiPath);
            CurrentPage.OnClose(OnClosed);
            Destroy(CurrentPage.gameObject);
        }
        private void CloseAllUI()
        {
            foreach (var _ui in uiDict)
            {
                BaseUi _baseUi = _ui.Value;
                if (_baseUi == null) continue;

                Destroy(_baseUi.gameObject);
            }
            CurrentPage = null;
            uiDict.Clear();
        }
        private void OnClosed(BaseUi _baseUi)
        {
            if (_baseUi == null) return;
            Destroy(_baseUi.gameObject);
        }
    }

}