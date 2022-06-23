using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NReco_HtmlToPdf.Models
{
    public class DLVModel
    {
        [JsonProperty("odata.metadata")]
        public string OdataMetadata { get; set; }

        [JsonProperty("odata.type")]
        public string OdataType { get; set; }

        [JsonProperty("odata.id")]
        public string OdataId { get; set; }

        [JsonProperty("odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("odata.editLink")]
        public string OdataEditLink { get; set; }

        [JsonProperty("AttachmentFiles@odata.navigationLinkUrl")]
        public string AttachmentFilesOdataNavigationLinkUrl { get; set; }
        public List<object> AttachmentFiles { get; set; }

        [JsonProperty("L_Status@odata.navigationLinkUrl")]
        public string LStatusOdataNavigationLinkUrl { get; set; }
        public LStatus L_Status { get; set; }

        [JsonProperty("DLVCreator@odata.navigationLinkUrl")]
        public string DLVCreatorOdataNavigationLinkUrl { get; set; }
        public DLVCreator DLVCreator { get; set; }
        public int Id { get; set; }
        public object Title { get; set; }
        public object DLVDescription { get; set; }
        public string DLVID { get; set; }
        public string DLVItemDescription { get; set; }
        public string DLVTitle { get; set; }
        public string DLVType { get; set; }
        public DateTime DLVValidFrom { get; set; }
        public DateTime DLVValidTill { get; set; }
        public double GSTAmount { get; set; }
        public string GSTCurrency { get; set; }
        public string GSTPercentage { get; set; }
        public string Provider { get; set; }
        public string ScopeText { get; set; }
        public string DeptHeadDesignation { get; set; }

        public string AreGoodsInvolved { get; set; }
        public string AreServicesRelatedToExibition { get; set; }
        public string CurrencyType { get; set; }
        public string EventOrganizer { get; set; }
        public string LocationOfGoods { get; set; }
        public double Margin { get; set; }
        public string NameOfSupplierOfGoods { get; set; }
        public string NatureOfTransation { get; set; }
        public string OwnershipOfGoods { get; set; }
        public string PAN { get; set; }
        public string Reason { get; set; }
        public string ServiceType { get; set; }
        public string ThirdParty { get; set; }
        public double TotalPriceAgreed { get; set; }
        public string PlaceOfEvent { get; set; }
        public string ol_Department { get; set; }
        public object Receiver { get; set; }
        public string ProviderShortName { get; set; }
        public object ReceiverIncorporated { get; set; }
        public string ConsiderationText { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderIncorporated { get; set; }
        public string ProviderCompleteAddress { get; set; }
        public object ReceiverCompleteAddress { get; set; }
        public object GSTClauseText { get; set; }
        public string ReceiverEmail { get; set; }
        public object ReceiverShortName { get; set; }
        public string ProviderBusinessActivities { get; set; }
        public object ReceiverBusinessActivities { get; set; }
        public string NoticeReceiver { get; set; }
        public string NoticeSAVWIPL { get; set; }
        public string ProviderCostCenter { get; set; }
        public string ReceiverCostCenter { get; set; }
        public string MiscelleneousClauses { get; set; }
        public double BasicCost { get; set; }
        public object secondPartySignature01 { get; set; }
        public object secondPartySignature02 { get; set; }
        public string GSTApplicable { get; set; }
        public string IndirectTaxComment { get; set; }
        public string NoticePeriod { get; set; }
        public object OtherNoticePeriod { get; set; }
        public int ID { get; set; }
        public string LocationOfEvent { get; set; }
        public string isPerpetualDLV { get; set; }
        public string otherPartyDesignation { get; set; }
        public string otherPartyDepartment { get; set; }
        public string savwiplDesignation { get; set; }
        public string savwiplDepartment { get; set; }
        public Settings Configurations { get; set; }
        public string AdditionalConsideration { get; set; }
        public string signatoryDesignation01 { get; set; }
        public string signatoryDepartment01 { get; set; }
        public string signatoryDesignation02 { get; set; }
        public string signatoryDepartment02 { get; set; }
    }
    public class DLVCreator
    {
        [JsonProperty("odata.type")]
        public string OdataType { get; set; }

        [JsonProperty("odata.id")]
        public string OdataId { get; set; }
        public string Title { get; set; }
    }

    public class LStatus
    {
        [JsonProperty("odata.type")]
        public string OdataType { get; set; }

        [JsonProperty("odata.id")]
        public string OdataId { get; set; }
        public string StatusName { get; set; }
    }

    public class Settings
    {
        public string ViewName { get; set; }
        public string OutputFileName { get; set; }
        public string CustomWkHtmlCommand { get; set; }
        public Margin Margin { get; set; }
        public PDFDetails PDFDetails { get; set; }
        //public string ViewName { get; set; }
    }

    public class Margin
    {
        public string All { get; set; }
        public string Bottom { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
    }

    public class PDFDetails
    {
        public string Title { get; set; }
    }

}