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
using System.Threading.Tasks;

namespace CmdMp3CsNetForYoutube
{
    public static class Program {
        internal static Settings Settings;
        private static Server _server;

        private static void PrintHeader()
        {
            Console.WriteLine( $"{Global.Title} [Version {Global.Version}]" );
            Console.WriteLine( Global.Copyright );
            Console.WriteLine(  );
        }

        private static void Main(string[] args) {
#if DEBUG
            Console.Title = $"{Global.Title} (v{Global.Version}) [DEBUG]";
#else
            Console.Title = $"{Global.Title} (v{Global.Version})";
#endif

            PrintHeader( );

            Settings = Settings.Load( );
            if(Settings == null) {
                Console.WriteLine( "There's something wrong with your settings, please check it and try again. It's probably the Developer's fault..." );
                Global.Quit( -1 );
            }

            Console.WriteLine("Press H for help\r\n");
            SoundManager.SelectDevice( );
            _server = new Server(  );
            Task.Run( ( ) => _server.StartServer( ) );

            

            while(true ) {
                if(Console.KeyAvailable) {
                    ConsoleKeyInfo ck = Console.ReadKey( true );

                    switch(ck.Key) {
                        case ConsoleKey.Escape: Global.Quit(  );
                            break;
                        case ConsoleKey.D:
                            SoundManager.SelectDevice( true );
                            break;

                        case ConsoleKey.H:
                            Console.WriteLine( "[Key] - [Description]" );
                            Console.WriteLine( " ESC  -  Exit the Program" );
                            Console.WriteLine( "  D   -  Select Device" );
                            Console.WriteLine( "  H   -  This Help" );
                            Console.WriteLine( " DEL  -  Clear Screen" );
                            Console.WriteLine(  );
                            break;
                        case ConsoleKey.Delete: Console.Clear( );
                            Console.WriteLine( $"[{DateTime.Now:dd MMM, yyy HH:mm:ss}] Console was cleared" );
                            break;
                    }
                }
            }
        }
    }
}
