using Microsoft.EntityFrameworkCore;
using School.Domain.Location;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Seeds
{
    public static class DefaultLocationData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var existingCountryCodes = context.Countries.Select(c => c.CountryCode).ToHashSet();
            var countries = new List<Country>
            {
                new Country { Name = "Afghanistan", CountryCode = "AF", Currency = "AFN", CurrencySymbol = "؋", IsActive = true },
                new Country { Name = "Albania", CountryCode = "AL", Currency = "ALL", CurrencySymbol = "L", IsActive = true },
                new Country { Name = "Algeria", CountryCode = "DZ", Currency = "DZD", CurrencySymbol = "د.ج", IsActive = true },
                new Country { Name = "Andorra", CountryCode = "AD", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Angola", CountryCode = "AO", Currency = "AOA", CurrencySymbol = "Kz", IsActive = true },
                new Country { Name = "Antigua and Barbuda", CountryCode = "AG", Currency = "XCD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Argentina", CountryCode = "AR", Currency = "ARS", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Armenia", CountryCode = "AM", Currency = "AMD", CurrencySymbol = "֏", IsActive = true },
                new Country { Name = "Australia", CountryCode = "AU", Currency = "AUD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Austria", CountryCode = "AT", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Azerbaijan", CountryCode = "AZ", Currency = "AZN", CurrencySymbol = "₼", IsActive = true },
                new Country { Name = "Bahamas", CountryCode = "BS", Currency = "BSD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Bahrain", CountryCode = "BH", Currency = "BHD", CurrencySymbol = ".د.ب", IsActive = true },
                new Country { Name = "Bangladesh", CountryCode = "BD", Currency = "BDT", CurrencySymbol = "৳", IsActive = true },
                new Country { Name = "Barbados", CountryCode = "BB", Currency = "BBD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Belarus", CountryCode = "BY", Currency = "BYN", CurrencySymbol = "Br", IsActive = true },
                new Country { Name = "Belgium", CountryCode = "BE", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Belize", CountryCode = "BZ", Currency = "BZD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Benin", CountryCode = "BJ", Currency = "XOF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Bhutan", CountryCode = "BT", Currency = "BTN", CurrencySymbol = "Nu.", IsActive = true },
                new Country { Name = "Bolivia", CountryCode = "BO", Currency = "BOB", CurrencySymbol = "Bs.", IsActive = true },
                new Country { Name = "Bosnia and Herzegovina", CountryCode = "BA", Currency = "BAM", CurrencySymbol = "KM", IsActive = true },
                new Country { Name = "Botswana", CountryCode = "BW", Currency = "BWP", CurrencySymbol = "P", IsActive = true },
                new Country { Name = "Brazil", CountryCode = "BR", Currency = "BRL", CurrencySymbol = "R$", IsActive = true },
                new Country { Name = "Brunei", CountryCode = "BN", Currency = "BND", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Bulgaria", CountryCode = "BG", Currency = "BGN", CurrencySymbol = "лв", IsActive = true },
                new Country { Name = "Burkina Faso", CountryCode = "BF", Currency = "XOF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Burundi", CountryCode = "BI", Currency = "BIF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Cabo Verde", CountryCode = "CV", Currency = "CVE", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Cambodia", CountryCode = "KH", Currency = "KHR", CurrencySymbol = "៛", IsActive = true },
                new Country { Name = "Cameroon", CountryCode = "CM", Currency = "XAF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Canada", CountryCode = "CA", Currency = "CAD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Central African Republic", CountryCode = "CF", Currency = "XAF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Chad", CountryCode = "TD", Currency = "XAF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Chile", CountryCode = "CL", Currency = "CLP", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "China", CountryCode = "CN", Currency = "CNY", CurrencySymbol = "¥", IsActive = true },
                new Country { Name = "Colombia", CountryCode = "CO", Currency = "COP", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Comoros", CountryCode = "KM", Currency = "KMF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Congo, Democratic Republic of the", CountryCode = "CD", Currency = "CDF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Congo, Republic of the", CountryCode = "CG", Currency = "XAF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Costa Rica", CountryCode = "CR", Currency = "CRC", CurrencySymbol = "₡", IsActive = true },
                new Country { Name = "Croatia", CountryCode = "HR", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Cuba", CountryCode = "CU", Currency = "CUP", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Cyprus", CountryCode = "CY", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Czech Republic", CountryCode = "CZ", Currency = "CZK", CurrencySymbol = "Kč", IsActive = true },
                new Country { Name = "Denmark", CountryCode = "DK", Currency = "DKK", CurrencySymbol = "kr", IsActive = true },
                new Country { Name = "Djibouti", CountryCode = "DJ", Currency = "DJF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Dominica", CountryCode = "DM", Currency = "XCD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Dominican Republic", CountryCode = "DO", Currency = "DOP", CurrencySymbol = "RD$", IsActive = true },
                new Country { Name = "Ecuador", CountryCode = "EC", Currency = "USD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Egypt", CountryCode = "EG", Currency = "EGP", CurrencySymbol = "£", IsActive = true },
                new Country { Name = "El Salvador", CountryCode = "SV", Currency = "USD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Equatorial Guinea", CountryCode = "GQ", Currency = "XAF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Eritrea", CountryCode = "ER", Currency = "ERN", CurrencySymbol = "Nfk", IsActive = true },
                new Country { Name = "Estonia", CountryCode = "EE", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Eswatini", CountryCode = "SZ", Currency = "SZL", CurrencySymbol = "L", IsActive = true },
                new Country { Name = "Ethiopia", CountryCode = "ET", Currency = "ETB", CurrencySymbol = "Br", IsActive = true },
                new Country { Name = "Fiji", CountryCode = "FJ", Currency = "FJD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Finland", CountryCode = "FI", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "France", CountryCode = "FR", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Gabon", CountryCode = "GA", Currency = "XAF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Gambia", CountryCode = "GM", Currency = "GMD", CurrencySymbol = "D", IsActive = true },
                new Country { Name = "Georgia", CountryCode = "GE", Currency = "GEL", CurrencySymbol = "₾", IsActive = true },
                new Country { Name = "Germany", CountryCode = "DE", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Ghana", CountryCode = "GH", Currency = "GHS", CurrencySymbol = "₵", IsActive = true },
                new Country { Name = "Greece", CountryCode = "GR", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Grenada", CountryCode = "GD", Currency = "XCD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Guatemala", CountryCode = "GT", Currency = "GTQ", CurrencySymbol = "Q", IsActive = true },
                new Country { Name = "Guinea", CountryCode = "GN", Currency = "GNF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Guinea-Bissau", CountryCode = "GW", Currency = "XOF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Guyana", CountryCode = "GY", Currency = "GYD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Haiti", CountryCode = "HT", Currency = "HTG", CurrencySymbol = "G", IsActive = true },
                new Country { Name = "Honduras", CountryCode = "HN", Currency = "HNL", CurrencySymbol = "L", IsActive = true },
                new Country { Name = "Hungary", CountryCode = "HU", Currency = "HUF", CurrencySymbol = "Ft", IsActive = true },
                new Country { Name = "Iceland", CountryCode = "IS", Currency = "ISK", CurrencySymbol = "kr", IsActive = true },
                new Country { Name = "India", CountryCode = "IN", Currency = "INR", CurrencySymbol = "₹", IsActive = true },
                new Country { Name = "Indonesia", CountryCode = "ID", Currency = "IDR", CurrencySymbol = "Rp", IsActive = true },
                new Country { Name = "Iran", CountryCode = "IR", Currency = "IRR", CurrencySymbol = "﷼", IsActive = true },
                new Country { Name = "Iraq", CountryCode = "IQ", Currency = "IQD", CurrencySymbol = "ع.د", IsActive = true },
                new Country { Name = "Ireland", CountryCode = "IE", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Israel", CountryCode = "IL", Currency = "ILS", CurrencySymbol = "₪", IsActive = true },
                new Country { Name = "Italy", CountryCode = "IT", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Jamaica", CountryCode = "JM", Currency = "JMD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Japan", CountryCode = "JP", Currency = "JPY", CurrencySymbol = "¥", IsActive = true },
                new Country { Name = "Jordan", CountryCode = "JO", Currency = "JOD", CurrencySymbol = "د.ا", IsActive = true },
                new Country { Name = "Kazakhstan", CountryCode = "KZ", Currency = "KZT", CurrencySymbol = "₸", IsActive = true },
                new Country { Name = "Kenya", CountryCode = "KE", Currency = "KES", CurrencySymbol = "KSh", IsActive = true },
                new Country { Name = "Kiribati", CountryCode = "KI", Currency = "AUD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Kuwait", CountryCode = "KW", Currency = "KWD", CurrencySymbol = "د.ك", IsActive = true },
                new Country { Name = "Kyrgyzstan", CountryCode = "KG", Currency = "KGS", CurrencySymbol = "som", IsActive = true },
                new Country { Name = "Laos", CountryCode = "LA", Currency = "LAK", CurrencySymbol = "₭", IsActive = true },
                new Country { Name = "Latvia", CountryCode = "LV", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Lebanon", CountryCode = "LB", Currency = "LBP", CurrencySymbol = "ل.ل", IsActive = true },
                new Country { Name = "Lesotho", CountryCode = "LS", Currency = "LSL", CurrencySymbol = "L", IsActive = true },
                new Country { Name = "Liberia", CountryCode = "LR", Currency = "LRD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Libya", CountryCode = "LY", Currency = "LYD", CurrencySymbol = "ل.د", IsActive = true },
                new Country { Name = "Liechtenstein", CountryCode = "LI", Currency = "CHF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Lithuania", CountryCode = "LT", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Luxembourg", CountryCode = "LU", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Madagascar", CountryCode = "MG", Currency = "MGA", CurrencySymbol = "Ar", IsActive = true },
                new Country { Name = "Malawi", CountryCode = "MW", Currency = "MWK", CurrencySymbol = "MK", IsActive = true },
                new Country { Name = "Malaysia", CountryCode = "MY", Currency = "MYR", CurrencySymbol = "RM", IsActive = true },
                new Country { Name = "Maldives", CountryCode = "MV", Currency = "MVR", CurrencySymbol = "Rf", IsActive = true },
                new Country { Name = "Mali", CountryCode = "ML", Currency = "XOF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Malta", CountryCode = "MT", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Marshall Islands", CountryCode = "MH", Currency = "USD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Mauritania", CountryCode = "MR", Currency = "MRU", CurrencySymbol = "UM", IsActive = true },
                new Country { Name = "Mauritius", CountryCode = "MU", Currency = "MUR", CurrencySymbol = "₨", IsActive = true },
                new Country { Name = "Mexico", CountryCode = "MX", Currency = "MXN", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Micronesia", CountryCode = "FM", Currency = "USD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Moldova", CountryCode = "MD", Currency = "MDL", CurrencySymbol = "L", IsActive = true },
                new Country { Name = "Monaco", CountryCode = "MC", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Mongolia", CountryCode = "MN", Currency = "MNT", CurrencySymbol = "₮", IsActive = true },
                new Country { Name = "Montenegro", CountryCode = "ME", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Morocco", CountryCode = "MA", Currency = "MAD", CurrencySymbol = "د.م.", IsActive = true },
                new Country { Name = "Mozambique", CountryCode = "MZ", Currency = "MZN", CurrencySymbol = "MT", IsActive = true },
                new Country { Name = "Myanmar", CountryCode = "MM", Currency = "MMK", CurrencySymbol = "K", IsActive = true },
                new Country { Name = "Namibia", CountryCode = "NA", Currency = "NAD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Nauru", CountryCode = "NR", Currency = "AUD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Nepal", CountryCode = "NP", Currency = "NPR", CurrencySymbol = "₨", IsActive = true },
                new Country { Name = "Netherlands", CountryCode = "NL", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "New Zealand", CountryCode = "NZ", Currency = "NZD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Nicaragua", CountryCode = "NI", Currency = "NIO", CurrencySymbol = "C$", IsActive = true },
                new Country { Name = "Niger", CountryCode = "NE", Currency = "XOF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Nigeria", CountryCode = "NG", Currency = "NGN", CurrencySymbol = "₦", IsActive = true },
                new Country { Name = "North Korea", CountryCode = "KP", Currency = "KPW", CurrencySymbol = "₩", IsActive = true },
                new Country { Name = "North Macedonia", CountryCode = "MK", Currency = "MKD", CurrencySymbol = "ден", IsActive = true },
                new Country { Name = "Norway", CountryCode = "NO", Currency = "NOK", CurrencySymbol = "kr", IsActive = true },
                new Country { Name = "Oman", CountryCode = "OM", Currency = "OMR", CurrencySymbol = "ر.ع.", IsActive = true },
                new Country { Name = "Pakistan", CountryCode = "PK", Currency = "PKR", CurrencySymbol = "₨", IsActive = true },
                new Country { Name = "Palau", CountryCode = "PW", Currency = "USD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Palestine State", CountryCode = "PS", Currency = "ILS", CurrencySymbol = "₪", IsActive = true },
                new Country { Name = "Panama", CountryCode = "PA", Currency = "PAB", CurrencySymbol = "B/.", IsActive = true },
                new Country { Name = "Papua New Guinea", CountryCode = "PG", Currency = "PGK", CurrencySymbol = "K", IsActive = true },
                new Country { Name = "Paraguay", CountryCode = "PY", Currency = "PYG", CurrencySymbol = "₲", IsActive = true },
                new Country { Name = "Peru", CountryCode = "PE", Currency = "PEN", CurrencySymbol = "S/.", IsActive = true },
                new Country { Name = "Philippines", CountryCode = "PH", Currency = "PHP", CurrencySymbol = "₱", IsActive = true },
                new Country { Name = "Poland", CountryCode = "PL", Currency = "PLN", CurrencySymbol = "zł", IsActive = true },
                new Country { Name = "Portugal", CountryCode = "PT", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Qatar", CountryCode = "QA", Currency = "QAR", CurrencySymbol = "ر.ق", IsActive = true },
                new Country { Name = "Romania", CountryCode = "RO", Currency = "RON", CurrencySymbol = "lei", IsActive = true },
                new Country { Name = "Russia", CountryCode = "RU", Currency = "RUB", CurrencySymbol = "₽", IsActive = true },
                new Country { Name = "Rwanda", CountryCode = "RW", Currency = "RWF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Saint Kitts and Nevis", CountryCode = "KN", Currency = "XCD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Saint Lucia", CountryCode = "LC", Currency = "XCD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Saint Vincent and the Grenadines", CountryCode = "VC", Currency = "XCD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Samoa", CountryCode = "WS", Currency = "WST", CurrencySymbol = "T", IsActive = true },
                new Country { Name = "San Marino", CountryCode = "SM", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Sao Tome and Principe", CountryCode = "ST", Currency = "STN", CurrencySymbol = "Db", IsActive = true },
                new Country { Name = "Saudi Arabia", CountryCode = "SA", Currency = "SAR", CurrencySymbol = "ر.س", IsActive = true },
                new Country { Name = "Senegal", CountryCode = "SN", Currency = "XOF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Serbia", CountryCode = "RS", Currency = "RSD", CurrencySymbol = "дин.", IsActive = true },
                new Country { Name = "Seychelles", CountryCode = "SC", Currency = "SCR", CurrencySymbol = "₨", IsActive = true },
                new Country { Name = "Sierra Leone", CountryCode = "SL", Currency = "SLL", CurrencySymbol = "Le", IsActive = true },
                new Country { Name = "Singapore", CountryCode = "SG", Currency = "SGD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Slovakia", CountryCode = "SK", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Slovenia", CountryCode = "SI", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Solomon Islands", CountryCode = "SB", Currency = "SBD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Somalia", CountryCode = "SO", Currency = "SOS", CurrencySymbol = "Sh", IsActive = true },
                new Country { Name = "South Africa", CountryCode = "ZA", Currency = "ZAR", CurrencySymbol = "R", IsActive = true },
                new Country { Name = "South Korea", CountryCode = "KR", Currency = "KRW", CurrencySymbol = "₩", IsActive = true },
                new Country { Name = "South Sudan", CountryCode = "SS", Currency = "SSP", CurrencySymbol = "£", IsActive = true },
                new Country { Name = "Spain", CountryCode = "ES", Currency = "EUR", CurrencySymbol = "€", IsActive = true },
                new Country { Name = "Sri Lanka", CountryCode = "LK", Currency = "LKR", CurrencySymbol = "Rs", IsActive = true },
                new Country { Name = "Sudan", CountryCode = "SD", Currency = "SDG", CurrencySymbol = "ج.س.", IsActive = true },
                new Country { Name = "Suriname", CountryCode = "SR", Currency = "SRD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Sweden", CountryCode = "SE", Currency = "SEK", CurrencySymbol = "kr", IsActive = true },
                new Country { Name = "Switzerland", CountryCode = "CH", Currency = "CHF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Syria", CountryCode = "SY", Currency = "SYP", CurrencySymbol = "£", IsActive = true },
                new Country { Name = "Tajikistan", CountryCode = "TJ", Currency = "TJS", CurrencySymbol = "SM", IsActive = true },
                new Country { Name = "Tanzania", CountryCode = "TZ", Currency = "TZS", CurrencySymbol = "Sh", IsActive = true },
                new Country { Name = "Thailand", CountryCode = "TH", Currency = "THB", CurrencySymbol = "฿", IsActive = true },
                new Country { Name = "Timor-Leste", CountryCode = "TL", Currency = "USD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Togo", CountryCode = "TG", Currency = "XOF", CurrencySymbol = "Fr", IsActive = true },
                new Country { Name = "Tonga", CountryCode = "TO", Currency = "TOP", CurrencySymbol = "T$", IsActive = true },
                new Country { Name = "Trinidad and Tobago", CountryCode = "TT", Currency = "TTD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Tunisia", CountryCode = "TN", Currency = "TND", CurrencySymbol = "د.ت", IsActive = true },
                new Country { Name = "Turkey", CountryCode = "TR", Currency = "TRY", CurrencySymbol = "₺", IsActive = true },
                new Country { Name = "Turkmenistan", CountryCode = "TM", Currency = "TMT", CurrencySymbol = "m", IsActive = true },
                new Country { Name = "Tuvalu", CountryCode = "TV", Currency = "AUD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Uganda", CountryCode = "UG", Currency = "UGX", CurrencySymbol = "Sh", IsActive = true },
                new Country { Name = "Ukraine", CountryCode = "UA", Currency = "UAH", CurrencySymbol = "₴", IsActive = true },
                new Country { Name = "United Arab Emirates", CountryCode = "AE", Currency = "AED", CurrencySymbol = "د.إ", IsActive = true },
                new Country { Name = "United Kingdom", CountryCode = "GB", Currency = "GBP", CurrencySymbol = "£", IsActive = true },
                new Country { Name = "United States of America", CountryCode = "US", Currency = "USD", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Uruguay", CountryCode = "UY", Currency = "UYU", CurrencySymbol = "$", IsActive = true },
                new Country { Name = "Uzbekistan", CountryCode = "UZ", Currency = "UZS", CurrencySymbol = "so'm", IsActive = true },
                new Country { Name = "Vanuatu", CountryCode = "VU", Currency = "VUV", CurrencySymbol = "Vt", IsActive = true },
                new Country { Name = "Venezuela", CountryCode = "VE", Currency = "VES", CurrencySymbol = "Bs.S.", IsActive = true },
                new Country { Name = "Vietnam", CountryCode = "VN", Currency = "VND", CurrencySymbol = "₫", IsActive = true },
                new Country { Name = "Yemen", CountryCode = "YE", Currency = "YER", CurrencySymbol = "﷼", IsActive = true },
                new Country { Name = "Zambia", CountryCode = "ZM", Currency = "ZMW", CurrencySymbol = "ZK", IsActive = true },
                new Country { Name = "Zimbabwe", CountryCode = "ZW", Currency = "ZWL", CurrencySymbol = "$", IsActive = true }
            };
            var countriesToAdd = countries.Where(c => !existingCountryCodes.Contains(c.CountryCode)).ToList();
            if (countriesToAdd.Any())
            {
                context.Countries.AddRange(countriesToAdd);
                await context.SaveChangesAsync();
            }

            var indiaId = context.Countries.FirstOrDefault(c => c.CountryCode == "IN")?.Id ?? 1;
            var existingStateCodes = context.States.Where(s => s.CountryId == indiaId).Select(s => s.StateCode).ToHashSet();

            var states = new List<State>
            {
                new State { Name = "Andhra Pradesh", StateCode = "AP", CountryId = indiaId, IsActive = true },
                new State { Name = "Arunachal Pradesh", StateCode = "AR", CountryId = indiaId, IsActive = true },
                new State { Name = "Assam", StateCode = "AS", CountryId = indiaId, IsActive = true },
                new State { Name = "Bihar", StateCode = "BR", CountryId = indiaId, IsActive = true },
                new State { Name = "Chhattisgarh", StateCode = "CG", CountryId = indiaId, IsActive = true },
                new State { Name = "Goa", StateCode = "GA", CountryId = indiaId, IsActive = true },
                new State { Name = "Gujarat", StateCode = "GJ", CountryId = indiaId, IsActive = true },
                new State { Name = "Haryana", StateCode = "HR", CountryId = indiaId, IsActive = true },
                new State { Name = "Himachal Pradesh", StateCode = "HP", CountryId = indiaId, IsActive = true },
                new State { Name = "Jharkhand", StateCode = "JH", CountryId = indiaId, IsActive = true },
                new State { Name = "Karnataka", StateCode = "KA", CountryId = indiaId, IsActive = true },
                new State { Name = "Kerala", StateCode = "KL", CountryId = indiaId, IsActive = true },
                new State { Name = "Madhya Pradesh", StateCode = "MP", CountryId = indiaId, IsActive = true },
                new State { Name = "Maharashtra", StateCode = "MH", CountryId = indiaId, IsActive = true },
                new State { Name = "Manipur", StateCode = "MN", CountryId = indiaId, IsActive = true },
                new State { Name = "Meghalaya", StateCode = "ML", CountryId = indiaId, IsActive = true },
                new State { Name = "Mizoram", StateCode = "MZ", CountryId = indiaId, IsActive = true },
                new State { Name = "Nagaland", StateCode = "NL", CountryId = indiaId, IsActive = true },
                new State { Name = "Odisha", StateCode = "OR", CountryId = indiaId, IsActive = true },
                new State { Name = "Punjab", StateCode = "PB", CountryId = indiaId, IsActive = true },
                new State { Name = "Rajasthan", StateCode = "RJ", CountryId = indiaId, IsActive = true },
                new State { Name = "Sikkim", StateCode = "SK", CountryId = indiaId, IsActive = true },
                new State { Name = "Tamil Nadu", StateCode = "TN", CountryId = indiaId, IsActive = true },
                new State { Name = "Telangana", StateCode = "TG", CountryId = indiaId, IsActive = true },
                new State { Name = "Tripura", StateCode = "TR", CountryId = indiaId, IsActive = true },
                new State { Name = "Uttar Pradesh", StateCode = "UP", CountryId = indiaId, IsActive = true },
                new State { Name = "Uttarakhand", StateCode = "UK", CountryId = indiaId, IsActive = true },
                new State { Name = "West Bengal", StateCode = "WB", CountryId = indiaId, IsActive = true },
                new State { Name = "Andaman and Nicobar Islands", StateCode = "AN", CountryId = indiaId, IsActive = true },
                new State { Name = "Chandigarh", StateCode = "CH", CountryId = indiaId, IsActive = true },
                new State { Name = "Dadra and Nagar Haveli and Daman and Diu", StateCode = "DN", CountryId = indiaId, IsActive = true },
                new State { Name = "Delhi", StateCode = "DL", CountryId = indiaId, IsActive = true },
                new State { Name = "Jammu and Kashmir", StateCode = "JK", CountryId = indiaId, IsActive = true },
                new State { Name = "Ladakh", StateCode = "LA", CountryId = indiaId, IsActive = true },
                new State { Name = "Lakshadweep", StateCode = "LD", CountryId = indiaId, IsActive = true },
                new State { Name = "Puducherry", StateCode = "PY", CountryId = indiaId, IsActive = true }
            };
            var statesToAdd = states.Where(s => !existingStateCodes.Contains(s.StateCode)).ToList();
            if (statesToAdd.Any())
            {
                context.States.AddRange(statesToAdd);
                await context.SaveChangesAsync();
            }

            var mhId = context.States.FirstOrDefault(s => s.StateCode == "MH" && s.CountryId == indiaId)?.Id ?? 1;
            var dlId = context.States.FirstOrDefault(s => s.StateCode == "DL" && s.CountryId == indiaId)?.Id ?? 1;
            var upId = context.States.FirstOrDefault(s => s.StateCode == "UP" && s.CountryId == indiaId)?.Id ?? 1;
            var kaId = context.States.FirstOrDefault(s => s.StateCode == "KA" && s.CountryId == indiaId)?.Id ?? 1;
            var tnId = context.States.FirstOrDefault(s => s.StateCode == "TN" && s.CountryId == indiaId)?.Id ?? 1;
            var wbId = context.States.FirstOrDefault(s => s.StateCode == "WB" && s.CountryId == indiaId)?.Id ?? 1;
            var gjId = context.States.FirstOrDefault(s => s.StateCode == "GJ" && s.CountryId == indiaId)?.Id ?? 1;
            var tgId = context.States.FirstOrDefault(s => s.StateCode == "TG" && s.CountryId == indiaId)?.Id ?? 1;
            var rjId = context.States.FirstOrDefault(s => s.StateCode == "RJ" && s.CountryId == indiaId)?.Id ?? 1;

            var existingCityCodes = context.Cities.Where(c => c.State.CountryId == indiaId).Select(c => c.CityCode).ToHashSet();

            var cities = new List<City>
            {
                new City { Name = "Mumbai", CityCode = "MUM", StateId = mhId, IsActive = true },
                new City { Name = "Pune", CityCode = "PUN", StateId = mhId, IsActive = true },
                new City { Name = "Nagpur", CityCode = "NAG", StateId = mhId, IsActive = true },
                new City { Name = "New Delhi", CityCode = "NDL", StateId = dlId, IsActive = true },
                new City { Name = "Lucknow", CityCode = "LKO", StateId = upId, IsActive = true },
                new City { Name = "Kanpur", CityCode = "KNP", StateId = upId, IsActive = true },
                new City { Name = "Varanasi", CityCode = "VNS", StateId = upId, IsActive = true },
                new City { Name = "Bengaluru", CityCode = "BLR", StateId = kaId, IsActive = true },
                new City { Name = "Mysuru", CityCode = "MYS", StateId = kaId, IsActive = true },
                new City { Name = "Chennai", CityCode = "MAA", StateId = tnId, IsActive = true },
                new City { Name = "Coimbatore", CityCode = "CJB", StateId = tnId, IsActive = true },
                new City { Name = "Kolkata", CityCode = "CCU", StateId = wbId, IsActive = true },
                new City { Name = "Ahmedabad", CityCode = "AMD", StateId = gjId, IsActive = true },
                new City { Name = "Surat", CityCode = "STV", StateId = gjId, IsActive = true },
                new City { Name = "Hyderabad", CityCode = "HYD", StateId = tgId, IsActive = true },
                new City { Name = "Jaipur", CityCode = "JAI", StateId = rjId, IsActive = true }
            };
            var citiesToAdd = cities.Where(c => !existingCityCodes.Contains(c.CityCode)).ToList();
            if (citiesToAdd.Any())
            {
                context.Cities.AddRange(citiesToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}
