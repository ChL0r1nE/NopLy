using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace Map
{
    public class Production : AbstractConnectableLocation
    {
        public override void SetCargo(int id, int count) => ItemCount += count;

        public Info.Item Item;
        public int ItemCount;
        public Info.Item ProductItem;
        public int ProductItemCount;

        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private int _frameCount = 0; //ToTickMachine

        private void Start() => SetTargetsID();

        private void FixedUpdate()
        {
            if (_frameCount++ < 250) return;

            if (Item == null)
                ProductItemCount += 10;
            else
            {
                ItemCount -= 10;
                ProductItemCount += 2;
            }
        }

        protected override void OnDown()
        {
            base.OnDown();
            Slot[] slots = { new Slot(ProductItem, ProductItemCount) };
            _mapLocation.SetCityMenu(slots);
        }

        protected override void SetTargetsID()
        {
            if (!File.Exists($"{Application.persistentDataPath}/LocationsID.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/LocationsID.dat", FileMode.Open);
            int[] ids = (_formatter.Deserialize(_file) as Data.IDArray).IDs;
            _file.Close();

            foreach (int id in ids)
            {
                if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Production production) && (production.Item.ID == ProductItem.ID))
                    TargetsID.Add(id);
                else if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Storage storage))
                    TargetsID.Add(id);
            }
        }

        public int GetCargo(int count)
        {
            int startCount = ProductItemCount;

            ProductItemCount -= count;

            if (ProductItemCount < 0)
                ProductItemCount = 0;

            return startCount - ProductItemCount;
        }
    }
}
