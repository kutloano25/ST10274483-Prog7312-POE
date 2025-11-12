// File: Program.cs
using System;
using System.Windows.Forms;

namespace MunicipalApp
{
    /// <summary>
    /// Entry point for the Municipal Services Application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] // required for Windows Forms to handle UI threads
        private static void Main()
        {
            // Enable visual styles (modern look for controls)
            Application.EnableVisualStyles();

            // Use GDI+ text rendering for better font clarity
            Application.SetCompatibleTextRenderingDefault(false);

            // Launch the main menu form (implemented in MainForm.cs)
            Application.Run(new MainForm());
        }
    }
}
