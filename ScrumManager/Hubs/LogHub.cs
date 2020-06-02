using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ScrumManager.Models;

namespace ScrumManager.Hubs
{
    public class LogHub : Hub
    {
        public async Task AddToGroup(string group)
        {
            System.Diagnostics.Debug.WriteLine("Added connection: " + Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            //await Clients.Group(group).SendAsync("LogAdded", new Log());
        }


        public async Task RemoveFromGroup(string group)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }

        public async Task LogAdded(string group, Log log)
        {
            //System.Diagnostics.Debug.WriteLine("Added log to conn: " + Context.ConnectionId);

            if (Clients != null)
            {
                //await Clients.All.SendAsync("LogAdded", log);
                await Clients.Group(group).SendAsync("LogAdded", log);
            }
        }

        public async Task LogModified(string group, Log log)
        {
            //System.Diagnostics.Debug.WriteLine("Modified log to conn: " + Context.ConnectionId);

            if (Clients != null)
            {
                //await Clients.All.SendAsync("LogModified", log);
                await Clients.Group(group).SendAsync("LogModified", log);
            }
        }

        public async Task LogRemoved(string group, Log log)
        {
            if (Clients != null)
            {
                //await Clients.All.SendAsync("LogRemoved", log);
                await Clients.Group(group).SendAsync("LogRemoved", log);
            }
        }
    }
}