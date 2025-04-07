using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000056 RID: 86
	public static class Extensions
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x000169E0 File Offset: 0x00014BE0
		public static string ToHexadecimalString(this uint number)
		{
			return string.Format("{0:X}", number);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000169F4 File Offset: 0x00014BF4
		public static string Description(this Enum value)
		{
			object[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (customAttributes.Length != 0)
			{
				return ((DescriptionAttribute)customAttributes[0]).Description;
			}
			return value.ToString();
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00016A3B File Offset: 0x00014C3B
		public static float NextFloat(this Random random)
		{
			return (float)random.NextDouble();
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00016A44 File Offset: 0x00014C44
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			TKey tkey;
			return source.MaxBy(selector, Comparer<TKey>.Default, out tkey);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00016A5F File Offset: 0x00014C5F
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TKey maxKey)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default, out maxKey);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00016A70 File Offset: 0x00014C70
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TKey maxKey)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				maxKey = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, maxKey) > 0)
					{
						tsource = tsource2;
						maxKey = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00016B28 File Offset: 0x00014D28
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00016B38 File Offset: 0x00014D38
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence was empty");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) < 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00016BE4 File Offset: 0x00014DE4
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.DistinctBy(keySelector, null);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00016BEE File Offset: 0x00014DEE
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			return Extensions.DistinctByImpl<TSource, TKey>(source, keySelector, comparer);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00016C14 File Offset: 0x00014E14
		private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			return from g in source.GroupBy(keySelector, comparer)
			select g.First<TSource>();
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00016C42 File Offset: 0x00014E42
		public static string Add(this string str, string appendant, bool newLine = true)
		{
			if (str == null)
			{
				str = "";
			}
			str += appendant;
			if (newLine)
			{
				str += "\n";
			}
			return str;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00016C68 File Offset: 0x00014E68
		public static IEnumerable<string> Split(this string str, int maxChunkSize)
		{
			for (int i = 0; i < str.Length; i += maxChunkSize)
			{
				yield return str.Substring(i, MathF.Min(maxChunkSize, str.Length - i));
			}
			yield break;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00016C7F File Offset: 0x00014E7F
		public static BattleSideEnum GetOppositeSide(this BattleSideEnum side)
		{
			if (side == BattleSideEnum.Attacker)
			{
				return BattleSideEnum.Defender;
			}
			if (side != BattleSideEnum.Defender)
			{
				return side;
			}
			return BattleSideEnum.Attacker;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00016C90 File Offset: 0x00014E90
		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int splitItemCount)
		{
			if (splitItemCount <= 0)
			{
				throw new ArgumentException();
			}
			int i = 0;
			return source.GroupBy(delegate(T x)
			{
				int i = i;
				i++;
				return i % splitItemCount;
			});
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00016CD4 File Offset: 0x00014ED4
		public static bool IsEmpty<T>(this IEnumerable<T> source)
		{
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				return collection.Count == 0;
			}
			ICollection collection2 = source as ICollection;
			if (collection2 != null)
			{
				return collection2.Count == 0;
			}
			return !source.Any<T>();
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00016D14 File Offset: 0x00014F14
		public static void Shuffle<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = MBRandom.RandomInt(i + 1);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00016D5C File Offset: 0x00014F5C
		public static T GetRandomElement<T>(this IReadOnlyList<T> e)
		{
			if (e.Count == 0)
			{
				return default(T);
			}
			return e[MBRandom.RandomInt(e.Count)];
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00016D8C File Offset: 0x00014F8C
		public static T GetRandomElement<T>(this MBReadOnlyList<T> e)
		{
			if (e.Count == 0)
			{
				return default(T);
			}
			return e[MBRandom.RandomInt(e.Count)];
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00016DBC File Offset: 0x00014FBC
		public static T GetRandomElement<T>(this MBList<T> e)
		{
			if (e.Count == 0)
			{
				return default(T);
			}
			return e[MBRandom.RandomInt(e.Count)];
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00016DEC File Offset: 0x00014FEC
		public static T GetRandomElement<T>(this T[] e)
		{
			if (e.Length == 0)
			{
				return default(T);
			}
			return e[MBRandom.RandomInt(e.Length)];
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00016E18 File Offset: 0x00015018
		public static T GetRandomElementInefficiently<T>(this IEnumerable<T> e)
		{
			if (e.IsEmpty<T>())
			{
				return default(T);
			}
			return e.ElementAt(MBRandom.RandomInt(e.Count<T>()));
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00016E48 File Offset: 0x00015048
		public static T GetRandomElementWithPredicate<T>(this T[] e, Func<T, bool> predicate)
		{
			if (e.Length == 0)
			{
				return default(T);
			}
			int num = 0;
			for (int i = 0; i < e.Length; i++)
			{
				if (predicate(e[i]))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return default(T);
			}
			int num2 = MBRandom.RandomInt(num);
			for (int j = 0; j < e.Length; j++)
			{
				if (predicate(e[j]))
				{
					num2--;
					if (num2 < 0)
					{
						return e[j];
					}
				}
			}
			Debug.FailedAssert("false", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Extensions.cs", "GetRandomElementWithPredicate", 442);
			return default(T);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00016EF0 File Offset: 0x000150F0
		public static T GetRandomElementWithPredicate<T>(this MBReadOnlyList<T> e, Func<T, bool> predicate)
		{
			if (e.Count == 0)
			{
				return default(T);
			}
			int num = 0;
			for (int i = 0; i < e.Count; i++)
			{
				if (predicate(e[i]))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return default(T);
			}
			int num2 = MBRandom.RandomInt(num);
			for (int j = 0; j < e.Count; j++)
			{
				if (predicate(e[j]))
				{
					num2--;
					if (num2 < 0)
					{
						return e[j];
					}
				}
			}
			Debug.FailedAssert("false", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Extensions.cs", "GetRandomElementWithPredicate", 485);
			return default(T);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00016FA1 File Offset: 0x000151A1
		public static T GetRandomElementWithPredicate<T>(this MBList<T> e, Func<T, bool> predicate)
		{
			return e.GetRandomElementWithPredicate(predicate);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00016FAC File Offset: 0x000151AC
		public static T GetRandomElementWithPredicate<T>(this IReadOnlyList<T> e, Func<T, bool> predicate)
		{
			if (e.Count == 0)
			{
				return default(T);
			}
			int num = 0;
			for (int i = 0; i < e.Count; i++)
			{
				if (predicate(e[i]))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return default(T);
			}
			int num2 = MBRandom.RandomInt(num);
			for (int j = 0; j < e.Count; j++)
			{
				if (predicate(e[j]))
				{
					num2--;
					if (num2 < 0)
					{
						return e[j];
					}
				}
			}
			Debug.FailedAssert("false", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Extensions.cs", "GetRandomElementWithPredicate", 533);
			return default(T);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00017060 File Offset: 0x00015260
		public static List<Tuple<T1, T2>> CombineWith<T1, T2>(this IEnumerable<T1> list1, IEnumerable<T2> list2)
		{
			List<Tuple<T1, T2>> list3 = new List<Tuple<T1, T2>>();
			foreach (T1 item in list1)
			{
				foreach (T2 item2 in list2)
				{
					list3.Add(new Tuple<T1, T2>(item, item2));
				}
			}
			return list3;
		}
	}
}
