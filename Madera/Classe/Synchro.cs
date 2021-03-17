using Madera.Jsons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Classe
{
    public class Synchro
    {
        public Synchro()
        {
            this.synchroniseDatas();
        }

        private async void synchroniseDatas()
        {
            await this.LogToApiCS();
            await this.synchroniseRoles();
            await this.synchroniseUsers();
        }

        private async Task<Auth> LogToApiCS()
        {
            Auth auth = await Auth.postAuth("leandreg", "string");
            return auth;
        }

        private async Task<Role[]> synchroniseRoles()
        {
            var roles = await Role.getAllRoles(true);

            return roles;
        }

        private async Task<User[]> synchroniseUsers()
        {
            var users = await User.getAllUsers(true);

            return users;
        }
    }
}
