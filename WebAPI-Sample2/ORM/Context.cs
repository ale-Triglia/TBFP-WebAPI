using WebAPI_Sample2.Helper;

namespace WebAPI_Sample2.ORM
{
    public class Context
    {
        #region "--> Dichiarazioni"

        private string dataPath;
        private IConfiguration _configuration;

        #endregion

        #region "--> Costruttori"

        public Context(IConfiguration configuration)
        {
            var path = configuration["DataPath"];
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            dataPath = path;
            _configuration = configuration;
        }

        #endregion

        #region --> Metodi

        #region --> Users

        public string GetUsersPath()
        {
            var filePath = Path.Combine(dataPath, "users.json");
            return filePath;
        }


        public IEnumerable<Models.UserInfo> GetUsers()
        {
            var filePath = GetUsersPath();
            if (!File.Exists(filePath)) return Enumerable.Empty<Models.UserInfo>();
            var js = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(js)) return Enumerable.Empty<Models.UserInfo>();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Models.UserInfo>>(js);
            if (result == null) return Enumerable.Empty<Models.UserInfo>();
            return result;
        }

        public void SaveUsers(IEnumerable<Models.UserInfo> data)
        {
            var filePath = GetUsersPath();
            if (File.Exists(filePath)) File.Delete(filePath);

            var d = data.ToArray();
            var js = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            File.WriteAllText(filePath, js);
        }

        public void AddUser(Models.UserInfo data)
        {
            var users = this.GetUsers().ToList();
            if ((from x in users where x.UserName.ToReal().ToLowerInvariant() == data.UserName.ToReal().ToLowerInvariant() select x).Any())
                throw new Exception(string.Format(@"UserName ""{0}"" already exist.", data.UserName));
            if ((from x in users where x.Email.ToReal().ToLowerInvariant() == data.Email.ToReal().ToLowerInvariant() select x).Any())
                throw new Exception(string.Format(@"Email ""{0}"" already exist.", data.Email));
            if (!VerificaCF.Check(data.CF)) 
                throw new Exception(string.Format(@"CodiceFiscale ""{0}"" formato errato.", data.CF));
            if ((from x in users where x.CF.ToReal().ToLowerInvariant() == data.CF.ToReal().ToLowerInvariant() select x).Any())
                throw new Exception(string.Format(@"CodiceFiscale ""{0}"" already exist.", data.CF));


            if ((from x in users where (x.NDI.ToReal().ToLowerInvariant() + x.TipoDoc.ToString().ToLowerInvariant())== (data.NDI.ToReal().ToLowerInvariant() + data.TipoDoc.ToString().ToLowerInvariant()) select x).Any())
                throw new Exception(string.Format(@"codice documento ""{0}, {1}"" already exist.", data.NDI, data.TipoDoc));

            data.UserId = Guid.NewGuid();
            users.Add(data);
            this.SaveUsers(users);
        }

        #endregion

        #region --> Recensioni

        public string GetRecensioniPath()
        {
            var filePath = Path.Combine(dataPath, "Recensioni.json");
            return filePath;
        }


        public IEnumerable<Models.Recensione> GetRecensioni()
        {
            var filePath = GetRecensioniPath();
            if (!File.Exists(filePath)) return Enumerable.Empty<Models.Recensione>();
            var js = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(js)) return Enumerable.Empty<Models.Recensione>();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Models.Recensione>>(js);
            if (result == null) return Enumerable.Empty<Models.Recensione>();
            return result;
        }

        public void SaveRecensioni(IEnumerable<Models.Recensione> data)
        {
            var filePath = GetRecensioniPath();
            if (File.Exists(filePath)) File.Delete(filePath);

            var d = data.ToArray();
            var js = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            File.WriteAllText(filePath, js);
        }

        public void AddRecensione(Models.Recensione data)
        {
            var Recensioni = this.GetRecensioni().ToList();
       
            data.Id=Guid.NewGuid();
            data.DataRec=DateTime.Now;
            Recensioni.Add(data);
            this.SaveRecensioni(Recensioni);
        }

        #endregion

        #endregion

    }
}
