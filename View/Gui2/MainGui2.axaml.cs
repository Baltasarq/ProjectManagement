// (c) VentaCoches 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.View.Gui2;


using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using ProjectMan.Core;


public partial class MainGui2 : Window
{
    public MainGui2()
    {
        InitializeComponent();

        var btNewProject = this.GetControl<Button>( "BtNewProject" );
        var btDeleteProject = this.GetControl<Button>( "BtDeleteProject" );
        var btRenameProject = this.GetControl<Button>( "BtRenameProject" );
        var btSaveProjectName = this.GetControl<Button>( "BtSaveProjectName" );
        var btAddStudentToProject = this.GetControl<Button>( "BtAddStudentToProject" );
        var btRemoveStudentFromProject = this.GetControl<Button>( "BtRemoveStudentFromProject" );
        var opDeleteStudent = this.GetControl<MenuItem>( "OpDeleteStudent" );
        var opNewStudent = this.GetControl<MenuItem>( "OpNewStudent" );
        var opDeleteProject = this.GetControl<MenuItem>( "OpDeleteProject" );
        var opRenameProject = this.GetControl<MenuItem>( "OpRenameProject" );
        var opNewProject = this.GetControl<MenuItem>( "OpNewProject" );
        var opAddStudentToProject = this.GetControl<MenuItem>( "OpAddStudentToProject" );
        var opRemoveStudentFromProject = this.GetControl<MenuItem>( "OpRemoveStudentFromProject" );
        var opClose = this.GetControl<MenuItem>( "OpClose" );
        var lbProjects = this.GetControl<ListBox>( "LbProjects" );

        EventHandler<RoutedEventArgs> newProjectAction =
                                        (_, _) => this.NewProject();
        EventHandler<RoutedEventArgs> deleteProjectAction =
                                        (_, _) => this.DeleteProject();
        EventHandler<RoutedEventArgs> newStudentAction =
                                        (_, _) => this.NewStudent();
        EventHandler<RoutedEventArgs> deleteStudentAction =
                                        (_, _) => this.DeleteStudent();
        EventHandler<RoutedEventArgs> saveProjectNameAction =
                                        (_, _) => this.SaveProjectName();
        EventHandler<RoutedEventArgs> addStudentToProjectAction =
                                        (_, _) => this.AddStudentToProject();
        EventHandler<RoutedEventArgs> removeStudentFromProjectAction =
                                        (_, _) => this.RemoveStudentFromProject();
        EventHandler<RoutedEventArgs> renameProjectAction =
                                        (_, _) => this.RenameProject();

        btNewProject.Click += newProjectAction;
        btRenameProject.Click += renameProjectAction;
        btDeleteProject.Click += deleteProjectAction;
        btSaveProjectName.Click += saveProjectNameAction;

        btAddStudentToProject.Click += addStudentToProjectAction;
        btRemoveStudentFromProject.Click += removeStudentFromProjectAction;

        opNewProject.Click += newProjectAction;
        opDeleteProject.Click += deleteProjectAction;
        opRenameProject.Click += renameProjectAction;
        opNewStudent.Click += newStudentAction;
        opDeleteStudent.Click += deleteStudentAction;
        opAddStudentToProject.Click += addStudentToProjectAction;
        opRemoveStudentFromProject.Click += removeStudentFromProjectAction;
        opClose.Click += (_, _) => this.Quit();

        lbProjects.SelectionChanged += (_, _) => SyncPanels();
    }

    public Task ShowDialog(Window owner, Projects projects)
    {
        var toret = base.ShowDialog( owner );

        this._projects = projects;
        this.UpdateMainPanel();

        return toret;
    }

    private async void NewProject()
    {
        Debug.Assert( this._projects is not null,
                      "NewProject(): projects is null" );

        var dlgProject = new NewProjectDialog();
        await dlgProject.ShowDialog( this, this._projects.Students );

        if ( dlgProject.Project is not null ) {
            this._projects.Projs.Add( dlgProject.Project );
            this.UpdateMainPanel();
        }
    }

    private void RenameProject()
    {
        Debug.Assert( this._projects is not null,
                      "UpdateMainPanel(): projects is null" );

        var lbProjects = this.GetControl<ListBox>( "LbProjects" );
        int index = lbProjects.SelectedIndex;

        if ( index >= 0
          && index < this._projects.Projs.Count )
        {
            var pnlProjectName = this.GetControl<DockPanel>( "PnlProjectName" );
            var edProjectName = this.GetControl<TextBox>( "EdProjectName" );
            var project = this._projects.Projs[ index ];

            edProjectName.Text = project.Name;
            pnlProjectName.IsVisible = true;
            this._projectToRename = project;
        }
    }

