namespace SqlExport.Common.Data
{
    /// <summary>
    /// A delegate for handling connection messages.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="message">The message.</param>
    /// <param name="lineNumber">The line number.</param>
    public delegate void ConnectionMessageHandler(MessageType type, string message, int? lineNumber);
}
