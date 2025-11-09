// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.Core;


using System.Collections.Generic;


public class Project: IItem {
    public Project()
    {
        this.Id = ++num;
        this._name = "Prj";
        this._students = new Register<Student>();
    }

    public string Name { get {
            return this._name;
        }
        set {
            if ( !string.IsNullOrWhiteSpace( value ) ) {
                this._name = value;
            }
        }
    }
    public int Id { get; private set; }

    public void Add(Student student)
    {
        this._students.Add( student );
    }

    public void Remove(Student student)
    {
        this._students.Remove( student );
    }

    public IList<Student> Students => this._students.All();

    public override int GetHashCode() => this.Id;

    public override bool Equals(object? obj)
    {
        bool toret = false;

        if ( obj is Project other ) {
            toret = this.Id == other.Id;
        }

        return toret;
    }

    public override string ToString()
    {
        return $"# {this.Name}\n\n{this._students.ToString()}";
    }

    private Register<Student> _students;
    private string _name;
    private static int num;
}
