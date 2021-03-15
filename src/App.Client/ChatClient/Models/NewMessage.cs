namespace App.Client.ChatClient.Models
{
    internal sealed class NewMessage
    {
        public string Message { get; set; }
        
        public string Group { get; set; }
        
        public string Nick { get; set; }
    }
}