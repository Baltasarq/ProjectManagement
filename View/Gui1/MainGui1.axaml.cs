// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.View.Gui1;


using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using System.Threading.Tasks;

using ProjectMan.Core;


public partial class MainGui1 : Window
{
    public MainGui1()
    {
        InitializeComponent();

        var btGui1 = this.GetControl<Button>( "BtProjects" );
        var btGui2 = this.GetControl<Button>( "BtStudents" );
        var btOpGui1 = this.GetControl<MenuItem>( "OpStudents" );
        var btOpGui2 = this.GetControl<MenuItem>( "OpProjects" );
        var btOpQuit = this.GetControl<MenuItem>( "OpClose" );

        btGui1.Click += (_, _) => this.LaunchProjectsDialog();
        btGui2.Click += (_, _) => this.LaunchStudentsDialog();

        btOpGui1.Click += (_,_) => this.LaunchProjectsDialog();
        btOpGui2.Click += (_,_) => this.LaunchStudentsDialog();

        btOpQuit.Click += (_, _) => this.OnQuit();
    }

    public Task ShowDialog(Window owner, Projects projects)
    {
        this.projects = projects;
        return base.ShowDialog( owner );
    }

    private void OnQuit()
    {
        this.Close();
    }

    private void LaunchProjectsDialog()
    {
        var dlgPrjs = new RegisterDialog();
        dlgPrjs.ShowDialogForProjects( this, projects );
    }

    private void LaunchStudentsDialog()
    {
        var dlgStudents = new RegisterDialog();
        dlgStudents.ShowDialogForStudents( this, projects );
    }

    public Projects? projects;
}
