﻿using System;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Cryptography;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a base class for encrypted network sessions.
    /// </summary>
    /// <remarks>
    /// This class provides the packet bufferring and decryption logic for inbound packets,
    /// as well as the logic to write outbound packets.
    /// </remarks>
    public abstract class EncryptedNetworkSession
    {
        #region Events

        /// <summary>
        /// The event is raised when an incoming packet is fully received.
        /// </summary>
        public event EventHandler<PacketReceivedEventArgs> OnPacketReceived;

        /// <summary>
        /// The event is raised before the session is closed.
        /// </summary>
        public event EventHandler OnClosing;

        #endregion

        /// <summary>
        /// The buffer used to store packet headers.
        /// </summary>
        protected BoundedBuffer HeaderBuffer { get; private set; }

        /// <summary>
        /// The buffer used to store packet data.
        /// </summary>
        protected BoundedBuffer PacketBuffer { get; private set; }

        /// <summary>
        /// Gets the internal NetworkSession instance.
        /// </summary>
        protected NetworkSession Session { get; private set; }

        /// <summary>
        /// The cryptographic transformer for this session.
        /// </summary>
        public AbstractCrypto Crypto { get; protected set; }

        /// <summary>
        /// Initializes the internal fields and <see cref="Session"/> with no specified socket.
        /// </summary>
        /// <remarks>
        /// Call <see cref="AttachSocket(Socket)"/> before starting the network operations.</remarks>
        protected EncryptedNetworkSession()
        {
            this.PacketBuffer = new BoundedBuffer();
            this.HeaderBuffer = new BoundedBuffer(4);

            this.Session = new NetworkSession();
            this.Session.OnDataArrived += this.HandleIncomingData;
            this.Session.OnClosing += this.RaiseClosingEvent;      
        }

        /// <summary>
        /// Attaches a <see cref="Socket"/> to this ServerSession instance.
        /// </summary>
        /// <param name="socket">The Socket to attach. </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="socket"/> is <c>null</c>.
        /// </exception>
        public void AttachSocket(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }
            this.Session.AttachSocket(socket);
        }

        private void RaiseClosingEvent(object sender, EventArgs e)
        {
            if (this.OnClosing != null)
            {
                this.OnClosing(this, e);
            }
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public void Close()
        {
            this.OnPacketReceived = null;
            this.Session.Close();
            this.OnClosing = null;
        }

        /// <summary>
        /// Encrypts the given data as a packet 
        /// and writes it to the network stream.
        /// </summary>
        /// <param name="packet">The data to send.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <c>null</c>.
        /// </exception>
        public void WritePacket(byte[] packet)
        {
            if (packet == null) throw new ArgumentNullException("packet");

            if (this.Session.Socket.Connected)
            {
                byte[] rawData = this.Crypto.EncryptAndPack(packet);
                this.Session.Write(rawData);
            }
        }

        /// <summary>
        /// Called when the <see cref="OnPacketReceived"/> event is raised.
        /// </summary>
        /// <remarks>
        /// When overriding this method in a derived class, call the base implementation after your logic.
        /// </remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The packet data that was received.</param>
        protected virtual void HandleIncomingData(object sender, DataArrivedEventArgs args)
        {
            byte[] data = args.Data;
            int position = 0, remaining = data.Length;
            while (this.PacketBuffer.FreeSpace == 0)
            {
                byte[] rawData = this.PacketBuffer.ExtractAndReset(0);
                if (rawData.Length > 0)
                {
                    this.Crypto.Decrypt(rawData);

                    var incomingPacketArgs = new PacketReceivedEventArgs(rawData);
                    this.OnPacketReceived(this, incomingPacketArgs);
                }

                if (remaining == 0) break;

                int bufferred;
                int headerRemaining = this.HeaderBuffer.FreeSpace;
                if (headerRemaining > 0)
                {
                    bufferred = this.HeaderBuffer.AppendFill(data, position, headerRemaining);

                    // For the confused: if we didn't fill the header, it 
                    // means the data array didn't have enough elements.
                    // We move on.
                    if (bufferred < headerRemaining) break;

                    position += bufferred;
                    remaining -= bufferred;
                }

                byte[] header = this.HeaderBuffer.ExtractAndReset(4);
                int length = this.Crypto.TryGetLength(header);
                if (length == -1)
                {
                    this.Close();
                    return;
                }

                this.PacketBuffer.Reset(length);

                bufferred = this.PacketBuffer.AppendFill(data, position, remaining);
                position += bufferred;
                remaining -= bufferred;
            }
        }

        /// <summary>
        /// Checks if the <see cref="OnPacketReceived"/> event has a subscriber and throws 
        /// <see cref="InvalidOperationException"/> if not.
        /// </summary>
        protected void ThrowIfNoPacketReceivedSubscriber()
        {
            if (this.OnPacketReceived == null)
            {
                throw new InvalidOperationException("'OnPacketReceived' has no subscribers.");
            }
        }
    }
}