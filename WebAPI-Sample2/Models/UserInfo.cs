using System.ComponentModel.DataAnnotations;
using WebAPI_Sample2.Helper;

namespace WebAPI_Sample2.Models
{
    public enum eTipoDocumento
    {
        ci,pt,ps

    }
    public class UserInfo
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        [Required]
        public string? DisplayName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public DateTime? CreatedDate { get; set; }

        [Required]
        public string? NTel { get; set; }
        public eTipoDocumento TipoDoc { get; set; }
        [Required]
        public string? NDI { get; set; }


        [Required]
        public string? CF { get; set; }
        [Required]
        public int? DataN { get; set; }

        public int eta { get{

                var d = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day;
                return (d - this.DataN.ToReal()) / 10000;
                    
                    
                    } 
        }

        
    
      
    }
}

