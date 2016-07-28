﻿using System;
using System.Net.Sockets;

namespace OpenStory.Server.Networking
{
    /// <summary>
    /// Represents an EventArgs wrapper around a Socket.
    /// </summary>
    public sealed class SocketEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the socket of this instance.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketEventArgs"/> class.
        /// </summary>
        /// <param name="socket">The socket for this instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="socket"/> is <see langword="null"/>.</exception>
        public SocketEventArgs(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            this.Socket = socket;
        }
    }
}
