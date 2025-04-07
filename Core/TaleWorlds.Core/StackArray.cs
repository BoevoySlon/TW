using System;
using System.Collections.Specialized;

namespace TaleWorlds.Core
{
	// Token: 0x020000BF RID: 191
	public class StackArray
	{
		// Token: 0x0200010A RID: 266
		public struct StackArray5Float
		{
			// Token: 0x17000361 RID: 865
			public float this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					default:
						return 0f;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x04000702 RID: 1794
			private float _element0;

			// Token: 0x04000703 RID: 1795
			private float _element1;

			// Token: 0x04000704 RID: 1796
			private float _element2;

			// Token: 0x04000705 RID: 1797
			private float _element3;

			// Token: 0x04000706 RID: 1798
			private float _element4;

			// Token: 0x04000707 RID: 1799
			public const int Length = 5;
		}

		// Token: 0x0200010B RID: 267
		public struct StackArray3Int
		{
			// Token: 0x17000362 RID: 866
			public int this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					default:
						return 0;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x04000708 RID: 1800
			private int _element0;

			// Token: 0x04000709 RID: 1801
			private int _element1;

			// Token: 0x0400070A RID: 1802
			private int _element2;

			// Token: 0x0400070B RID: 1803
			public const int Length = 3;
		}

		// Token: 0x0200010C RID: 268
		public struct StackArray4Int
		{
			// Token: 0x17000363 RID: 867
			public int this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					default:
						return 0;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x0400070C RID: 1804
			private int _element0;

			// Token: 0x0400070D RID: 1805
			private int _element1;

			// Token: 0x0400070E RID: 1806
			private int _element2;

			// Token: 0x0400070F RID: 1807
			private int _element3;

			// Token: 0x04000710 RID: 1808
			public const int Length = 4;
		}

		// Token: 0x0200010D RID: 269
		public struct StackArray2Bool
		{
			// Token: 0x17000364 RID: 868
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & 1 << index) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | 1 << index) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x04000711 RID: 1809
			private byte _element;

			// Token: 0x04000712 RID: 1810
			public const int Length = 2;
		}

		// Token: 0x0200010E RID: 270
		public struct StackArray8Int
		{
			// Token: 0x17000365 RID: 869
			public int this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					case 5:
						return this._element5;
					case 6:
						return this._element6;
					case 7:
						return this._element7;
					default:
						return 0;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					case 5:
						this._element5 = value;
						return;
					case 6:
						this._element6 = value;
						return;
					case 7:
						this._element7 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x04000713 RID: 1811
			private int _element0;

			// Token: 0x04000714 RID: 1812
			private int _element1;

			// Token: 0x04000715 RID: 1813
			private int _element2;

			// Token: 0x04000716 RID: 1814
			private int _element3;

			// Token: 0x04000717 RID: 1815
			private int _element4;

			// Token: 0x04000718 RID: 1816
			private int _element5;

			// Token: 0x04000719 RID: 1817
			private int _element6;

			// Token: 0x0400071A RID: 1818
			private int _element7;

			// Token: 0x0400071B RID: 1819
			public const int Length = 8;
		}

		// Token: 0x0200010F RID: 271
		public struct StackArray10FloatFloatTuple
		{
			// Token: 0x17000366 RID: 870
			public ValueTuple<float, float> this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					case 5:
						return this._element5;
					case 6:
						return this._element6;
					case 7:
						return this._element7;
					case 8:
						return this._element8;
					case 9:
						return this._element9;
					default:
						return new ValueTuple<float, float>(0f, 0f);
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					case 5:
						this._element5 = value;
						return;
					case 6:
						this._element6 = value;
						return;
					case 7:
						this._element7 = value;
						return;
					case 8:
						this._element8 = value;
						return;
					case 9:
						this._element9 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x0400071C RID: 1820
			private ValueTuple<float, float> _element0;

			// Token: 0x0400071D RID: 1821
			private ValueTuple<float, float> _element1;

			// Token: 0x0400071E RID: 1822
			private ValueTuple<float, float> _element2;

			// Token: 0x0400071F RID: 1823
			private ValueTuple<float, float> _element3;

			// Token: 0x04000720 RID: 1824
			private ValueTuple<float, float> _element4;

			// Token: 0x04000721 RID: 1825
			private ValueTuple<float, float> _element5;

			// Token: 0x04000722 RID: 1826
			private ValueTuple<float, float> _element6;

			// Token: 0x04000723 RID: 1827
			private ValueTuple<float, float> _element7;

			// Token: 0x04000724 RID: 1828
			private ValueTuple<float, float> _element8;

			// Token: 0x04000725 RID: 1829
			private ValueTuple<float, float> _element9;

			// Token: 0x04000726 RID: 1830
			public const int Length = 10;
		}

		// Token: 0x02000110 RID: 272
		public struct StackArray3Bool
		{
			// Token: 0x17000367 RID: 871
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & 1 << index) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | 1 << index) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x04000727 RID: 1831
			private byte _element;

			// Token: 0x04000728 RID: 1832
			public const int Length = 3;
		}

		// Token: 0x02000111 RID: 273
		public struct StackArray4Bool
		{
			// Token: 0x17000368 RID: 872
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & 1 << index) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | 1 << index) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x04000729 RID: 1833
			private byte _element;

			// Token: 0x0400072A RID: 1834
			public const int Length = 4;
		}

		// Token: 0x02000112 RID: 274
		public struct StackArray5Bool
		{
			// Token: 0x17000369 RID: 873
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & 1 << index) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | 1 << index) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x0400072B RID: 1835
			private byte _element;

			// Token: 0x0400072C RID: 1836
			public const int Length = 5;
		}

		// Token: 0x02000113 RID: 275
		public struct StackArray6Bool
		{
			// Token: 0x1700036A RID: 874
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & 1 << index) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | 1 << index) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x0400072D RID: 1837
			private byte _element;

			// Token: 0x0400072E RID: 1838
			public const int Length = 6;
		}

		// Token: 0x02000114 RID: 276
		public struct StackArray7Bool
		{
			// Token: 0x1700036B RID: 875
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & 1 << index) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | 1 << index) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x0400072F RID: 1839
			private byte _element;

			// Token: 0x04000730 RID: 1840
			public const int Length = 7;
		}

		// Token: 0x02000115 RID: 277
		public struct StackArray8Bool
		{
			// Token: 0x1700036C RID: 876
			public bool this[int index]
			{
				get
				{
					return ((int)this._element & 1 << index) != 0;
				}
				set
				{
					this._element = (byte)(value ? ((int)this._element | 1 << index) : ((int)this._element & ~(1 << index)));
				}
			}

			// Token: 0x04000731 RID: 1841
			private byte _element;

			// Token: 0x04000732 RID: 1842
			public const int Length = 8;
		}

		// Token: 0x02000116 RID: 278
		public struct StackArray32Bool
		{
			// Token: 0x1700036D RID: 877
			public bool this[int index]
			{
				get
				{
					return this._element[1 << index];
				}
				set
				{
					this._element[1 << index] = value;
				}
			}

			// Token: 0x06000A89 RID: 2697 RVA: 0x000220D0 File Offset: 0x000202D0
			public StackArray32Bool(int init)
			{
				this._element = new BitVector32(init);
			}

			// Token: 0x04000733 RID: 1843
			private BitVector32 _element;

			// Token: 0x04000734 RID: 1844
			public const int Length = 32;
		}
	}
}
