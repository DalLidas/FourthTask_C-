using System.Security.Cryptography;
using System.Text;

namespace FourthTask.Utilities
{
    class EncryptionUtility
    {
        private static string _encryptionKey = "Мы Русские с нами бог";

        // Метод для генерации соли из пароля
        private static string GenerateSaltFromPassword(string password, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] saltBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(saltBytes);
            }
        }


        // Метод для хэширования пароля с солью
        public static string HashPassword(string password)
        {
            // Генерация соли
            string salt = GenerateSaltFromPassword(password, _encryptionKey);

            // Объединяем пароль с солью
            string saltedPassword = password + salt;

            // Хэшируем результат
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashBytes);
            }
        }


        // Метод для проверки пароля
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Генерация соли
            string salt = GenerateSaltFromPassword(password, _encryptionKey);

            // Объединяем пароль с солью
            string saltedPassword = password + salt;

            // Хэшируем результат
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                string computedHash = Convert.ToBase64String(hashBytes);

                // Сравниваем хэши
                return hashedPassword == computedHash;
            }
        }
    }
}