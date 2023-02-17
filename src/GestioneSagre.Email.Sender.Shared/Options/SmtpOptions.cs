﻿using MailKit.Security;

namespace GestioneSagre.Email.Sender.Shared.Options;

public class SmtpOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public SecureSocketOptions Security { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Sender { get; set; }
}