using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Models
{
    public class StoredCredentials
    {
        public string Username { get; set; } = string.Empty;
        public string EncryptedPassword { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}
