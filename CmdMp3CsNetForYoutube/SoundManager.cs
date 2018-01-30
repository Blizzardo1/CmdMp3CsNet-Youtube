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
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using NAudio.Wave;

namespace CmdMp3CsNetForYoutube
{
    public static class SoundManager {
        private const string ReplyMessage = "<html><head><title>AudioServ</title></head><body bgcolor=\"black\"></body></html>";

        private static ulong __counter;

        public static DirectSoundDeviceInfo SelectedDevice { get; private set;}


        public static void SelectDevice(bool overrideSettings = false) {
            int i = 0;

            if(Program.Settings.DeviceInfo.IsEmpty() ||
                DirectSoundOut.Devices.FirstOrDefault(f=> f.Description == Program.Settings.DeviceInfo) == null || overrideSettings) {
                foreach ( DirectSoundDeviceInfo dso in DirectSoundOut.Devices ) {
                    Console.WriteLine( $"({i++}) {dso.Description} {( $"- {dso.Guid}" ),25} " );
                }
                Console.WriteLine( $"{i} devices available\r\n" );
                Console.Write( $"Please select a number between 0 and {i - 1}[0]> " );
                int.TryParse( Console.ReadLine( ), out int devi );

                devi = devi > i || devi < 0 ? 0 : devi;
                SelectedDevice = DirectSoundOut.Devices.ToArray( )[ devi ];
                Program.Settings.DeviceInfo = SelectedDevice.Description;
                Program.Settings.Save( );
            } else {
                SelectedDevice = DirectSoundOut.Devices.First( f => f.Description == Program.Settings.DeviceInfo );
            }

            Console.WriteLine( $"Selected device is now \"{SelectedDevice.Description}\"" );
        }

        public static void Play(Client client, string file, int play = 0, int loop = 0) {
            file = new Uri( HttpUtility.UrlDecode( file ) ?? throw new ArgumentNullException( nameof(file) ) )
                .LocalPath;

            if(!File.Exists(file)) {
                return;
            }
            
            Console.WriteLine( $"({__counter++}) [{DateTime.Now:dd MMM, yyyy HH:mm:ss}] Audio: \"{file}\"" );
            p( file, play, loop );
            client.SendMessage( ReplyMessage );
        }

        private static void p(string f, int p, int l) {
            if ( f.StartsWith( "robots.txt" ) ) return;
            if (f.IsEmpty() || !File.Exists(f)) return; // Yeah, I know, Another check for File.Exists... It's a safetynet, sowhat!?

            using(var ar = new AudioFileReader(f)) {
                using (var dso = new DirectSoundOut(SelectedDevice.Guid)) {
                    dso.Init( ar );

                    for ( int i = 0; i < l + 1; i++ ) {
                        ar.CurrentTime = TimeSpan.FromMilliseconds( p );
                        
                        dso.Play(  );

                        while(dso.PlaybackState == PlaybackState.Playing) {
                            Thread.Sleep( 1 );
                        }
                    }
                }
                ar.Dispose();
            }
        }
    }
}
