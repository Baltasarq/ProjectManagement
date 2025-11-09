// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.Core;


public class Projects {
    public Projects()
    {
        this._projects = new Register<Project>();
        this._students = new Register<Student>();

        Load( this );
    }

    public Register<Project> Projs => this._projects;
    public Register<Student> Students => this._students;

    public void RemoveStudent(Student student)
    {
        this.Students.Remove( student );

        foreach(Project p in this.Projs.All()) {
            p.Remove( student );
        }
    }

    public override string ToString()
    {
        return $"# Students\n\n{this._students}"
                + "\n\n"
                + $"{this._projects}";

    }

    private Register<Project> _projects;
    private Register<Student> _students;

    private static void Load(Projects prjs)
    {
        var students = new Student[] {
            new Student {
                DNI = "00000001D",
                Name = "Pan",
                Surname = "Con Tomate"
            },
            new Student {
                DNI = "00000002D",
                Name = "Pan",
                Surname = "Con Queso"
            },
            new Student {
                DNI = "00000003D",
                Name = "Marty",
                Surname = "Del Futuro"
            },
            new Student {
                DNI = "00000004D",
                Name = "Camisa",
                Surname = "De Rayas"
            },
            new Student {
                DNI = "00000005D",
                Name = "Van",
                Surname = "Sueltitos"
            },
        };

        var projs = new Project[] {
            new Project{ Name = "Bank Manager" },
            new Project{ Name = "Novel Writer" },
        };

        projs[ 0 ].Add( students[ 0 ] );
        projs[ 0 ].Add( students[ 1 ] );
        projs[ 0 ].Add( students[ 2 ] );

        projs[ 1 ].Add( students[ 3 ] );
        projs[ 1 ].Add( students[ 4 ] );

        foreach(Student student in students) {
            prjs.Students.Add( student );
        }

        foreach(Project project in projs) {
            prjs.Projs.Add( project );
        }
    }
}
