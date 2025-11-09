// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.View.Gui1;


using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

using ProjectMan.Core;


public partial class RegisterDialog: Window {
    private enum OpMode { Projects, Students };

    public RegisterDialog()
    {
        InitializeComponent();

        var opClose = this.GetControl<MenuItem>( "OpClose" );
        var btNew = this.GetControl<Button>( "BtNew" );
        var btDelete = this.GetControl<Button>( "BtDelete" );

        opClose.Click += (_, _) => this.Quit();
        btNew.Click += async (_, _) => await this.NewItem();
        btDelete.Click += (_, _) => this.DeleteItem();
    }

    private void Quit()
    {
        this.Close();
    }

    public Task ShowDialogForStudents(Window main, Projects? projs)
    {
        Debug.Assert( projs is not null, "projs cannot be null" );

        var lbItems = this.GetControl<ListBox>( "LbItems" );

        this._projects = projs;
        this.Title = "Students";
        this.Mode = OpMode.Students;
        lbItems.ItemsSource = projs.Students.All().Cast<IItem>();

        return base.ShowDialog( main );
    }

    public Task ShowDialogForProjects(Window main, Projects? projs)
    {
        Debug.Assert( projs is not null, "projs cannot be null" );

        var lbItems = this.GetControl<ListBox>( "LbItems" );

        this._projects = projs;
        this.Mode = OpMode.Projects;
        this.Title = "Projects";
        lbItems.ItemsSource = projs.Projs.All().Cast<IItem>();

        return base.ShowDialog( main );
    }

    private OpMode Mode { get; set; }

    private void DeleteItem()
    {
        var lbItems = this.GetControl<ListBox>( "LbItems" );
        int index = lbItems.SelectedIndex;

        Debug.Assert( this._projects is not null, "project list is null " );

        if ( index >= 0
          && index < lbItems.Items.Count)
        {
            if ( this.Mode == OpMode.Projects ) {
                this._projects.Projs.RemoveAt( index );
                lbItems.ItemsSource = this._projects.Projs.All().Cast<IItem>();
            }
            else
            if ( this.Mode == OpMode.Students ) {
                var listStudents = this._projects.Students;
                var student = listStudents[ index ];

                this._projects.RemoveStudent( student );
                lbItems.ItemsSource = this._projects.Students.All().Cast<IItem>();
            } else {
                throw new ArgumentException( "delete: trying to delete unexpected" );
            }

            lbItems.IsVisible = ( lbItems.Items.Count > 0 );
        }
    }

    private async Task NewItem()
    {
        var lbItems = this.GetControl<ListBox>( "LbItems" );

        Debug.Assert( this._projects is not null, "NewItem: projects is null" );

        if ( this.Mode == OpMode.Students ) {
            var dlgStudent = new NewStudentDialog();
            await dlgStudent.ShowDialog( this );

            if ( dlgStudent.Student is not null ) {
                this._projects.Students.Add( dlgStudent.Student );
                lbItems.ItemsSource = this._projects.Students.All().Cast<IItem>();
                lbItems.IsVisible = true;
            }
        }
        else
        if ( this.Mode == OpMode.Projects ) {
            var dlgProject = new NewProjectDialog();
            await dlgProject.ShowDialog( this, this._projects.Students );

            if ( dlgProject.Project is not null ) {
                this._projects.Projs.Add( dlgProject.Project );
                lbItems.ItemsSource = this._projects.Projs.All().Cast<IItem>();
                lbItems.IsVisible = true;
            }
        } else {
            throw new ArgumentException( "unexpected opmode: " + this.Mode  );
        }
    }

    private Projects? _projects;
}
