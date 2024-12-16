using System.Collections.Generic;

namespace ChatkaReservation.Models
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public IList<string> UserRoles { get; set; } = new List<string>();
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}