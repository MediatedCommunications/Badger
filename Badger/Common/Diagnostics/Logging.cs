using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common.Diagnostics {
    public static class Logging {

        public static void ApplySimpleConfiguation() {
            var Pattern = SimplePattern();

            ApplyDefaultConfiguration(Pattern);
        }

        public static void ApplyDefaultConfiguration(PatternLayout Pattern = default, AppenderSkeleton[] Appenders = default) {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            

            if (Pattern == default) {
                Pattern = DefaultPattern();
                Pattern.ActivateOptions();
            }

            if (Appenders == default) {
                Appenders = new AppenderSkeleton[] {
                    DefaultDebugAppender(),
                    DefaultConsoleAppender(),
                    DefaultFileAppender(),
                };
            }
            
            foreach (var Appender in Appenders) {
                Appender.Layout = Pattern;
                Appender.ActivateOptions();
                hierarchy.Root.AddAppender(Appender);
            }

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;

        }

        public static RollingFileAppender DefaultFileAppender() {
            return DefaultFileAppender("Logs.txt");
        }

        public static RollingFileAppender DefaultFileAppender(string FileName) {
            return DefaultFileAppender(FileName, true);
        }

        public static RollingFileAppender DefaultFileAppender(string FileName, bool Append) {
            var ret = new RollingFileAppender() {
                File = FileName,
                StaticLogFileName = false,
                AppendToFile = Append,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                DatePattern = "yyyy-MM-dd",
                MaxFileSize = 1024 * 1024 * 10, //10MB,
                MaxSizeRollBackups = 10,
                PreserveLogFileNameExtension = true,
            };

            return ret;
        }

        public static PatternLayout DefaultPattern() {
            var ret = new PatternLayout() {
                ConversionPattern = "%date [%thread] %-10logger %-10level %message%newline",
            };

            return ret;
        }

        public static PatternLayout SimplePattern() {
            var ret = new PatternLayout() {
                ConversionPattern = "%date %message%newline",
            };

            return ret;
        }

        public static DebugAppender DefaultDebugAppender() {
            var ret = new DebugAppender() {
                
            };

            return ret;

        }

        public static ColoredConsoleAppender DefaultConsoleAppender() {
            var ret = new log4net.Appender.ColoredConsoleAppender() {
                
            };

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Emergency,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
                BackColor = log4net.Appender.ColoredConsoleAppender.Colors.Red | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Fatal,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White,
                BackColor = log4net.Appender.ColoredConsoleAppender.Colors.Red | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
            });


            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Alert,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
                BackColor = log4net.Appender.ColoredConsoleAppender.Colors.Red,
            });


            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Critical,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
                BackColor = log4net.Appender.ColoredConsoleAppender.Colors.Red,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Severe,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White,
                BackColor = log4net.Appender.ColoredConsoleAppender.Colors.Red,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Error,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.Red,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Warn,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.Red | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Notice,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.Yellow | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Info,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.Cyan | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Debug,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White | log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Fine,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Trace,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Finer,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Verbose,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White,
            });

            ret.AddMapping(new log4net.Appender.ColoredConsoleAppender.LevelColors() {
                Level = Level.Finest,
                ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.White,
            });


            return ret;
        }


    }
}
