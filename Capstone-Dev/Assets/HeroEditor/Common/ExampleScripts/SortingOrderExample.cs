using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

namespace Assets.HeroEditor.Common.ExampleScripts
{
	/// <summary>
	/// Handle character overlapping example.
	/// </summary>
	public class SortingOrderExample
	{
		/// <summary>
		/// Order characters by sorting order.
		/// </summary>
		public static void OrderByIndex(Character front, Character back)
		{
			front.LayerManager.SetOrderBySortingOrder();
			back.LayerManager.SetOrderBySortingOrder();

			front.LayerManager.SetSortingGroupOrder(200);
			back.LayerManager.SetSortingGroupOrder(100);
		}

		/// <summary>
		/// Order characters by Z coordinate.
		/// </summary>
		public static void OrderByZ(Character front, Character back)
		{
			front.LayerManager.SetOrderByZCoordinate();
			back.LayerManager.SetOrderByZCoordinate();

			front.transform.localPosition = new Vector3(0, 0, -10);
			back.transform.localPosition = new Vector3(0, 0, +10);
		}
	}
}