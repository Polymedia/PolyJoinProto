using System;
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

        private static bool IsPresenter = false;

        [STAThread]
        static void Main(string[] args)
        {
            Connection connection = ConnectionManager.Connect();

            ClientWebSocketConnection clientWebSocketConnection = new ClientWebSocketConnection(ConnectionManager.Connect());

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
                            while (true)
                            {
                                if (_runDiffThread)
                                {
                                    screenShot = new Bitmap(screenShot, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                                    Graphics screenShotGraphics = Graphics.FromImage(screenShot);
                                    screenShotGraphics.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));


                                    //   if (_compr!=1)
                                    //screenShot = new Bitmap(screenShot, (int)(Screen.PrimaryScreen.Bounds.Width / _compr), (int)(Screen.PrimaryScreen.Bounds.Height / _compr));

                                    screenShotGraphics = Graphics.FromImage(screenShot);
                                    DiffContainer diffContainer = _diffDetector.GetDiffs(screenShot);

                                    diffContainer.Data = DiffContainer.Split(diffContainer.Data, 40000);

                                    clientWebSocketConnection.SendDiff(diffContainer);
                                }

                                Thread.Sleep(330);
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
                        mf.DrawDiff(eventArgs.Value.Container);
                };

            //ждем подключения
            Thread.Sleep(5000);
            clientWebSocketConnection.QueryState();

            
            Application.Run(mf);

            connection.Stop();
        }
    }
}
