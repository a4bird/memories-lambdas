﻿using System.Net;
using System.Net.Sockets;

namespace Memories.Image.Ingestor.Tests.Infrastructure
{
    public static class PortHelper
    {
        // Copied from https://github.com/aspnet/KestrelHttpServer/blob/47f1db20e063c2da75d9d89653fad4eafe24446c/test/Microsoft.AspNetCore.Server.Kestrel.FunctionalTests/AddressRegistrationTests.cs#L508
        public static int GetNextAvailablePort()
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Let the OS assign the next available port. Unless we cycle through all ports
            // on a test run, the OS will always increment the port number when making these calls.
            // This prevents races in parallel test runs where a test is already bound to
            // a given port, and a new test is able to bind to the same port due to port
            // reuse being enabled by default by the OS.
            socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            return ((IPEndPoint)socket.LocalEndPoint).Port;
        }
    }
}
