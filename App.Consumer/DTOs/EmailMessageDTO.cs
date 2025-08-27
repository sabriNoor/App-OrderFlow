using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Consumer.DTOs
{
    public record class EmailMessageDTO
    {
        public string Email { get; init; } = string.Empty;
        public string Subject { get; init; }= string.Empty;
        public string Body { get; init; }= string.Empty;
        
    }
}