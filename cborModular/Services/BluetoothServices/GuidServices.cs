using cborModular.DataIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class GuidServices
    {
        private const string AppId = "ab12ef34";

        public static (BluetoothCharakteristicIdentifiers? characteristicType, bool isValid) ParseCustomGuid(Guid guid)
        {
            // Rozdělíme GUID na segmenty
            string guidString = guid.ToString();
            string[] parts = guidString.Split('-');

            // Ověříme, zda má GUID správný počet částí (8-4-4-4-12)
            if (parts.Length != 5)
                return (null, false);

            // Zkontrolujeme, zda první segment odpovídá AppId
            if (parts[0] != AppId)
                return (null, false);

            // Parsujeme druhý segment na číselný kód charakteristiky
            if (int.TryParse(parts[1], out int characteristicCode))
            {
                // Převádíme číselný kód na odpovídající hodnotu výčtu
                if (Enum.IsDefined(typeof(BluetoothCharakteristicIdentifiers), characteristicCode))
                {
                    var characteristicType = (BluetoothCharakteristicIdentifiers)characteristicCode;
                    return (characteristicType, true);
                }
            }

            // Pokud něco nesedí, vracíme, že GUID není platné
            return (null, false);
        }
        public static Guid ReverseGuidByteOrder(byte[] bytes)
        {
            Array.Reverse(bytes);
            // Obrátit první 4 bajty (int)
            Array.Reverse(bytes, 0, 4);
            // Obrátit další 2 bajty (short)
            Array.Reverse(bytes, 4, 2);
            // Obrátit další 2 bajty (short)
            Array.Reverse(bytes, 6, 2);

            return new Guid(bytes);
        }
    }
}