    private void SaveProjectName()
    {
        Debug.Assert( this._projects is not null,
                      "SaveProjectName(): projects is null" );
        Debug.Assert( this._projectToRename is not null,
                      "SaveProjectName(): project to rename is null" );
        var pnlProjectName = this.GetControl<DockPanel>( "PnlProjectName" );
        var edProjectName = this.GetControl<TextBox>( "EdProjectName" );
        var lbProjects = this.GetControl<ListBox>( "LbProjects" );

        pnlProjectName.IsVisible = false;
        this._projectToRename.Name = ( edProjectName.Text ?? "" ).Trim();
        this._projectToRename = null;
        this.UpdateMainPanel();
    }

    private async void NewStudent()
    {
        Debug.Assert( this._projects is not null,
                      "NewStudent(): projects is null" );

        var dlgStudent = new NewStudentDialog();
        await dlgStudent.ShowDialog( this );

        if ( dlgStudent.Student is not null ) {
            this._projects.Students.Add( dlgStudent.Student );
        }
    }

    private async void DeleteStudent()
    {
        Debug.Assert( this._projects is not null,
                      "NewStudent(): projects is null" );

        var dlgDeleteStudent = new SelectStudentDialog();
        await dlgDeleteStudent.ShowDialog( this, this._projects );

        var student = dlgDeleteStudent.Student;
        if ( student is not null ) {
            this._projects.RemoveStudent( student );
            this.UpdateMainPanel();
        }
    }

    private void DeleteProject()
    {
        Debug.Assert( this._projects is not null,
                      "DeleteProject(): projects is null" );

        var lbProjects = this.GetControl<ListBox>( "LbProjects" );
        int index = lbProjects.SelectedIndex;

        if ( index >= 0
          && index < this._projects.Projs.Count )
        {
            this._projects.Projs.RemoveAt( index );
            this.UpdateProjects( lbProjects, this._projects.Projs.All() );

            if ( lbProjects.Items.Count > 0 ) {
                this.SyncPanels();
            } else {
                var lbStudents = this.GetControl<ListBox>( "LbStudents" );
                lbStudents.IsVisible = false;
            }
        }
    }

    private void RemoveStudentFromProject()
    {
        Debug.Assert( this._projects is not null,
                      "RemoveStudentFromProject(): projects is null" );

        var lbProjects = this.GetControl<ListBox>( "LbProjects" );
        var lbStudents = this.GetControl<ListBox>( "LbStudents" );
        int projectIndex = lbProjects.SelectedIndex;

        if ( projectIndex >= 0
          && projectIndex < this._projects.Projs.Count )
        {
            var project = this._projects.Projs[ projectIndex ];
            var projectStudents = project.Students;
            int studentIndex = lbStudents.SelectedIndex;

            if ( studentIndex >= 0
              && studentIndex < projectStudents.Count )
            {
                project.Remove( projectStudents[ studentIndex ] );
                this.SyncPanels();
            }
        }
    }

    private async void AddStudentToProject()
    {
        Debug.Assert( this._projects is not null,
                            "AddStudentToProject: projects can't be null" );

        var lbProjects = this.GetControl<ListBox>( "LbProjects" );
        int prjIndex = lbProjects.SelectedIndex;

        if ( prjIndex >= 0
          && prjIndex < this._projects.Projs.Count )
        {
            var prj = this._projects.Projs[ prjIndex ];
            var dlgSelectStudentDialog = new SelectStudentDialog();
            await dlgSelectStudentDialog.ShowDialog( this, this._projects );

            var student = dlgSelectStudentDialog.Student;
            if ( student is not null ) {
                prj.Add( student );
                this.SyncPanels();
            }
        }
    }

    private void Quit()
    {
        this.Close();
    }

    private void UpdateProjects(ListBox lbProjects, IEnumerable<Project> projects)
    {
        lbProjects.ItemsSource = projects.Select ( p => p.Name );
    }

    private void UpdateMainPanel()
    {
        Debug.Assert( this._projects is not null,
                      "UpdateMainPanel(): projects is null" );

        var lbStudents = this.GetControl<ListBox>( "LbStudents" );
        var lbProjects = this.GetControl<ListBox>( "LbProjects" );
        var listProjects = this._projects.Projs.All();

        this.UpdateProjects( lbProjects, this._projects.Projs.All() );

        if ( listProjects.Count > 0 ) {
            this.SyncPanels();
            lbStudents.IsVisible = true;
        } else {
            lbStudents.IsVisible = false;
        }
    }

    private void SyncPanels()
    {
        var lbStudents = this.GetControl<ListBox>( "LbStudents" );
        var lbProjects = this.GetControl<ListBox>( "LbProjects" );

        Debug.Assert( this._projects is not null );

        if ( lbProjects.SelectedIndex >= 0
          && lbProjects.SelectedIndex < this._projects.Projs.Count )
        {
            var project = this._projects.Projs[ lbProjects.SelectedIndex ];
            var projectStudents = project.Students;

            lbStudents.ItemsSource = projectStudents;
            lbStudents.IsVisible = projectStudents.Count > 0;
        }
    }

    private Projects? _projects;
    private Project? _projectToRename;
}
