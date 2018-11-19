using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core.Models
{
    public class DropboxUser
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DropboxAccessToken { get; set; }
        public string ExactAccessToken { get; set; }
        public string ExactRefreshToken { get; set; }
        public DateTime? ExactAccessTokenExpiration { get; set; }

        public static DropboxUser Empty
        {
            get { return new DropboxUser(); }
        }

        public override bool Equals(object obj)
        {
            var other = obj as DropboxUser;

            if (other != null)
                return this.Id == other.Id
                    && this.UserId == other.UserId
                    && this.DropboxAccessToken == other.DropboxAccessToken
                    && this.ExactAccessToken == other.ExactAccessToken
                    && this.ExactRefreshToken == other.ExactRefreshToken
                    && this.ExactAccessTokenExpiration == other.ExactAccessTokenExpiration;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return
                this.Id.GetHashCode() ^
                this.UserId.GetHashCode() ^
                this.DropboxAccessToken.GetHashCode() ^
                this.ExactAccessToken.GetHashCode() ^
                this.ExactRefreshToken.GetHashCode() ^
                this.ExactAccessTokenExpiration.GetHashCode();
        }
    }
}
