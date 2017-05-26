using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public static class ClientMessage
    {

        /// Client messages

        /// Message printed when a client can't connect to the server host 
        /// and port that were passed in.
        public static string CLIENT_CANNOT_CONNECT = "Unable to connect to {0}:{1}";

        /// Message printed before exiting, if the server disconnected.
        public static string CLIENT_SERVER_DISCONNECTED = "Server at {0}:{1} has disconnected";

        /// Printed at the beginning of new lines in the client.
        public static string CLIENT_MESSAGE_PREFIX = "[Me] ";

        /* Message that can be printed over the above prefix to clear it.
           This string has white space at the end to ensure that all of the
           "[Me]" prefix is overwritten with this string. The next string written
           to stdout will need to begin with "\r" to avoid unnecessary white
           space.*/
        public static string CLIENT_WIPE_ME = "\r    ";
    }
}
