using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Drawing;
using DifferenceLib;
using System.Windows.Forms;

namespace Polymedia.PolyJoin.Client
{
    class Program
    {
        private static ConnectForm _connectForm;
        private static MainForm _mainForm;
	    private static ConnectionManager _connectionManager;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _connectionManager = new ConnectionManager();
            _connectionManager.ServerIp = "localhost";
            _connectForm = new ConnectForm();
            _connectForm.ServerIp = _connectionManager.ServerIp;
            _mainForm = new MainForm
                {
                    ScreenshotScale = float.Parse(ConfigurationSettings.AppSettings["scale"]),
                    ScreenshotTimeout = int.Parse(ConfigurationSettings.AppSettings["timeout"])
                };

            _connectForm.VisibleChanged += (sender, eventArgs) =>
                {
                    if (!_connectForm.Visible)
                    {
                        _connectionManager.ServerIp = _connectForm.ServerIp;
                        
                        var clientWebSocketConnection = new ClientWebSocketConnection(_connectionManager.GetConnection());
                        _mainForm.ClientWebSocketConnection = clientWebSocketConnection;
                        _mainForm.ConferenceId = _connectForm.ConferenceId;

                        _connectionManager.Connect();
                        
                        _mainForm.Show();
                    }
                    else
                    {
                        _connectionManager.Disconnect();
                    }
                };

            _mainForm.VisibleChanged += (sender, eventArgs) =>
            {
                if (!_mainForm.Visible)
                    _connectForm.Show();
            };

            Application.Run(_connectForm);
        }
    }
}
