﻿namespace Disco.DomainModel
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Avatar { get; set; }
        public bool Bot { get; set; }
        public string Email { get; set; }
    }
}
