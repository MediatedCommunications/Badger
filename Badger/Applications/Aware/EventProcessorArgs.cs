using System;

namespace Badger.Applications.Aware {
    public class EventProcessorArgs : EventArgs {
        public EventProcessorEventType EventType { get; set; }
        public Version Version { get; set; }
    }
}
