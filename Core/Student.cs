// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.Core;


using System.Linq;


public class Student: IItem {
    public Student()
    {
        this.Id = ++num;
        this._dni = "";
    }

    /// <summary>The student's ID card.</summary>
    public required string DNI {
        get {
            return this._dni;
        }
        init {
            this._dni =
                string.Join( "", Enumerable.Repeat( "0", 9 - value.Length ))
                    + value.Trim().ToUpper();
        }
    }

    /// <summary>The student's name.</summary>
    public required string Name { get; set; }

    /// <summary>The student's surname.</summary>
    public required string Surname { get; set; }

    public int Id { get; init; }

    public override int GetHashCode()
    {
        return this.Id;
    }

    public override bool Equals(object? obj)
    {
        bool toret = false;

        if ( obj is Student other ) {
            toret = ( this.Id == other.Id );
        }

        return toret;
    }

    public override string ToString()
    {
        return $"{this.DNI}/ {this.Surname}, {this.Name}";
    }

    private string _dni;
    private static int num = 0;
}
