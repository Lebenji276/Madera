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
            await this.synchroniseClients();
            await this.synchroniseDevis();
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

        private async Task<Devis[]> synchroniseDevis()
        {
            var devis = await Devis.GetAllDevisSynchro();

            return devis;
        }

        private async Task<User[]> synchroniseUsers()
        {
            var users = await User.getAllUsers(true);

            return users;
        }

        private async Task<Client[]> synchroniseClients()
        {
            var clients = await Client.GetAllClientSynchro();

            return clients;
        }

        public static async Task<bool> launchSyncroToAPI()
        {
            try
            {
                await synchroRolesToAPI();
                await synchroClientsToAPI();
                await synchroDevisToAPI();
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        private static async Task<bool> synchroRolesToAPI()
        {
            return await Role.synchroRoleAPI();
        }

        private static async Task<bool> synchroClientsToAPI()
        {
            return await Client.synchroClientsToAPI();
        }

        private static async Task<bool> synchroDevisToAPI()
        {
            return await Devis.synchroDevisToAPI();
        }
    }
}
