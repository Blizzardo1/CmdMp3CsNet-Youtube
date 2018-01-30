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

using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CmdMp3CsNetForYoutube
{
    public class Client {
        private TcpClient _client;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        
        public Client(TcpClient client) {
            _client = client;
            _reader = new StreamReader( _client.GetStream( ) );
            _writer = new StreamWriter( _client.GetStream( ) );
        }

        public string ReadLine() => _reader.ReadLine();

        public void SendMessage( string text ) {

            var sb = new StringBuilder( );
            sb.AppendLine("HTTP/1.1 200 OK");
            sb.AppendLine("Server: BZTA");
            sb.AppendLine($"Content-Length: {text.Length}");
            sb.AppendLine("Content-Type: text/html");
            sb.AppendLine("Connection: Closed");
            sb.AppendLine();

            _writer.WriteLine( sb.ToString( ) );

            _writer.WriteLine( text );
            _writer.Flush( );
        }

        public void Execute(string file) {
            string[] args = file.Split( '+' );
            string[] prms = null;
            int play = 0, loop = 0;

            foreach ( string s in args ) {
                if(s.Contains("?")) {
                    string[] tmp = s.Split( '?' );
                    prms = tmp[ 1 ].Split( '&' );
                }

                if(prms != null) {
                    foreach ( string p in prms ) {
                        string[] w = p.Split( '=' );
                        string x = w[ 0 ];
                        string y = w[ 1 ];
                        
                        if(x == Program.Settings.PlaybackAlias) {
                            int.TryParse( y, out play );
                            continue;
                        }

                        if (x == Program.Settings.LoopAlias) {
                            int.TryParse( y, out loop );
                        }
                    }
                }
            }
            SoundManager.Play( this, file, play, loop );
        }
    }
}
