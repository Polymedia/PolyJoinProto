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
        private static volatile Boolean _runDiffThread = true;
        private static Thread _diffThread = null;
        static Bitmap screenShot = null;
        private static DiffDetector _diffDetector;
        private static float compr = 1;
        private static int timeout = 330;

        private static bool IsPresenter = false;

        [STAThread]
        static void Main(string[] args)
        {
            compr = float.Parse(ConfigurationSettings.AppSettings["compr"]);
            timeout = int.Parse(ConfigurationSettings.AppSettings["timeout"]);
            
            ConnectionManager connectionManager = new ConnectionManager();

            ClientWebSocketConnection clientWebSocketConnection = new ClientWebSocketConnection(connectionManager.Connect());

            clientWebSocketConnection.ConnectionStateChanged += (sender, eventArgs) =>
                {
                    if (eventArgs.Value)
                    {
                        clientWebSocketConnection.QueryState();
                    }
                    else
                    {
                        _runDiffThread = false;
                    }
                };

            #region

            clientWebSocketConnection.StateCommandReceived += (sender, eventArgs) =>
                {
                    Console.WriteLine(eventArgs.Value.ConferenceId + " " + eventArgs.Value.IsPresenter);
                    IsPresenter = eventArgs.Value.IsPresenter;
                    if (eventArgs.Value.IsPresenter)
                    {
                        _runDiffThread = false;
                        _runDiffThread = new bool();
                        _runDiffThread = true;

                        _diffThread = new Thread(() =>
                        {
                            _diffDetector = new DiffDetector();
                            screenShot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                            while (_runDiffThread)
                            {
                                
                                screenShot = new Bitmap(screenShot, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                                Graphics screenShotGraphics = Graphics.FromImage(screenShot);
                                screenShotGraphics.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));

                                if (compr!=1)
                                    screenShot = new Bitmap(screenShot, (int)(Screen.PrimaryScreen.Bounds.Width / compr), (int)(Screen.PrimaryScreen.Bounds.Height / compr));

                                screenShotGraphics = Graphics.FromImage(screenShot);
                                DiffContainer diffContainer = _diffDetector.GetDiffs(screenShot);

                                diffContainer.Data = DiffContainer.Split(diffContainer.Data, 40000);

                                foreach (var s in diffContainer.Data)
                                    clientWebSocketConnection.SendDiff(new DiffItem(s));

                                Thread.Sleep(timeout);
                            }

                        });
                        _diffThread.Start();
                    }
                };

            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mf = new MainForm();

            clientWebSocketConnection.DiffCommandReceived += (sender, eventArgs) =>
                {
                    if(!IsPresenter)
                        mf.DrawDiff(eventArgs.Value.DiffItem);
                };

            Application.Run(mf);

            connectionManager.Disconnect();
        }
    }
}
