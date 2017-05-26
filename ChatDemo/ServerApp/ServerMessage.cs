using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public static class ServerMessage
    {
        /// Server messages 

        /// The client sent a control message (a message starting with "/") 
        /// that doesn't exist (e.g., /foobar).
        public static string SERVER_INVALID_CONTROL_MESSAGE = 
            "{} is not a valid control message. Valid messages are /create, /list, and /join.";

        /// Message returned when a client attempts to join a channel that doesn't exist.
        public static string SERVER_NO_CHANNEL_EXISTS =
            "No channel named {0} exists. Try '/create {0}'?";

        /// Message sent to a client that uses the "/join" command without a channel name.
        public static string SERVER_JOIN_REQUIRES_ARGUMENT =
            "/join command must be followed by the name of a channel to join.";

        /// Message sent to all clients in a channel when a new client joins.
        public static string SERVER_CLIENT_JOINED_CHANNEL = "{0} has joined";

        /// Message sent to all clients in a channel when a client leaves.
        public static string SERVER_CLIENT_LEFT_CHANNEL = "{0} has left";

        /// Message sent to a client that tries to create a channel that doesn't exist.
        public static string SERVER_CHANNEL_EXISTS =
            "Room {0} already exists, so cannot be created.";

        /// Message sent to a client that uses the "/create" command without a channel name.
        public static string SERVER_CREATE_REQUIRES_ARGUMENT = 
            "/create command must be followed by the name of a channel to create";

        /// Message sent to a client that sends a regular message before joining any channels.
        public static string SERVER_CLIENT_NOT_IN_CHANNEL = 
            "Not currently in any channel. Must join a channel before sending messages.";
    }
}
