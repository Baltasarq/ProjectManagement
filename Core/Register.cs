// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.Core;


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


public class Register<T> where T: IItem {
    public Register()
    {
        this._v = new List<T>();
    }

    /// <summary>
    /// Adds a new element of class T.
    /// </summary>
    /// <param name="x">An object of class T.</param>
    public void Add(T x)
    {
        if ( !this._v.Contains( x ) ) {
            this._v.Add( x );
        }
    }

    /// <summary>Removes the given element.</summary>
    /// <param name="x">A given element.</param>
    public void Remove(T x)
    {
        this._v.Remove( x );
    }

    /// <summary>Removes the given element at a position x.</summary>
    /// <param name="x">A given index.</param>
    public void RemoveAt(int x)
    {
        this._v.RemoveAt( x );
    }

    public T this[int index]
    {
        get {
            return this._v[ index ];
        }
        set {
            this._v[ index ] = value;
        }
    }

    /// <summary>
    /// Get all the elements.
    /// </summary>
    /// <returns>A read-only collection with all the stored elements.</returns>
    public ReadOnlyCollection<T> All() => this._v.AsReadOnly();

    /// <summary>Get the number of elements.</summary>
    public int Count => this._v.Count;

    public T? LookUp(Predicate<T> pred)
    {
        int pos = ( (List<T>) this._v).FindIndex( pred );
        T? toret = default(T);

        if ( pos >= 0 ) {
            toret = this._v[ pos ];
        }

        return toret;
    }

    public override string ToString()
    {
        return string.Join( "\n", this._v );
    }

    private IList<T> _v;
}
