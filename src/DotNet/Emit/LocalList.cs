﻿using System.Collections.Generic;
using System.Diagnostics;
using dot10.Utils;

namespace dot10.DotNet.Emit {
	/// <summary>
	/// Stores a collection of <see cref="Local"/>
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public sealed class LocalList : IListListener<Local>, IList<Local> {
		LazyList<Local> locals;

		/// <summary>
		/// Gets the number of locals
		/// </summary>
		public int Count {
			get { return locals.Count; }
		}

		/// <summary>
		/// Gets the list of locals
		/// </summary>
		public IList<Local> Locals {
			get { return locals; }
		}

		/// <summary>
		/// Gets the N'th local
		/// </summary>
		/// <param name="index">The local index</param>
		public Local this[int index] {
			get { return locals[index]; }
			set { locals[index] = value; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public LocalList() {
			this.locals = new LazyList<Local>(this);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="locals">All locals that will be owned by this instance</param>
		public LocalList(IEnumerable<Local> locals) {
			this.locals = new LazyList<Local>(this);
			foreach (var local in locals)
				this.locals.Add(local);
		}

		/// <summary>
		/// Adds a new local and then returns it
		/// </summary>
		/// <param name="local">The local that should be added to the list</param>
		/// <returns>The input is always returned</returns>
		public Local Add(Local local) {
			locals.Add(local);
			return local;
		}

		/// <inheritdoc/>
		void IListListener<Local>.OnLazyAdd(int index, ref Local value) {
		}

		/// <inheritdoc/>
		void IListListener<Local>.OnAdd(int index, Local value) {
		}

		/// <inheritdoc/>
		void IListListener<Local>.OnRemove(int index, Local value) {
			value.Index = -1;
		}

		/// <inheritdoc/>
		void IListListener<Local>.OnResize(int index) {
			for (int i = index; i < locals.Count; i++)
				locals[i].Index = i;
		}

		/// <inheritdoc/>
		void IListListener<Local>.OnClear() {
			foreach (var local in locals)
				local.Index = -1;
		}

		/// <inheritdoc/>
		public int IndexOf(Local item) {
			return locals.IndexOf(item);
		}

		/// <inheritdoc/>
		public void Insert(int index, Local item) {
			locals.Insert(index, item);
		}

		/// <inheritdoc/>
		public void RemoveAt(int index) {
			locals.RemoveAt(index);
		}

		void ICollection<Local>.Add(Local item) {
			locals.Add(item);
		}

		/// <inheritdoc/>
		public void Clear() {
			locals.Clear();
		}

		/// <inheritdoc/>
		public bool Contains(Local item) {
			return locals.Contains(item);
		}

		/// <inheritdoc/>
		public void CopyTo(Local[] array, int arrayIndex) {
			locals.CopyTo(array, arrayIndex);
		}

		int ICollection<Local>.Count {
			get { return locals.Count; }
		}

		bool ICollection<Local>.IsReadOnly {
			get { return false; }
		}

		/// <inheritdoc/>
		public bool Remove(Local item) {
			return locals.Remove(item);
		}

		/// <inheritdoc/>
		public IEnumerator<Local> GetEnumerator() {
			return locals.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return ((IEnumerable<Local>)this).GetEnumerator();
		}
	}

	/// <summary>
	/// A method local
	/// </summary>
	public sealed class Local : IVariable {
		TypeSig typeSig;
		int index;
		string name;

		/// <summary>
		/// Gets/sets the type of the local
		/// </summary>
		public TypeSig Type {
			get { return typeSig; }
			set { typeSig = value; }
		}

		/// <summary>
		/// Local index
		/// </summary>
		public int Index {
			get { return index; }
			internal set { index = value; }
		}

		/// <summary>
		/// Gets/sets the name
		/// </summary>
		public string Name {
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="typeSig">The type</param>
		public Local(TypeSig typeSig) {
			this.typeSig = typeSig;
		}

		/// <inheritdoc/>
		public override string ToString() {
			if (string.IsNullOrEmpty(Name))
				return string.Format("V_{0}", Index);
			return Name;
		}
	}
}
