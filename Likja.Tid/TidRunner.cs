using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Likja.Tid.Extensions;
using Likja.Tid.Logger;
using Newtonsoft.Json;

namespace Likja.Tid
{
    internal class TidRunner
    {
        private static readonly string CONFIG_FILE_NAME = TidStrings.R1;

        private string _command;
        private string _parameter;
        private readonly ILogger _logger;

        public TidRunner(ILogger logger)
        {
            _logger = logger;
        }

        public void Run(string command, string parameter)
        {
            _command = command;
            _parameter = parameter;

            switch (_command.ToLower())
            {
                case "init":
                    Init();
                    break;

                case "add":
                    Add();
                    break;

                case "show":
                    Show();
                    break;

                case "start":
                    Start();
                    break;

                case "done":
                    Done();
                    break;

                case "purge":
                    Purge();
                    break;

                case "help":
                    ShowHelp();
                    break;

                case "edit":
                    Edit();
                    break;

                default:
                    _logger.LogError(TidStrings.R2);
                    break;
            }
        }

        private bool IsInitialized(bool showWarning = true)
        {
            if (File.Exists(CONFIG_FILE_NAME)) return true;
            if (showWarning)
                _logger.LogWarning(TidStrings.R3);

            return false;
        }

        private bool ChangeStatus(EntryStatus status)
        {
            var config = ReadConfig();
            var entryToModify = config.GetEntryByIdNumber(_parameter);

            if (entryToModify == null)
            {
                _logger.LogWarning(TidStrings.R4, _parameter);
                return false;
            }

            entryToModify.Status = status;
            SaveConfig(config);
            return true;
        }

        private static TidConfig ReadConfig()
        {
            var tid = File.ReadAllText(CONFIG_FILE_NAME);
            return JsonConvert.DeserializeObject<TidConfig>(tid.FromBase64());
        }

        private static void SaveConfig(TidConfig config)
        {
            var configSerialized = JsonConvert.SerializeObject(config);
            File.WriteAllText(CONFIG_FILE_NAME, configSerialized.ToBase64());
        }

        private static string GenerateId(string code, int entryCount)
        {
            var nextId = entryCount + 1;

            return nextId < 10
                ? string.Format("{0}-00{1}", code.ToUpper(), nextId)
                : string.Format(nextId < 100
                    ? "{0}-0{1}"
                    : "{0}-{1}", code.ToUpper(), nextId);
        }

        private void Init()
        {
            // check if .tid already exists
            if (IsInitialized(showWarning: false))
            {
                _logger.LogWarning(TidStrings.R5);
                return;
            }

            // names should be at least three letters
            if (_parameter.Length < 3)
            {
                _logger.LogWarning(TidStrings.R6);
                return;
            }

            var config = new TidConfig()
            {
                Name = _parameter,
                Code = _parameter.ToUpper().Substring(0, 3),
                Entries = new List<TidEntry>()
            };

            SaveConfig(config);
        }

        private void Add()
        {
            if (!IsInitialized()) return;

            var config = ReadConfig();

            var entry = new TidEntry
            {
                Id = GenerateId(config.Code, config.Entries.Count()),
                Details = _parameter,
                Status = EntryStatus.Todo
            };

            config.Entries.Add(entry);
            SaveConfig(config);

            _logger.LogInfo(TidStrings.R7, entry.Id);
            //Show();
        }

        private void Show()
        {
            if (!IsInitialized()) return;

            var config = ReadConfig();

            var statuses = new List<EntryStatus>() { EntryStatus.InProgress, EntryStatus.Todo, EntryStatus.Done };

            statuses.ForEach(status =>
            {
                _logger.LogWarning("[{0}]", status.ToString());

                bool colorSwitcher = true;
                config.Entries.Where(x => x.Status == status).ToList().ForEach(o =>
                {
                    if (colorSwitcher)
                    {
                        _logger.LogInfo("  {0}: {1}", o.Id, o.Details);
                    }
                    else
                    {
                        var oldColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("  {0}: {1}", o.Id, o.Details);
                        Console.ForegroundColor = oldColor;
                    }
                    colorSwitcher = !colorSwitcher;
                });
                Console.WriteLine();
            });

        }

        private void Start()
        {
            if (!IsInitialized()) return;
            if (!ChangeStatus(EntryStatus.InProgress)) return;
            _logger.LogInfo(TidStrings.R8, _parameter);
        }

        private void Done()
        {
            if (!IsInitialized()) return;
            if (!ChangeStatus(EntryStatus.Done)) return;
            _logger.LogInfo(TidStrings.R9, _parameter);
        }

        private void Purge()
        {
            if (!IsInitialized()) return;
            if (!ChangeStatus(EntryStatus.Archive)) return;
            _logger.LogInfo(TidStrings.R10, _parameter);
        }

        private void Edit()
        {
            if (!IsInitialized()) return;

            var config = ReadConfig();
            var id = _parameter.Split(':')[0];
            var newDetails = _parameter.Split(':')[1];
            var entry = config.GetEntryByIdNumber(id);

            if (entry == null)
            {
                _logger.LogWarning(string.Format("Task ID {0} not found.", id));
                return;
            }

            entry.Details = newDetails;
            SaveConfig(config);
        }

        public static int ShowHelp()
        {
            var _logger = new ConsoleLogger();
            _logger.LogInfo(TidStrings.R11);
            _logger.LogInfo("------------------------------------------");
            _logger.LogInfo(TidStrings.R12);
            _logger.LogInfo("\n");
            _logger.LogInfo(TidStrings.R13);
            _logger.LogInfo("  " + TidStrings.R14);
            _logger.LogInfo("  " + TidStrings.R15);
            _logger.LogInfo("  " + TidStrings.R16);
            _logger.LogInfo("  " + TidStrings.R17);
            _logger.LogInfo("  " + TidStrings.R18);
            _logger.LogInfo("  " + TidStrings.R19);
            _logger.LogInfo("  " + TidStrings.R20);

            return -1;
        }

        public static string ShowUsage(string command)
        {
            switch (command.ToLower())
            {
                case "init":
                    return TidStrings.R21;
                case "add":
                    return TidStrings.R22;
                case "start":
                    return TidStrings.R23;
                case "done":
                    return TidStrings.R24;
                case "purge":
                    return TidStrings.R25;
                case "edit":
                    return TidStrings.R26;
            }
            return "";
        }
    }
}
