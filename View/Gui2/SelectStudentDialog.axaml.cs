// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.View.Gui2;


using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

using ProjectMan.Core;


public partial class SelectStudentDialog: Window {
    public SelectStudentDialog()
    {
        InitializeComponent();

        var opClose = this.GetControl<MenuItem>( "OpClose" );
        var btSelect = this.GetControl<Button>( "BtSelect" );
        var btClose = this.GetControl<Button>( "BtClose" );
        var lbItems = this.GetControl<ListBox>( "LbItems" );

        opClose.Click += (_, _) => this.Quit();
        btSelect.Click += (_, _) => this.SelectItem();
        btClose.Click += (_, _) => { this.Student = null; this.Quit(); };
        lbItems.SelectionChanged += (_, _) => this.RegisterSelection();
    }

    public Task ShowDialog(Window main, Projects? projs)
    {
        Debug.Assert( projs is not null, "projs cannot be null" );

        var lbItems = this.GetControl<ListBox>( "LbItems" );

        this._projects = projs;
        lbItems.ItemsSource = projs.Students.All().Cast<IItem>();
        this.Student = null;

        return base.ShowDialog( main );
    }

    private void Quit()
    {
        this.Close();
    }

    private Student? GetStudentSelected()
    {
        Debug.Assert( this._projects is not null, "SelectItem: projects is null" );

        var lbItems = this.GetControl<ListBox>( "LbItems" );
        var lblSelected = this.GetControl<TextBlock>( "LblSelected" );
        int index = lbItems.SelectedIndex;

        this.Student = null;
        lblSelected.Text = "none";

        if ( index >= 0
          && index <= this._projects.Students.Count )
        {
            this.Student = this._projects.Students[ index ];
            lblSelected.Text = this.Student.ToString();
        }

        return this.Student;
    }

    private void RegisterSelection()
    {
        this.GetStudentSelected();
    }

    private void SelectItem()
    {
        if ( this.GetStudentSelected() is not null )
        {
            this.Quit();
        }
    }

    public Student? Student { get; private set; }

    private Projects? _projects;
}
