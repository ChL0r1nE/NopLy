using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class MapLocation : MonoBehaviour
    {
        public void LoadLocation() => _player.LoadLocation();

        public void Open() => _isOpen = true;

        [SerializeField] private Image[] _images;
        [SerializeField] private Text[] _textes;

        [SerializeField] private GameObject _enemyMenu;
        [SerializeField] private GameObject _cityMenu;
        [SerializeField] private Image _locationImage;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _subText;
        [SerializeField] private Map.Player _player;
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
            _player.Exit();
        }

        public void SetLocationImage(Sprite sprite, string locationName)
        {
            _nameText.text = locationName;
            _locationImage.sprite = sprite;
        }

        public void SetProductMenu(string st)
        {
            if (_isOpen) return;

            _oldMenu?.SetActive(false);
            _enemyMenu.SetActive(true);
            _oldMenu = _enemyMenu;

            _subText.text = st;
        }

        public void SetBildingMenu(string st2)
        {
            if (_isOpen) return;

            _oldMenu?.SetActive(false);
            _enemyMenu.SetActive(true);
            _oldMenu = _enemyMenu;

            _subText.text = st2;
        }

        public void SetCityMenu(Slot[] slots)
        {
            if (_isOpen) return;

            _oldMenu?.SetActive(false);
            _cityMenu.SetActive(true);
            _oldMenu = _cityMenu;

            for (int i = 0; i < slots.Length; i++)
            {
                _images[i].gameObject.SetActive(slots[i].Item);

                if (!slots[i].Item) continue;

                _images[i].sprite = slots[i].Item.Sprite;
                _textes[i].text = slots[i].Count != 1 ? $"{slots[i].Count}" : "";
            }
        }

        public void SetVillageMenu(string product)
        {
            if (_isOpen) return;

            _oldMenu?.SetActive(false);
            _enemyMenu.SetActive(true);
            _oldMenu = _enemyMenu;

            _subText.text = $"Production - {product}";
        }

        public void SetEnemyMenu(string damage)
        {
            if (_isOpen) return;

            _oldMenu?.SetActive(false);
            _enemyMenu.SetActive(true);
            _oldMenu = _enemyMenu;

            _subText.text = damage;
        }
    }
}
