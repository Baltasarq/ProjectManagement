// (c) ProjectMan 2025 Baltasar MIT License <jbgarcia@uvigo.es>


namespace ProjectMan.View;


using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using ProjectMan.Core;


public partial class NewStudentDialog : Window
{
    public NewStudentDialog()
    {
        InitializeComponent();

        var btSave = this.GetControl<Button>( "BtSave" );
        var btClose = this.GetControl<Button>( "BtClose" );

        btSave.Click += (_, _) => this.Quit();
        btClose.Click += (_, _) => this.Close();
    }

    private void Quit()
    {
        var edDNI = this.GetControl<TextBox>( "EdDNI" );
        var edName = this.GetControl<TextBox>( "EdName" );
        var edSurname = this.GetControl<TextBox>( "EdSurname" );

        this.Student = null;

        if ( !string.IsNullOrWhiteSpace( edDNI.Text )
          && !string.IsNullOrWhiteSpace( edName.Text )
          && !string.IsNullOrWhiteSpace( edSurname.Text ) )
        {
            this.Student = new Student {
                DNI = edDNI.Text,
                Name = edName.Text,
                Surname = edSurname.Text
            };
        }

        base.Close();
    }

    public Student? Student { get; private set; }
}
