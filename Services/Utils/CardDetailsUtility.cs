using System;
using System.Text;

namespace PaymentGateway.Services.Utils
{
    public class CardDetailsUtility
    {
        public static string MaskCardNumber(string cardNumber)
        {
            // todo what if card number is too short?
            var sb = new StringBuilder(cardNumber);
            sb[6] = '*';
            sb[7] = '*';
            sb[8] = '*';
            sb[9] = '*';
            sb[10] = '*';
            sb[11] = '*';
            return sb.ToString();
        }
    }
}