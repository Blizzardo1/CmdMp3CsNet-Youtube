/** Program.cs
A Soundboard program for the Elgato Stream Deck
  Copyright (C) 2018  Adonis S. Deliannis
  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Net;
using System.Net.Sockets;

namespace CmdMp3CsNetForYoutube
{
    public class Server {
        private const int DefaultPort = 4002;
        private TcpListener _listener;
        private int _port;
        
        public Server( ) {
            _port = Program.Settings.Port == 0 ? DefaultPort : Program.Settings.Port;

            _listener = new TcpListener( IPAddress.Loopback,
                _port );
        }

        public void StartServer(int backlog = 10) {
            _listener.Start( backlog );
            Console.WriteLine( $"Server has started on localhost Port {_port}\r\n" );
            Listen();
        }

        private void Listen()
        {
            while ( true ) {
                var c = new Client( _listener.AcceptTcpClient( ) );
                try {
                    string msg;
                    while((msg = c.ReadLine()) != null) {
                        string[] arr = msg.Split( ' ' ); // GET /[filepath] HTTP/1.1
                        if ( arr.Length < 3 ) continue;

                        string cmd = arr[ 0 ]; // GET
                        string rStr = arr[ 1 ].Substring( 1 ); // /[filepath]
                        
                        switch(cmd.ToUpper()) {
                            case "GET":
                                c.Execute( rStr );
                                break;
                        }
                    }
                }catch(Exception ex) {
                    // Execution is finished
                }
            }
        }
    }
}
