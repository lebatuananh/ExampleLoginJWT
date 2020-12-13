using System.Collections.Generic;
using ExampleLoginJWT.WebAPI.ViewModels;

namespace ExampleLoginJWT.WebAPI.Request
{
    public class UpdatePermissionRequest
    {
        public List<PermissionVm> Permissions { get; set; } = new List<PermissionVm>();

    }
}