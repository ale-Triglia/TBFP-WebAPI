namespace WebAPI_Sample2.Helper
{
    public static class VerificaCF

    {
        public static bool Check (string codiceFiscale)
        {
            try
            {
                var newCin = CFCalcolaCIN(codiceFiscale, true);

                return newCin == codiceFiscale.ToUpper()[15];

            }
            catch (Exception ex)
            {
                // log ex
                return false;
            }
        }

        /// <summary>
        /// calcola il cin (16esimo carattere) del codice fiscale
        /// </summary>
        /// <param name="cf">codice fiscale</param>
        /// <param name="cinIncluded">se true si aspetta che il codice fiscale comprenda il CIN</param>
        /// <returns>CIN</returns>
        public static char CFCalcolaCIN(string cf, bool cinIncluded)
        {
            const string validNumber0 = "0123456789LMNPQRSTUV"; // lettere per gestire i casi di Omocodia
            const string validLetter1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string validMonth2 = "ABCDEHLMPRST";
            byte[] RE = { 1, 1, 1, 1, 1, 1, 0, 0, 2, 0, 0, 1, 0, 0, 0 }; // 1 = lettera, 0 = Numero o lettera, 2=mese
            int[] TD = { 01, 00, 05, 07, 09, 13, 15, 17, 19, 21, 02, 04, 18, 20, 11, 03, 06, 08, 12, 14, 16, 10, 22, 25, 24, 23 };

            int cin = 0;
            bool dispari = true;

            if (string.IsNullOrWhiteSpace(cf))
                throw new ArgumentException("Parametro invalido");

            cf = cf.ToUpper();

            if (cinIncluded == true)
            {
                // mi aspetto il codice fiscale completo di 16 caratteri
                if (cf.Length != 16)
                    throw new ArgumentException("Lunghezza invalida");

                if (validLetter1.IndexOf(cf[15]) == -1)
                    throw new ArgumentException("Il 16esimo carattere (CIN) deve essere una lettera");
            }
            else
            {
                // mi aspetto il codice fiscale completo di 15 caratteri senza il cin finale
                if (cf.Length != 15)
                    throw new ArgumentException("Lunghezza invalida");
            }

            for (int i = 0; i < 15; i++)
            {
                char c = cf[i];
                // verifica carattere in posizione corretta
                byte rePos = RE[i];
                int v = -1;

                if (rePos == 0) // numeri
                {
                    v = validNumber0.IndexOf(c);
                    if (v > 9)
                    {
                        v = validLetter1.IndexOf(c);
                    }
                }
                else if (rePos == 1)  // lettere
                {
                    v = validLetter1.IndexOf(c);
                }
                else if (rePos == 2) // mese
                {
                    if (validMonth2.IndexOf(c) >= 0)
                    {
                        v = validLetter1.IndexOf(c);
                    }
                }
                if (v == -1)
                {
                    //se c'é una discordanza sulla posizione lettera/numero
                    throw new ArgumentException($"Carattere non valido alla posizione {i + 1}, '{cf.Substring(0, i) + " < " + cf[i] + " > " + cf.Substring(i + 1)}'");
                }

                cin += dispari == true ? TD[v] : v;
                dispari = !dispari;
            }
            cin -= (cin / 26) * 26; //cin = cin - (cin / 26) * 26;
                                    //Ritorna un carattere contenente il CIN
            return validLetter1[cin];
        }
    }
}
