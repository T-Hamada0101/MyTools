using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RegEditTools
{
    public static class RegistryHelper
    {
        // HKEY_CURRENT_USER is the default base for overloads

        /// <summary>
        /// 確認ダイアログを表示し、ユーザーの承認を得る
        /// </summary>
        private static bool ShowConfirmationDialog(
            string operationType,
            string subKeyPath,
            string valueName,
            RegistryKey root,
            string? additionalInfo = null
        )
        {
            string rootName = root.Name;
            string message =
                $"レジストリ操作の確認\n\n"
                + $"【操作の種類】 {operationType}\n"
                + $"【ルートキー】 {rootName}\n"
                + $"【サブキー】 {subKeyPath}\n"
                + $"【値の名前】 {valueName}";

            if (!string.IsNullOrEmpty(additionalInfo))
            {
                message += $"\n【追加情報】 {additionalInfo}";
            }

            message += "\n\nこの操作を実行しますか？";

            DialogResult result = MessageBox.Show(
                message,
                "レジストリ操作の確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            return result == DialogResult.Yes;
        }

        // Overload for backward compatibility (HKCU)
        public static string? ReadValue(string subKeyPath, string valueName)
        {
            return ReadValue(Registry.CurrentUser, subKeyPath, valueName);
        }

        public static string? ReadValue(RegistryKey root, string subKeyPath, string valueName)
        {
            // 読み取り操作の確認ダイアログを表示
            if (!ShowConfirmationDialog("レジストリ値の読み取り", subKeyPath, valueName, root))
            {
                return null; // キャンセルされた場合
            }

            try
            {
                using (RegistryKey? key = root.OpenSubKey(subKeyPath))
                {
                    if (key != null)
                    {
                        object? val = key.GetValue(valueName);
                        return val?.ToString();
                    }
                }
            }
            catch (Exception)
            {
                // Log or handle exception if needed
            }
            return null;
        }

        /// <summary>
        /// レジストリに値を書き込む（確認ダイアログ表示）- HKCU overload
        /// </summary>
        public static void WriteValue(string subKeyPath, string valueName, string value)
        {
            WriteValue(Registry.CurrentUser, subKeyPath, valueName, value, showConfirmation: true);
        }

        /// <summary>
        /// レジストリに値を書き込む（確認ダイアログ表示）
        /// </summary>
        public static void WriteValue(
            RegistryKey root,
            string subKeyPath,
            string valueName,
            string value
        )
        {
            WriteValue(root, subKeyPath, valueName, value, showConfirmation: true);
        }

        /// <summary>
        /// レジストリに値を書き込む
        /// </summary>
        public static void WriteValue(
            string subKeyPath,
            string valueName,
            string value,
            bool showConfirmation
        )
        {
            WriteValue(Registry.CurrentUser, subKeyPath, valueName, value, showConfirmation);
        }

        public static void WriteValue(
            RegistryKey root,
            string subKeyPath,
            string valueName,
            string value,
            bool showConfirmation
        )
        {
            // 確認ダイアログを表示する場合
            if (
                showConfirmation
                && !ShowConfirmationDialog(
                    "レジストリ値の書き込み",
                    subKeyPath,
                    valueName,
                    root,
                    $"設定する値: {value}"
                )
            )
            {
                return; // キャンセルされた場合
            }

            // CreateSubKey opens the key if it exists, or creates it if it doesn't.
            // It requests write access.
            using (RegistryKey key = root.CreateSubKey(subKeyPath, true))
            {
                key.SetValue(valueName, value);
            }
        }

        // Overload for HKCU
        public static void DeleteValue(string subKeyPath, string valueName)
        {
            DeleteValue(Registry.CurrentUser, subKeyPath, valueName);
        }

        public static void DeleteValue(RegistryKey root, string subKeyPath, string valueName)
        {
            // 削除操作の確認ダイアログを表示
            if (
                !ShowConfirmationDialog(
                    "レジストリ値の削除",
                    subKeyPath,
                    valueName,
                    root,
                    "⚠️ この操作は元に戻せません"
                )
            )
            {
                return; // キャンセルされた場合
            }

            using (RegistryKey? key = root.OpenSubKey(subKeyPath, true))
            {
                if (key != null)
                {
                    // Check if value exists before trying to delete to avoid throw
                    if (key.GetValue(valueName) != null)
                    {
                        key.DeleteValue(valueName);
                    }
                }
            }
        }
    }
}
