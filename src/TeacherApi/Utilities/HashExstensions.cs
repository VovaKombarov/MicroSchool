using System.Security.Cryptography;
using System.Text;
using System;
using System.Diagnostics;

namespace TeacherApi.Utilities
{
    /// <summary>
    /// Расширение для хэширования строки.
    /// </summary>
    public static class HashExtensions
    {
        /// <summary>
        /// Создает строку хэшированную SHA256.
        /// </summary>
        /// <param name="input">входная строка.</param>
        /// <returns>хэшированния строка.</returns>
        public static string Sha256(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Возвращает массив типа byte хэшированный Sha256.
        /// </summary>
        /// <param name="input">Входной массив.</param>
        /// <returns>Хэшированный массив.</returns>
        public static byte[] Sha256(this byte[] input)
        {
            if (input == null)
            {
                return null;
            }

            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(input);
            }
        }
    }
}


