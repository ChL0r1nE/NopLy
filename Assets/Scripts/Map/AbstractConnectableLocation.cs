using System.Collections.Generic;

namespace Map
{
    public abstract class AbstractConnectableLocation : AbstractLocation
    {
        public List<int> CaravanTargetsID = new();

        public abstract void UnitInteract(ref int id, ref int count);

        protected abstract void UpdateTargetsID();
    }
}
