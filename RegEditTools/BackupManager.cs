using System;
using System.IO;
using System.Text.Json;

namespace RegEditTools
{
    using Microsoft.Win32;

    public class RegBackupData
    {
        public string RootKeyName { get; set; } = "HKEY_CURRENT_USER";
        public string KeyPath { get; set; } = string.Empty;
        public string ValueName { get; set; } = string.Empty;
        public string? ValueData { get; set; }
        public bool ValueExists { get; set; }
    }

    public static class BackupManager
    {
        private static string BackupDir =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");

        private static void EnsureBackupDir()
        {
            if (!Directory.Exists(BackupDir))
            {
                Directory.CreateDirectory(BackupDir);
            }
        }

        public static void SaveBackup(string backupName, string keyPath, string valueName)
        {
            SaveBackup(backupName, Registry.CurrentUser, keyPath, valueName);
        }

        public static void SaveBackup(
            string backupName,
            RegistryKey root,
            string keyPath,
            string valueName
        )
        {
            EnsureBackupDir();

            string? currentValue = RegistryHelper.ReadValue(root, keyPath, valueName);

            var data = new RegBackupData
            {
                RootKeyName = root.Name,
                KeyPath = keyPath,
                ValueName = valueName,
                ValueData = currentValue,
                ValueExists = currentValue != null,
            };

            string json = JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions { WriteIndented = true }
            );
            string filePath = Path.Combine(BackupDir, $"{backupName}.json");

            File.WriteAllText(filePath, json);
        }

        public static void RestoreBackup(string backupName)
        {
            string filePath = Path.Combine(BackupDir, $"{backupName}.json");
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<RegBackupData>(json);

            if (data == null)
                return;

            RegistryKey root = Registry.CurrentUser;
            if (!string.IsNullOrEmpty(data.RootKeyName))
            {
                if (data.RootKeyName.Contains("HKEY_LOCAL_MACHINE"))
                    root = Registry.LocalMachine;
                else if (data.RootKeyName.Contains("HKEY_CLASSES_ROOT"))
                    root = Registry.ClassesRoot;
                else if (data.RootKeyName.Contains("HKEY_USERS"))
                    root = Registry.Users;
                else if (data.RootKeyName.Contains("HKEY_CURRENT_CONFIG"))
                    root = Registry.CurrentConfig;
                // else HKCU
            }

            if (data.ValueExists && data.ValueData != null)
            {
                RegistryHelper.WriteValue(root, data.KeyPath, data.ValueName, data.ValueData);
            }
            else
            {
                RegistryHelper.DeleteValue(root, data.KeyPath, data.ValueName);
            }
        }

        public static bool BackupExists(string backupName)
        {
            return File.Exists(Path.Combine(BackupDir, $"{backupName}.json"));
        }
    }
}
