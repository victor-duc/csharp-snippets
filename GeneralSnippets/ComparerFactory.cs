#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSnippets
{
	/// <summary>
	/// Represents a factory provides features to create <see cref="IComparer{T}"/> and <see cref="IEqualityComparer{T}"/> from lambda.
	/// </summary>
	public static class ComparerFactory
	{
		private class Comparer<T> : IComparer<T>
			where T : class
		{
			private readonly Func<T?, T?, int> compare;

			public Comparer([DisallowNull] Func<T?, T?, int> compare)
			{
				this.compare = compare;
			}

			public int Compare(T? x, T? y) => compare(x, y);
		}

		private class EqualityComparer<T> : IEqualityComparer<T>
			where T : class
		{
			private readonly Func<T?, T?, bool> equals;
			private readonly Func<T, int> getHashCode;

			public EqualityComparer(
				[DisallowNull] Func<T?, T?, bool> equals,
				[DisallowNull] Func<T, int> getHashCode)
			{
				this.equals = equals;
				this.getHashCode = getHashCode;
			}

			public bool Equals(T? x, T? y) => equals(x, y);

			public int GetHashCode([DisallowNull] T obj) => getHashCode(obj);
		}

		public static IComparer<T> CreateComparer<T>(
			[DisallowNull] Func<T?, T?, int> compare)
			where T : class
		{
			return new Comparer<T>(compare);
		}

		public static IEqualityComparer<T> CreateEqualityComparer<T>(
			[DisallowNull] Func<T?, T?, bool> equals,
			[DisallowNull] Func<T, int> getHashCode)
			where T : class
		{
			return new EqualityComparer<T>(equals, getHashCode);
		}
	}
}