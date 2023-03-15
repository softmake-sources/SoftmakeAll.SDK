using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using SoftmakeAll.SDK.Helpers.String.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Networking.Http
{
    public class ResponseMessage : SoftmakeAll.SDK.Networking.Http.Message
    {
        #region Constructor
        public ResponseMessage(System.Net.HttpStatusCode StatusCode) : this(StatusCode, System.TimeSpan.FromMilliseconds(0.0D)) { }
        public ResponseMessage(System.Net.HttpStatusCode StatusCode, System.TimeSpan ElapsedTime) : base()
        {
            this.StatusCode = StatusCode;
            this.ElapsedTime = ElapsedTime;
        }
        #endregion

        #region Properties
        public System.Net.HttpStatusCode StatusCode { get; }
        public System.TimeSpan ElapsedTime { get; }
        public System.Boolean IsSuccessStatusCode { get => ((int)this.StatusCode >= 200) && ((int)this.StatusCode <= 299); }
        public System.String StatusDescription { get => this.StatusCode.ToString(); }
        public System.String Status { get => $"{(int)this.StatusCode} - {this.StatusDescription}"; }

        private SoftmakeAll.SDK.Networking.Http.ResponseFileDetails _FileDetails;
        public SoftmakeAll.SDK.Networking.Http.ResponseFileDetails FileDetails { get => this._FileDetails; }
        #endregion

        #region Methods
        public override void AddCookie(System.Net.Cookie Cookie)
        {
            if ((Cookie != null) && (!(System.String.IsNullOrWhiteSpace(Cookie.Name))))
                base.AddCookie(Cookie);
        }
        internal void SetFileDetails(System.String ContentDisposition, System.String ContentType, System.Byte[] Content)
        {
            this._FileDetails = null;
            if (System.String.IsNullOrWhiteSpace(ContentDisposition))
                return;

            System.String FileExtension = null;
            System.String[] SplittedContentDisposition = ContentDisposition.Split("; ");
            System.String FileName = SplittedContentDisposition.FirstOrDefault(d => d.ToLower().StartsWith("filename="));
            if (!(System.String.IsNullOrWhiteSpace(FileName)))
            {
                FileName = FileName[9..^0];
                FileExtension = new System.IO.FileInfo(FileName).Extension;
            }

            this._FileDetails = new SoftmakeAll.SDK.Networking.Http.ResponseFileDetails(SplittedContentDisposition.Any(d => d == "attachment"), FileName, (System.String.IsNullOrWhiteSpace(ContentType) ? "application/octet-stream" : ContentType), SoftmakeAll.SDK.Networking.MIMETypes.GetMIMEType(FileExtension), Content);
        }

        public System.String ReadBodyAsString() => this.ReadBodyAsString(true, false);
        public System.String ReadBodyAsString(System.Boolean TreatUnicodeChars) => this.ReadBodyAsString(TreatUnicodeChars, false);
        public System.String ReadBodyAsString(System.Boolean TreatUnicodeChars, System.Boolean KeepBodyBytes)
        {
            if (base.Body == null)
                return null;

            if (base.Body.Length == 0)
                return "";

            System.String Result = System.Text.Encoding.UTF8.GetString(base.Body);

            if (!(KeepBodyBytes))
                base.ClearBody();

            if (TreatUnicodeChars)
                return Result.TreatUnicodeChars();

            return Result;
        }

        public System.Text.Json.JsonElement ReadBodyAsJSON() => this.ReadBodyAsJSON(true, false);
        public System.Text.Json.JsonElement ReadBodyAsJSON(System.Boolean TreatUnicodeChars) => this.ReadBodyAsJSON(TreatUnicodeChars, false);
        public System.Text.Json.JsonElement ReadBodyAsJSON(System.Boolean TreatUnicodeChars, System.Boolean KeepBodyBytes)
        {
            System.Text.Json.JsonElement Result = this.ReadBodyAsString(TreatUnicodeChars, true).ToJsonElement();
            if ((Result.ValueKind != System.Text.Json.JsonValueKind.Undefined) && (!(KeepBodyBytes))) base.ClearBody();
            return Result;
        }

        public System.Xml.Linq.XElement ReadBodyAsXML() => this.ReadBodyAsXML(true, false);
        public System.Xml.Linq.XElement ReadBodyAsXML(System.Boolean TreatUnicodeChars) => this.ReadBodyAsXML(TreatUnicodeChars, false);
        public System.Xml.Linq.XElement ReadBodyAsXML(System.Boolean TreatUnicodeChars, System.Boolean KeepBodyBytes)
        {
            try
            {
                System.Xml.Linq.XElement Result = System.Xml.Linq.XElement.Parse(this.ReadBodyAsString(TreatUnicodeChars, true));
                if (!(KeepBodyBytes)) base.ClearBody();
                return Result;
            }
            catch { }

            return null;
        }
        #endregion
    }
}