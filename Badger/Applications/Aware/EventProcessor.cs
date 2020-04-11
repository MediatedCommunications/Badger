using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Applications.Aware {


    public static class EventProcessor {
        private static string[] Keys_Install = new[] {
            "--squirrel-install"
        };

        private static string[] Keys_Update = new[] {
            "--squirrel-updated"
        };

        private static string[] Keys_Obsolete = new[] {
            "--squirrel-obsolete"
        };

        private static string[] Keys_Uninstall = new[] {
            "--squirrel-uninstall"
        };

        private static string[] Keys_FirstRun = new[] {
            "--squirrel-firstrun"
        };


        /// <summary>
        /// Call this method as early as possible in app startup. This method
        /// will dispatch to your methods to set up your app. Depending on the
        /// parameter, your app will exit after this method is called, which 
        /// is required by Squirrel. UpdateManager has methods to help you to
        /// do this, such as CreateShortcutForThisExe.
        /// </summary>
        /// <param name="onInstall">
        /// Called when your app is initially installed. Set up app shortcuts here as well as file associations.
        /// Triggers a Process Exit.
        /// </param>
        /// <param name="onUpdate">
        /// When an app is updated, the updated instance calls onUpdate.
        /// Triggers a Process Exit.
        /// </param>
        /// <param name="onObsolete">
        /// When an app is updated, the old version calls onObsolete.
        /// Triggers a Process Exit.
        /// </param>
        /// <param name="onUninstall">
        /// Called when your app is uninstalled by Add/Remove Programs.
        /// Triggers a Process Exit.
        /// </param>
        /// <param name="onFirstRun">
        /// Called the first time the application is run.
        /// Does not trigger a Process Exit.
        /// </param>
        /// <param name="arguments">
        /// If provided, use these command line arguments instead of the default ones.
        /// </param>
        /// <returns></returns>
        public static async Task<EventProcessorEventType> HandleEvents(
            Func<EventProcessorArgs, Task> onInstall = null,
            Func<EventProcessorArgs, Task> onUpdate = null,
            Func<EventProcessorArgs, Task> onObsolete = null,
            Func<EventProcessorArgs, Task> onUninstall = null,
            Func<EventProcessorArgs, Task> onFirstRun = null,
            Func<EventProcessorArgs, Task> onNone = null,
            string[] arguments = null) {

            //Get our command line arguments.  We skip one because the first argument is our process name.
            arguments = arguments ?? System.Environment.GetCommandLineArgs().Skip(1).ToArray();

            //Get the first event name.
            var EventString = arguments.Length >= 1 ? arguments[0] : "";

            //If there is a second parameter, try to parse it as a version.
            var Version = new System.Version();
            if(arguments.Length >= 2 && System.Version.TryParse(arguments[1], out var NewVersion)) {
                Version = NewVersion;
            }

            //A mapping between Events and the keys that trigger them.
            var KeysToEvents = new Dictionary<EventProcessorEventType, string[]> {
                [EventProcessorEventType.FirstRun] = Keys_FirstRun,
                [EventProcessorEventType.Install] = Keys_Install,
                [EventProcessorEventType.Update] = Keys_Update,
                [EventProcessorEventType.Obsolete] = Keys_Obsolete,
                [EventProcessorEventType.Uninstall] = Keys_Uninstall,
            };

            //Get the event that triggered.  If not match, the Default value will be "None".
            var CurrentEventType = (
                from Entry in KeysToEvents
                from EntryKey in Entry.Value
                where string.Equals(EntryKey, EventString, StringComparison.InvariantCultureIgnoreCase)
                select Entry.Key
                ).FirstOrDefault();


            var EventArgs = new EventProcessorArgs() {
                EventType = CurrentEventType,
                Version = Version,
            };

            //Set up a mapping between the event and their associated actions. 
            var EventsToActions = new Dictionary<EventProcessorEventType, Func<EventProcessorArgs, Task>> {
                [EventProcessorEventType.FirstRun] = onFirstRun,
                [EventProcessorEventType.Install] = onInstall,
                [EventProcessorEventType.Update] = onUpdate,
                [EventProcessorEventType.Obsolete] = onObsolete,
                [EventProcessorEventType.Uninstall] = onUninstall,
                [EventProcessorEventType.None] = onNone,
            };

            //Trigger the action if we can.
            if(EventsToActions.TryGetValue(CurrentEventType, out var Action) && Action != null) {
                await Action(EventArgs)
                    .ConfigureAwait(false)
                    ;
            }

            if(CurrentEventType != EventProcessorEventType.FirstRun && CurrentEventType != EventProcessorEventType.None) {
                System.Environment.Exit(0);
            }

            return CurrentEventType;
        }


    }
}
