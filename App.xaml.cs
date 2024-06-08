﻿using System.Windows;
using UserManagementApp.Classes;

namespace UserManagementApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseHelper.InitializeDatabase();
        }
    }
}
