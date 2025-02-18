using System;
using System.Collections.Generic;

namespace ServerApp.Models;

public partial class Message
{
    public int Id { get; set; }

    public string ChannelName { get; set; } = null!;

    public int SenderId { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime? SentAt { get; set; }

    public virtual User Sender { get; set; } = null!;
}
