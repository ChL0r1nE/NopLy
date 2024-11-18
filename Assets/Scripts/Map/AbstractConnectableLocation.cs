using System.Collections.Generic;

namespace Map
{
    public abstract class AbstractConnectableLocation : AbstractLocation
    {
        public List<int> TargetsID = new();

        protected abstract void SetTargetsID();
    }
}
