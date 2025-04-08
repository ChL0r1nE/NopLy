using System.Collections.Generic;

namespace Map
{
    public abstract class AbstractConnectableLocation : AbstractLocation
    {
        public List<int> CaravanTargetsID = new();

        public abstract void UnitInteract(int id, int count);

        protected abstract void UpdateTargetsID();
    }
}
