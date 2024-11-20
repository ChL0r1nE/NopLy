using System.Collections.Generic;

namespace Map
{
    public abstract class AbstractConnectableLocation : AbstractLocation
    {
        public List<int> TargetsID = new();

        public abstract void SetCargo(int id, int count);

        protected abstract void SetTargetsID();
    }
}
