using System.Collections;
using System.Collections.Generic;

namespace CollectionHelper
{
	public static class IEnumeratorHelper
	{
		/// <summary>
		/// Consume the entire enumerator.
		/// <br></br>
		/// Also consumes yielded enumerators.
		/// </summary>
		public static void All(this IEnumerator enumerator)
		{
			Stack<IEnumerator> stack = new();
			stack.Push(enumerator);

			while (stack.Count > 0)
			{
				IEnumerator current = stack.Pop();

				while (current.MoveNext())
					if (current.Current is IEnumerator nested)
					{
						stack.Push(current);
						current = nested;
					}
			}
		}
	}
}
