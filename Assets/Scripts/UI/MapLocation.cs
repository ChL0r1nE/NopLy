using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class MapLocation : MonoBehaviour
    {
        public void LoadLocation() => Map.Player.Static.LoadLocation();

        public void Open() => _isOpen = true;

        [Header("TownUI")]
        [SerializeField] private GameObject _townPanel;
        [SerializeField] private Image[] _images;
        [SerializeField] private Text[] _textes;

        [Header("ProductionUI")]
        [SerializeField] private GameObject _productionPanel;
        [SerializeField] private Image _baseProductImage;
        [SerializeField] private Image _resultProductImage;
        [SerializeField] private Text _baseProductCount;
        [SerializeField] private Text _resultProductCount;

        [Header("EnemyUI")]
        [SerializeField] private GameObject _enemyPanel;

        [Header("")]
        [SerializeField] private Image _locationImage;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _subText;
        private GameObject _oldMenu;
        private RectTransform _rectTransform;
        private Vector3 _position;
        private bool _isOpen = false;

        private void Start() => _rectTransform = GetComponent<RectTransform>();

        private void Update()
        {
            _position.x = Mathf.MoveTowards(_rectTransform.anchoredPosition.x, _isOpen ? -300f : 300f, Time.deltaTime * 2500);
            _rectTransform.anchoredPosition = _position;
        }

        public void Close()
        {
            _isOpen = false;
            Map.Player.Static.Exit();
        }

        public void SetLocationHead(Sprite sprite, string locationName)
        {
            _locationImage.sprite = sprite;
            _nameText.text = locationName;
        }

        public void SetTownMenu(Slot[] slots)
        {
            ChangePanel(_townPanel);

            for (int i = 0; i < slots.Length; i++)
            {
                _images[i].gameObject.SetActive(slots[i].Item);

                if (!slots[i].Item) continue;

                _images[i].sprite = slots[i].Item.Sprite;
                _textes[i].text = slots[i].Count != 1 ? $"{slots[i].Count}" : "";
            }
        }

        public void SetProductionMenu(Sprite resultProductSprite, int resultProductCount)
        {
            ChangePanel(_productionPanel);

            _baseProductImage.enabled = false;
            _baseProductCount.enabled = false;
            _resultProductImage.sprite = resultProductSprite;
            _resultProductCount.text = resultProductCount.ToString();
        }

        public void SetProductionMenu(Sprite baseProductSprite, int baseProductCount, Sprite resultProductSprite, int resultProductCount)
        {
            ChangePanel(_productionPanel);

            _baseProductImage.enabled = true;
            _baseProductCount.enabled = true;
            _baseProductImage.sprite = baseProductSprite;
            _baseProductCount.text = baseProductCount.ToString();

            _resultProductImage.sprite = resultProductSprite;
            _resultProductCount.text = resultProductCount.ToString();
        }

        public void SetEnemyMenu(string damage)
        {
            ChangePanel(_enemyPanel);

            _subText.text = damage;
        }

        private void ChangePanel(GameObject newPanel)
        {
            _oldMenu?.SetActive(false);
            newPanel.SetActive(true);
            _oldMenu = newPanel;
        }
    }
}
