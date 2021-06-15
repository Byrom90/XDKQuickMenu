using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameDiscEngineV2.Interop;

namespace XDKQuickPowerMenu
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }
    }

    public class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private EmulationServices emulationServices;
        private IEmulatorConnections emulatorConnections;
        private IEmulatorConnection emulatorConnection;

        public MyCustomApplicationContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.XQPicon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Power On", PowerOn),
                    new MenuItem("Power Off", PowerOff),
                    //new MenuItem("Reboot", Reboot),
                    new MenuItem("Open Tray", OpenTray),
                    new MenuItem("Close Tray", CloseTray),
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            emulationServices = new EmulationServicesClass();
        }

        bool IsConsoleAvailable()
        {
            emulatorConnections = emulationServices.GetEmulatorConnections();
            if (emulatorConnections.Count != 0)
            {
                emulatorConnection = emulatorConnections.Item(0);
                return true;
            }
            else
            {
                MessageBox.Show("Failed to detect XDK console.\nPlease remove & reinsert usb cable and try again.\nNote: Commands are performed via the Sidecar's usb slot");
                return false;
            }
        }

        void PowerOn(object sender, EventArgs e)
        {
            if (IsConsoleAvailable())
                emulatorConnection.SetPower((EmulatorPowerSetting)0);
        }

        void PowerOff(object sender, EventArgs e)
        {
            if (IsConsoleAvailable()) 
                emulatorConnection.SetPower((EmulatorPowerSetting)1);
        }

        void Reboot(object sender, EventArgs e)
        {

        }

        void OpenTray(object sender, EventArgs e)
        {
            if (IsConsoleAvailable()) 
                emulatorConnection.PressTrayEject();
        }
        void CloseTray(object sender, EventArgs e)
        {
            if (IsConsoleAvailable()) 
                emulatorConnection.PressTrayEject();
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}