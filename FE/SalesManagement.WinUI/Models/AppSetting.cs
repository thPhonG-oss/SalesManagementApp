using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// FE/SalesManagement.WinUI/Models/AppSettings.cs
namespace SalesManagement.WinUI.Models
{
    public class AppSettings
    {
        /// <summary>
        /// Số lượng item mỗi trang (5/10/15/20)
        /// </summary>
        public int ItemsPerPage { get; set; } = 20;

        /// <summary>
        /// Có lưu màn hình cuối cùng không
        /// </summary>
        public bool RememberLastScreen { get; set; } = true;

        /// <summary>
        /// Tên màn hình cuối cùng được mở
        /// </summary>
        public string? LastOpenedScreen { get; set; }

        /// <summary>
        /// Theme của app (Light/Dark)
        /// </summary>
        public string Theme { get; set; } = "Light";

        /// <summary>
        /// Ngôn ngữ hiển thị
        /// </summary>
        public string Language { get; set; } = "vi-VN";
    }
}
