// (c) VentaCoches 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.View;


using Avalonia.Controls;

using ProjectMan.Core;


public partial class MainWindow : Window {
    public MainWindow()
    {
        InitializeComponent();

        var btGui1 = this.GetControl<Button>( "BtGui1" );
        var btGui2 = this.GetControl<Button>( "BtGui2" );
        var btOpGui1 = this.GetControl<MenuItem>( "OpGui1" );
        var btOpGui2 = this.GetControl<MenuItem>( "OpGui2" );
        var btOpQuit = this.GetControl<MenuItem>( "OpQuit" );

        btGui1.Click += (_, _) => this.LaunchGui1();
        btGui2.Click += (_, _) => this.LaunchGui2();

        btOpGui1.Click += (_,_) => this.LaunchGui1();
        btOpGui2.Click += (_,_) => this.LaunchGui2();

        btOpQuit.Click += (_, _) => this.OnQuit();

        this._projects = new Projects();
    }

    private void LaunchGui1()
    {
        var mainGui1 = new View.Gui1.MainGui1();
        mainGui1.ShowDialog( this, this._projects );
    }

    private void LaunchGui2()
    {
        var mainGui2 = new View.Gui2.MainGui2();
        mainGui2.ShowDialog( this, this._projects );
    }

    private void OnQuit()
    {
        this.Close();
    }

    private Projects _projects;
}