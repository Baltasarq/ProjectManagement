// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.View.Gui2;


using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using ProjectMan.Core;


public partial class NewProjectDialog : Window
{
    public NewProjectDialog()
    {
        InitializeComponent();

        var btInsert = this.GetControl<Button>( "BtInsert" );
        var btClose = this.GetControl<Button>( "BtClose" );
        var btSave = this.GetControl<Button>( "BtSave" );

        btInsert.Click += (_, _) => this.InsertStudent();
        btClose.Click += (_, _) => this.Quit();
        btSave.Click += (_, _) => this.Save();
    }

    public Task ShowDialog(Window owner, Register<Student> students)
    {
        var toret = base.ShowDialog( owner );
        var cbStudent = this.GetControl<ComboBox>( "CbStudent" );

        this.Project = null;
        this._students = students;
        this._prj_students = new HashSet<Student>();
        cbStudent.ItemsSource = students.All();

        if ( cbStudent.Items.Count > 0 ) {
            cbStudent.SelectedIndex = 0;
        }

        return toret;
    }

    private void InsertStudent()
    {
        var lbStudents = this.GetControl<ListBox>( "LbStudents" );
        var cbStudent = this.GetControl<ComboBox>( "CbStudent" );

        this.Show( "" );

        if ( cbStudent.SelectedIndex >= 0 ) {
            Debug.Assert( this._students is not null,
                            "InsertStudent: students is null" );
            Debug.Assert( this._prj_students is not null,
                            "InsertStudent: students is null" );

            var student = this._students.All()[ cbStudent.SelectedIndex ];

            this._prj_students.Add( student );
            lbStudents.ItemsSource = this._prj_students.ToList();
        }
    }

    private void Save()
    {
        var edName = this.GetControl<TextBox>( "EdName" );
        var lbStudents = this.GetControl<ListBox>( "LbStudents" );

        this.Project = new Project {
            Name = edName.Text ?? ""
        };

        foreach(Student? student in lbStudents.Items) {
            if ( student is null ) {
                continue;
            }

            this.Project.Add( student );
        }

        this.Quit();
    }

    private void Quit()
    {
        this.Close();
    }

    private void Show(string msg)
    {
        var tbMessage = this.GetControl<TextBlock>( "TbMessage" );
        tbMessage.Text = msg;
    }

    public Project? Project { get; private set; }
    private Register<Student>? _students;
    private ISet<Student>? _prj_students;
}
