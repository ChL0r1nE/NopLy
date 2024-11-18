using UnityEngine;

namespace UI
{
	public class Button : MonoBehaviour
	{
		public void ButtonDown() => _mercenariesist.CaravanButtonDown(_id);

		private MercenariesList _mercenariesist;
		private int _id;

		public void SetMercenariesList(MercenariesList mercenariesList, int id)
		{
			_mercenariesist = mercenariesList;
			_id = id;
		}
	}
}
