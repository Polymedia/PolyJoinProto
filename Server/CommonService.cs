using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DifferenceLib;
using Polymedia.PolyJoin.Common;

namespace Polymedia.PolyJoin.Server
{
    class CommonService
    {
        private static int ConferenceId = 123;

        private static Graphics graphics;

        private static Bitmap bitmap;

        private static object syncObj = new object();

        private static Queue queue = Queue.Synchronized(new Queue());

        public static void Init()
        {
            ConnectionManager.GetStateCommandReceived += ConnectionManagerOnGetStateCommandReceived;
            ConnectionManager.DiffCommandReceived += ConnectionManagerOnDiffCommandReceived;


            Thread thread = new Thread(() => { 
                while(true)
                    ProceedCommand(); 
            });

            thread.Start();
        }

        private static void ProceedCommand()
        {
            Console.WriteLine("Queue length = " + queue.Count);
            
            if (queue.Count == 0)
                Thread.Sleep(40);
            else
            {
                Command command = (Command)queue.Dequeue();

                if (command is QueryStateCommand)
                {
                    var queryStateCommand = command as QueryStateCommand;
                    
                    if (command.SenderConnection == ConnectionManager.PresenterConnection)
                    {
                        bitmap = new Bitmap(queryStateCommand.Width, queryStateCommand.Height);
                        graphics = Graphics.FromImage(bitmap);
                    }

                    if (bitmap == null || graphics == null)
                    {
                        queue.Enqueue(command);
                        return;
                    }

                    ((ServerWebSocketConnection)queryStateCommand.SenderConnection).SendState(ConferenceId, command.SenderConnection == ConnectionManager.PresenterConnection, (int)bitmap.Size.Width, (int)bitmap.Size.Height);

                    if (command.SenderConnection != ConnectionManager.PresenterConnection)
                    {
                        //Отсылаем последний вариант картинки
                        var data = DiffContainer.Split(
                            new KeyValuePair<Rectangle, Bitmap>(
                                new Rectangle(0, 0, (int)bitmap.Size.Width, (int)bitmap.Size.Height), bitmap), 40000);

                        foreach (var d in data)
                        {
                            ((ServerWebSocketConnection)queryStateCommand.SenderConnection).SendDiff(new DiffItem(d));
                        }
                    }
                }
                else if (command is DiffCommand)
                {
                    var diffCommand = command as DiffCommand;

                    graphics.DrawImage(DiffContainer.ByteArrayToImage(diffCommand.DiffItem.ImageBytes),
                                                        diffCommand.DiffItem.X, diffCommand.DiffItem.Y,
                                                        diffCommand.DiffItem.Width, diffCommand.DiffItem.Height);

                    ConnectionManager.BroadcastSendDiff(diffCommand.DiffItem);
                }
            }
        }

        private static void ConnectionManagerOnGetStateCommandReceived(object sender, SimpleEventArgs<QueryStateCommand> connectionEventArgs)
        {
            queue.Enqueue(connectionEventArgs.Value);
            
            
            //ServerWebSocketConnection serverWebSocketConnection = sender as ServerWebSocketConnection;

            //lock (syncObj) 
            //{
            //    if (serverWebSocketConnection == ConnectionManager.PresenterConnection)
            //    {
            //        bitmap = new Bitmap(connectionEventArgs.Value.Width, connectionEventArgs.Value.Height);
            //        graphics = Graphics.FromImage(bitmap);
            //    }

            //    serverWebSocketConnection.SendState(ConferenceId, serverWebSocketConnection == ConnectionManager.PresenterConnection, (int)bitmap.Size.Width, (int)bitmap.Size.Height);

            //    if (serverWebSocketConnection != ConnectionManager.PresenterConnection)
            //    {
                    
                    
            //        //Отсылаем последний вариант картинки
            //        var data = DifferenceLib.DiffContainer.Split(
            //            new KeyValuePair<Rectangle, Bitmap>(
            //                new Rectangle(0, 0, (int) bitmap.Size.Width, (int) bitmap.Size.Height), bitmap), 40000);

            //        foreach (var d in data)
            //        {
            //            serverWebSocketConnection.SendDiff(new DiffItem(d));
            //        }
            //    }
            //}

        }

        private static void ConnectionManagerOnDiffCommandReceived(object sender, SimpleEventArgs<DiffCommand> connectionEventArgs)
        {
            queue.Enqueue(connectionEventArgs.Value);
            
            //lock (syncObj)
            //{
            //    graphics.DrawImage(connectionEventArgs.Value.DiffItem.Bitmap, connectionEventArgs.Value.DiffItem.Rectangle);

            //    ConnectionManager.BroadcastSendDiff(connectionEventArgs.Value.DiffItem);
            //}
        }
    }
}
